// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using CommandLine;

using Taxonomy.Cwe;
using Taxonomy.Disa;

using Tools.Pci;
using Tools.Wasc;

namespace Taxonomy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // see README.MD for usages
            bool result = Parser.Default.ParseArguments<AddOwaspRelationshipToCweOptions, CweOptions, DisaOptions, NistOptions, OwaspOptions, PciOptions, WascOptions>(args)
            .MapResult(
                (AddOwaspRelationshipToCweOptions o) =>
                {
                    return AddOwaspRelationshipToCwe(o);
                },
                (CweOptions o) =>
                {
                    return GenerateCwe(o);
                },
                (DisaOptions o) =>
                {
                    return GenerateDisa(o);
                },
                (NistOptions o) =>
                {
                    return GenerateNist(o);
                },
                (OwaspOptions o) =>
                {
                    return GenerateOwasp(o);
                },
                (PciOptions o) =>
                {
                    return GeneratePci(o);
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
            return o.Type switch
            {
                "asvs" => new OwaspASVSTaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version),
                "mobiletop10" => new OwaspMobileTop10TaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version),
                "top10" => new OwaspTop10TaxonomyGenerator().SaveToSarif(o.SourceFilePath, o.TargetFilePath, o.Version),
                _ => false,
            };
        }

        private static bool GeneratePci(PciOptions o)
        {
            return o.Type switch
            {
                "ssf" => new PciSsfGenerator().SaveToSarifAsync(o.SourceFilePath, o.TargetFilePath, o.Version).Result,
                "dss" => new PciDssGenerator(o.Version).SaveToSarifAsync(o.SourceFilePath, o.TargetFilePath, o.Version).Result,
                _ => false,
            };
        }

        private static bool GenerateCwe(CweOptions o)
        {
            return new CweTaxonomyGenerator().SaveXmlToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.Type);
        }

        private static bool GenerateDisa(DisaOptions o)
        {
            return new DisaTaxonomyGenerator().SaveXmlToSarif(o.SourceFilePath, o.TargetFilePath, o.Version, o.Type);
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
