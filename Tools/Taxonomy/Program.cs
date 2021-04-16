// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;
using Taxonomy.Cwe;

namespace Taxonomy
{
    class Program
    {
        static void Main(string[] args)
        {
            // example:
            // generatecwe "..\..\..\..\Source\cwec_v4.4.xml" "..\..\..\..\..\CWE_v4.4.sarif"
            // generateowasp "..\..\..\..\Source\OWASP Application Security Verification Standard 4.0.2-en.csv" "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif"
            // generatenistsp80053 "..\..\..\..\Source\sp800-53r5-control-catalog.csv" "..\..\..\..\..\NIST_SP800-53_v5.sarif"
            // generatenistsp80063b "..\..\..\..\Source\800-63-3-nist-pages\sp800-63b" "..\..\..\..\..\NIST_SP800-63B_v1.sarif"
            // addowasprelationshiptocwe "..\..\..\..\..\CWE_v4.4.sarif" "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" "..\..\..\..\..\CWE_v4.4.sarif"

            var result = Parser.Default.ParseArguments<CweOptions, OwaspOptions, NistSP80053Options, GenerateNistSP80063BOptions, AddOwaspRelationshipToCweOptions>(args)
            .MapResult(
                (CweOptions o) =>
                {
                    GenerateCwe(o);
                    return true;
                },
                (OwaspOptions o) =>
                {
                    GenerateOwasp(o);
                    return true;
                },
                (NistSP80053Options o) =>
                {
                    GenerateNistSP80053(o);
                    return true;
                },
                (GenerateNistSP80063BOptions o) =>
                {
                    GenerateNistSP80063B(o);
                    return true;
                },
                (AddOwaspRelationshipToCweOptions o) =>
                {
                    AddOwaspRelationshipToCwe(o);
                    return true;
                },
                e => false);
        }

        private static void AddOwaspRelationshipToCwe(AddOwaspRelationshipToCweOptions o)
        {
            var generator = new CweTaxonomyGenerator();
            generator.AddOwaspRelationshipToSarif(o.CweFilePath, o.OwaspFilePath, o.TargetFilePath);
        }

        private static void GenerateNistSP80053(NistSP80053Options o)
        {
            var generator = new NistSP80053TaxonomyGenerator();
            generator.SaveToSarif(o.SourceFilePath, o.TargetFilePath);
        }

        private static void GenerateNistSP80063B(GenerateNistSP80063BOptions o)
        {
            var generator = new NistSP80063BTaxonomyGenerator();
            generator.SaveToSarif(o.SourceFolderPath, o.TargetFilePath);
        }

        private static void GenerateOwasp(OwaspOptions o)
        {
            var generator = new OwaspASVSTaxonomyGenerator();
            generator.SaveToSarif(o.SourceFilePath, o.TargetFilePath);
        }

        private static void GenerateCwe(CweOptions o)
        {
            var generator = new CweTaxonomyGenerator();
            generator.SaveXmlToSarif(o.SourceFilePath, o.TargetFilePath);
        }
    }
}
