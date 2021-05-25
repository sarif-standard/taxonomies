// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using CommandLine;

using Taxonomy.Cwe;

namespace Taxonomy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // example:
            // generate-cwe --source-file-path "..\..\..\..\Source\cwec_v4.4.xml" --target-file-path "..\..\..\..\..\CWE_v4.4.sarif" --version "4.4" --release-date "2020-12-10"
            // generate-owasp --source-file-path "..\..\..\..\Source\OWASP Application Security Verification Standard 4.0.2-en.csv" --target-file-path "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" --version "4.0.2" --release-date "2020-10-01"
            // generate-nistsp80053csv --source-file-path "..\..\..\..\Source\sp800-53r5-control-catalog.csv" --target-file-path "..\..\..\..\..\NIST_SP800-53_v5.sarif" --version "5" --release-date "2020-12-10"
            // generate-nistsp80053json --source-file-path "..\..\..\..\Source\NIST_SP-800-53_rev4_catalog.json" --target-file-path "..\..\..\..\..\NIST_SP800-53_v4.sarif" --version "4" --release-date "2015-01-22"
            // generate-nistsp80063b --Source-folder-path "..\..\..\..\Source\800-63-3-nist-pages\sp800-63b" --target-file-path "..\..\..\..\..\NIST_SP800-63B_v1.sarif" --version "1" --release-date "2020-03-02"
            // add-owasprelationship-to-cwe --source-cwe-file-path "..\..\..\..\..\CWE_v4.4.sarif" --source-owasp-file-path "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" --target-file-path "..\..\..\..\..\CWE_v4.4.sarif"

            bool result = Parser.Default.ParseArguments<CweOptions, OwaspOptions, NistSP80053CsvOptions, NistSP80053JsonOptions, NistSP80063BOptions, AddOwaspRelationshipToCweOptions>(args)
            .MapResult(
                (CweOptions o) =>
                {
                    return GenerateCwe(o);
                },
                (OwaspOptions o) =>
                {
                    return GenerateOwasp(o);
                },
                (NistSP80053CsvOptions o) =>
                {
                    return GenerateNistSP80053Csv(o);
                },
                (NistSP80053JsonOptions o) =>
                {
                    return GenerateNistSP80053Json(o);
                },
                (NistSP80063BOptions o) =>
                {
                    return GenerateNistSP80063B(o);
                },
                (AddOwaspRelationshipToCweOptions o) =>
                {
                    return AddOwaspRelationshipToCwe(o);
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

        private static bool GenerateNistSP80053Csv(NistSP80053CsvOptions o)
        {
            var generator = new NistSP80053CsvTaxonomyGenerator();
            return generator.SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
        }

        private static bool GenerateNistSP80053Json(NistSP80053JsonOptions o)
        {
            var generator = new NistSP80053JsonTaxonomyGenerator();
            return generator.SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
        }

        private static bool GenerateNistSP80063B(NistSP80063BOptions o)
        {
            var generator = new NistSP80063BTaxonomyGenerator();
            return generator.SaveToSarif(o.SourceFolderPath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
        }

        private static bool GenerateOwasp(OwaspOptions o)
        {
            var generator = new OwaspASVSTaxonomyGenerator();
            return generator.SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
        }

        private static bool GenerateCwe(CweOptions o)
        {
            var generator = new CweTaxonomyGenerator();
            return generator.SaveXmlToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
        }
    }
}
