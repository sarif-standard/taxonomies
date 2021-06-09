// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using CommandLine;

using Taxonomy.Cwe;

using Tools.Wasc;

namespace Taxonomy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // example:
            // add-owasprelationship-to-cwe --source-cwe-file-path "..\..\..\..\..\CWE_v4.4.sarif" --source-owasp-file-path "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" --target-file-path "..\..\..\..\..\CWE_v4.4.sarif"
            // generate-cwe --source-file-path "..\..\..\..\Source\cwec_v4.4.xml" --target-file-path "..\..\..\..\..\CWE_v4.4.sarif" --version "4.4" --release-date "2020-12-10"
            // generate-nist --type sp80053 --source-file-path "..\..\..\..\Source\sp800-53r5-control-catalog.csv" --target-file-path "..\..\..\..\..\NIST_SP800-53_v5.sarif" --version "5" --release-date "2020-12-10"
            // generate-nist --type sp80053 --source-file-path "..\..\..\..\Source\NIST_SP-800-53_rev4_catalog.json" --target-file-path "..\..\..\..\..\NIST_SP800-53_v4.sarif" --version "4" --release-date "2015-01-22"
            // generate-nist --type sp80063b --Source-folder-path "..\..\..\..\Source\800-63-3-nist-pages\sp800-63b" --target-file-path "..\..\..\..\..\NIST_SP800-63B_v1.sarif" --version "1" --release-date "2020-03-02"
            // generate-owasp --type asvs --source-file-path "..\..\..\..\Source\OWASP Application Security Verification Standard 4.0.2-en.csv" --target-file-path "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" --version "4.0.2" --release-date "2020-10-01"
            // generate-owasp --type mobiletop10 --source-file-path "..\..\..\..\Source\www-project-mobile-top-10-master\2014-risks" --target-file-path "..\..\..\..\..\OWASP_MobileTop10_v2014.sarif" --version "2014" --release-date "2014-01-01"
            // generate-owasp --type mobiletop10 --source-file-path "..\..\..\..\Source\www-project-mobile-top-10-master\2016-risks" --target-file-path "..\..\..\..\..\OWASP_MobileTop10_v2016.sarif" --version "2016" --release-date "2016-01-01"
            // generate-wasc --source-file-path "..\..\..\..\Source\wasc_1.00.csv" --target-file-path "..\..\..\..\..\WASC_1.00.sarif" --version "1.00" --release-date "2004-01-01"
            // generate-wasc --source-file-path "http://projects.webappsec.org/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View" --target-file-path "..\..\..\..\..\WASC_2.00.sarif" --version "2.00" --release-date "2010-01-01"

            bool result = Parser.Default.ParseArguments<AddOwaspRelationshipToCweOptions, CweOptions, NistOptions, OwaspOptions, WascOptions>(args)
            .MapResult(
                (CweOptions o) =>
                {
                    return GenerateCwe(o);
                },
                (OwaspOptions o) =>
                {
                    return GenerateOwasp(o);
                },
                (NistOptions o) =>
                {
                    return GenerateNist(o);
                },
                (AddOwaspRelationshipToCweOptions o) =>
                {
                    return AddOwaspRelationshipToCwe(o);
                },
                (WascOptions o) =>
                {
                    return GenerateWasc(o);
                },
                e => false);

            if (result)
            {
                Console.WriteLine("Execution completed without errors.");
            }
            else
            {
                Console.WriteLine("Execution failed.");
            }
        }

        private static bool AddOwaspRelationshipToCwe(AddOwaspRelationshipToCweOptions o)
        {
            var generator = new CweTaxonomyGenerator();
            return generator.AddOwaspRelationshipToSarif(o.CweFilePath, o.OwaspFilePath, o.TargetFilePath);
        }

        private static bool GenerateNist(NistOptions o)
        {
            switch (o.Type)
            {
                case "sp80053":
                    if (o.SourceFilePath.EndsWith(".json"))
                    {
                        return new NistSP80053JsonTaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
                    }
                    else
                    {
                        return new NistSP80053CsvTaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
                    }
                case "sp80063b":
                    return new NistSP80063BTaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
                default:
                    return false;
            }
        }

        private static bool GenerateOwasp(OwaspOptions o)
        {
            return o.Type switch
            {
                "asvs" => new OwaspASVSTaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version),
                "mobiletop10" => new OwaspMobileTop10TaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version),
                _ => false,
            };
        }

        private static bool GenerateCwe(CweOptions o)
        {
            var generator = new CweTaxonomyGenerator();
            return generator.SaveXmlToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
        }

        private static bool GenerateWasc(WascOptions o)
        {
            return o.Version switch
            {
                "1.00" => new WascV1Generator().SaveToSarifAsync(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc).Result,
                "2.00" => new WascV2Generator().SaveToSarifAsync(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc).Result,
                _ => false,
            };
        }
    }
}
