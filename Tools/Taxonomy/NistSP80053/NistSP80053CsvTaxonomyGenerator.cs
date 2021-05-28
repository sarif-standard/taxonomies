// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using CsvHelper;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Common;

namespace Taxonomy
{
    public class NistSP80053CsvTaxonomyGenerator : TaxonomyGenerator
    {
        public bool SaveToSarif(string sourceFilePath, string targetFilePath, string version, string releaseDateUtc)
        {
            try
            {
                List<NistSP80053CsvRecord> results = this.ReadFromCsv(sourceFilePath);

                Run run = this.ConvertToSarif(results, version, releaseDateUtc);

                SarifLog log = new SarifLog
                {
                    Runs = new Run[] { run }
                };

                File.WriteAllText(targetFilePath, JsonConvert.SerializeObject(log, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        private List<NistSP80053CsvRecord> ReadFromCsv(string csvFilePath)
        {
            using FileStream input = File.Open(csvFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var textReader = new StreamReader(input);
            using var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);

            return csvReader.GetRecords<NistSP80053CsvRecord>().ToList();
        }

        private Run ConvertToSarif(List<NistSP80053CsvRecord> records, string version, string releaseDateUtc)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent toolComponent = new ToolComponent
            {
                Name = Constants.NistSP80053V5.Name,
                Guid = Constants.NistSP80053V5.Guid,
                Version = version,
                ReleaseDateUtc = releaseDateUtc,
                InformationUri = new Uri("https://csrc.nist.gov/publications/detail/sp/800-53/rev-5/final"),
                DownloadUri = new Uri("https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-53r5.pdf"),
                Organization = "National Institute of Standards and Technology",
                ShortDescription = new MultiformatMessageString { Text = "Security and Privacy Controlsfor Information Systems and Organizations" },
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = new List<ToolComponentReference>(),
            };

            records.ForEach(r => toolComponent.Taxa.Add(new ReportingDescriptor
            {
                Id = r.Id,
                Name = r.Name,
                ShortDescription = new MultiformatMessageString { Text = r.ShortDescription },
                FullDescription = string.IsNullOrEmpty(r.FullDescription) ? null : new MultiformatMessageString { Text = r.FullDescription },
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
                Relationships = this.GetRelationships(r.Related),
            }));

            taxonomies.Add(toolComponent);

            var tool = new Tool { Driver = new ToolComponent { Name = $"NIST SP800-53 v{version}" } };

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies
            };

            return run;
        }

        private List<ReportingDescriptorRelationship> GetRelationships(string relationshipString)
        {
            if (string.IsNullOrEmpty(relationshipString))
            {
                return null;
            }

            var idList = relationshipString.Split(new char[] { ',', '.', ';' }, StringSplitOptions.TrimEntries).Where(p => p.ToLower() != "none").Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
            List<ReportingDescriptorRelationship> relationships = new List<ReportingDescriptorRelationship>();
            foreach (string id in idList)
            {
                relationships.Add(new ReportingDescriptorRelationship
                {
                    Target = new ReportingDescriptorReference
                    {
                        Id = id,
                    },
                    Kinds = new string[] { "relevant" },
                });
            }

            return relationships;
        }
    }
}
