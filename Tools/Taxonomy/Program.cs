// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using CommandLine;

using Taxonomy.Cwe;

using Tools.Pic;
using Tools.Wasc;

namespace Taxonomy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // example:
            // add-owasprelationship-to-cwe --source-cwe-file-path "..\..\..\..\..\CWE_v4.4.sarif" --source-owasp-file-path "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" --target-file-path "..\..\..\..\..\CWE_v4.4.sarif"
            // generate-cwe --source-file-path "..\..\..\..\Source\cwec_v4.4.xml" --target-file-path "..\..\..\..\..\CWE_v4.4.sarif" --version "4.4"
            // generate-nist --type sp80053 --source-file-path "..\..\..\..\Source\sp800-53r5-control-catalog.csv" --target-file-path "..\..\..\..\..\NIST_SP800-53_v5.sarif" --version "5"
            // generate-nist --type sp80053 --source-file-path "..\..\..\..\Source\NIST_SP-800-53_rev4_catalog.json" --target-file-path "..\..\..\..\..\NIST_SP800-53_v4.sarif" --version "4"
            // generate-nist --type sp80063b --Source-folder-path "..\..\..\..\Source\800-63-3-nist-pages\sp800-63b" --target-file-path "..\..\..\..\..\NIST_SP800-63B_v1.sarif" --version "1"
            // generate-owasp --source-file-path "..\..\..\..\Source\OWASP Application Security Verification Standard 4.0.2-en.csv" --target-file-path "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" --version "4.0.2"
            // generate-pci --type ssf --source-file-path "..\..\..\..\Source\pci_ssf_v1.1.csv" --target-file-path "..\..\..\..\..\PCI_SSF_V1.1.sarif" --version "1.1"
            // generate-wasc --source-file-path "..\..\..\..\Source\wasc_1.00.csv" --target-file-path "..\..\..\..\..\WASC_1.00.sarif" --version "1.00"
            // generate-wasc --source-file-path "http://projects.webappsec.org/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View" --target-file-path "..\..\..\..\..\WASC_2.00.sarif" --version "2.00"

            bool result = Parser.Default.ParseArguments<AddOwaspRelationshipToCweOptions, CweOptions, NistOptions, OwaspOptions, PicOptions, WascOptions>(args)
            .MapResult(
                (AddOwaspRelationshipToCweOptions o) =>
                {
                    return AddOwaspRelationshipToCwe(o);
                },
                (CweOptions o) =>
                {
                    return GenerateCwe(o);
                },
                (NistOptions o) =>
                {
                    return GenerateNist(o);
                },
                (OwaspOptions o) =>
                {
                    return GenerateOwasp(o);
                },
                (PicOptions o) =>
                {
                    return GeneratePic(o);
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
                        return new NistSP80053JsonTaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version);
                    }
                    else
                    {
                        return new NistSP80053CsvTaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version);
                    }
                case "sp80063b":
                    return new NistSP80063BTaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version);
                default:
                    return false;
            }
        }

        private static bool GenerateOwasp(OwaspOptions o)
        {
            var generator = new OwaspASVSTaxonomyGenerator();
            return generator.SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version);
        }

        private static bool GeneratePic(PicOptions o)
        {
            return o.Type switch
            {
                "ssf" => new PicSsfGenerator().SaveToSarifAsync(o.SourceFilePath, o.TargetFilePath, o.Version).Result,
                // "dss" => new PicDssGenerator().SaveToSarifAsync(o.SourceFilePath, o.TargetFilePath, o.Version).Result,
                _ => false,
            };
        }

        private static bool GenerateCwe(CweOptions o)
        {
            var generator = new CweTaxonomyGenerator();
            return generator.SaveXmlToSarif(o.SourceFilePath, o.TargetFilePath, o.Version);
        }

        private static bool GenerateWasc(WascOptions o)
        {
            return o.Version switch
            {
                "1.00" => new WascV1Generator().SaveToSarifAsync(o.SourceFilePath, o.TargetFilePath, o.Version).Result,
                "2.00" => new WascV2Generator().SaveToSarifAsync(o.SourceFilePath, o.TargetFilePath, o.Version).Result,
                _ => false,
            };
        }
    }
}
