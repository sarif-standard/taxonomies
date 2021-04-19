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
            // generate-cwe "..\..\..\..\Source\cwec_v4.4.xml" "..\..\..\..\..\CWE_v4.4.sarif" "4.4" "2020-12-10"
            // generate-owasp "..\..\..\..\Source\OWASP Application Security Verification Standard 4.0.2-en.csv" "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" "4.0.2" "2020-10-01"
            // generate-nistsp80053 "..\..\..\..\Source\sp800-53r5-control-catalog.csv" "..\..\..\..\..\NIST_SP800-53_v5.sarif" "5" "2020-12-10"
            // generate-nistsp80063b "..\..\..\..\Source\800-63-3-nist-pages\sp800-63b" "..\..\..\..\..\NIST_SP800-63B_v1.sarif" "1" "2020-03-02"
            // add-owasprelationship-to-cwe "..\..\..\..\..\CWE_v4.4.sarif" "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" "..\..\..\..\..\CWE_v4.4.sarif"

            bool result = Parser.Default.ParseArguments<CweOptions, OwaspOptions, NistSP80053Options, GenerateNistSP80063BOptions, AddOwaspRelationshipToCweOptions>(args)
            .MapResult(
                (CweOptions o) =>
                {
                    return GenerateCwe(o);
                },
                (OwaspOptions o) =>
                {
                    return GenerateOwasp(o);
                },
                (NistSP80053Options o) =>
                {
                    return GenerateNistSP80053(o);
                },
                (GenerateNistSP80063BOptions o) =>
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

        private static bool GenerateNistSP80053(NistSP80053Options o)
        {
            var generator = new NistSP80053TaxonomyGenerator();
            return generator.SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.ReleaseDateUtc);
        }

        private static bool GenerateNistSP80063B(GenerateNistSP80063BOptions o)
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
