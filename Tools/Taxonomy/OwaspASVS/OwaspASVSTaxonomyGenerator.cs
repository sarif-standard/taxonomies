// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CsvHelper;
using Microsoft.CodeAnalysis.Sarif;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Taxonomy.Common;

namespace Taxonomy
{
    public class OwaspASVSTaxonomyGenerator : TaxonomyGenerator
    {
        public List<OwaspASVSCsvRecord> ReadFromCsv(string csvFilePath)
        {
            using FileStream input = File.Open(csvFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var textReader = new StreamReader(input);
            using var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);

            return csvReader.GetRecords<OwaspASVSCsvRecord>().ToList();
        }

        public void SaveToSarif(string sourceFilePath, string targetFilePath)
        {
            var results = this.ReadFromCsv(sourceFilePath);

            var run = this.ConvertToSarif(results);

            SarifLog log = new SarifLog
            {
                Runs = new Run[] { run }
            };

            File.WriteAllText(targetFilePath, JsonConvert.SerializeObject(log, Formatting.Indented));
        }

        public Run ConvertToSarif(List<OwaspASVSCsvRecord> records)
        {
            var supportedTaxonomies = new List<ToolComponentReference>();
            supportedTaxonomies.Add(new ToolComponentReference() { Guid = Constants.Guid.Cwe, Name = "CWE" });
            supportedTaxonomies.Add(new ToolComponentReference() { Guid = Constants.Guid.NistSP80063B, Name = "Nist SP800-63B" });

            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent toolComponent = new ToolComponent
            {
                Name = "OWASP",
                Guid = Constants.Guid.Owasp,
                Version = "4.0.2",
                ReleaseDateUtc = "2020-10-01",
                InformationUri = new Uri("https://owasp.org/www-project-application-security-verification-standard/"),
                DownloadUri = new Uri("https://github.com/OWASP/ASVS/raw/v4.0.2/4.0/OWASP%20Application%20Security%20Verification%20Standard%204.0.2-en.pdf"),
                Organization = "OWASP Foundation",
                ShortDescription = new MultiformatMessageString { Text = "OWASP Application Security Verification Standard" },
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = supportedTaxonomies,
            };

            records.ForEach(r => toolComponent.Taxa.Add(new ReportingDescriptor
            {
                Id = r.Id.Replace("V", ""),
                FullDescription = string.IsNullOrEmpty(r.FullDescription) ? null : new MultiformatMessageString { Text = r.FullDescription },
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
                Relationships = this.GetRelationships(r.RelatedCwe, r.RelatedNistSP80063B, toolComponent.Name),
            }));

            taxonomies.Add(toolComponent);

            var tool = new Tool { Driver = new ToolComponent { Name = "OWASP ASVS v4.0.2" } };

            ExternalPropertyFileReferences externalPropertyFileReferences = new ExternalPropertyFileReferences();
            externalPropertyFileReferences.Taxonomies = new List<ExternalPropertyFileReference>();
            externalPropertyFileReferences.Taxonomies.Add(new ExternalPropertyFileReference()
            {
                Guid = Constants.Guid.Cwe,
                Location = new ArtifactLocation() { Uri = new Uri("https://raw.githubusercontent.com/sarif-standard/taxonomies/5a8490df0cee6a0e7a8d4a210f8cfffbe3a5d319/CWE_v4.4.sarif") }
            });

            externalPropertyFileReferences.Taxonomies.Add(new ExternalPropertyFileReference()
            {
                Guid = Constants.Guid.NistSP80063B,
                Location = new ArtifactLocation() { Uri = new Uri("https://raw.githubusercontent.com/sarif-standard/taxonomies/5a8490df0cee6a0e7a8d4a210f8cfffbe3a5d319/NIST_SP800-63B_v1.sarif") }
            });

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies,
                ExternalPropertyFileReferences = externalPropertyFileReferences
            };

            return run;
        }

        private List<ReportingDescriptorRelationship> GetRelationships(string relationshipString, string relationshipStringNistSP80063B, string name)
        {
            List<ReportingDescriptorRelationship> relationships = new List<ReportingDescriptorRelationship>();

            if (!string.IsNullOrWhiteSpace(relationshipString))
            {
                var idList = relationshipString.Split(',', '.', ';').Select(p => p.Trim()).Where(p => p.ToLower() != "none").Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => "CWE-" + p).ToList();

                foreach (var id in idList)
                {
                    relationships.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = id,
                            ToolComponent = new ToolComponentReference { Name = "CWE", Guid = Constants.Guid.Cwe },
                        },
                        Kinds = new string[] { "relevant" },
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(relationshipStringNistSP80063B))
            {
                var idList = relationshipStringNistSP80063B.Split('/').Select(p => p.Trim()).Where(p => p.ToLower() != "none").Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p).ToList();

                foreach (var id in idList)
                {
                    relationships.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = id,
                            ToolComponent = new ToolComponentReference { Name = "Nist SP800-63B", Guid = Constants.Guid.NistSP80063B },
                        },
                        Kinds = new string[] { "relevant" },
                    });
                }
            }

            return relationships.Count > 0 ? relationships : null;
        }
    }
}
