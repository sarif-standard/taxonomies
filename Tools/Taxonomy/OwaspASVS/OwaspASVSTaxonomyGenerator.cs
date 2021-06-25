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
    public class OwaspASVSTaxonomyGenerator : TaxonomyGenerator
    {
        public bool SaveToSarif(string sourceFilePath, string targetFilePath, string version)
        {
            try
            {
                List<OwaspASVSCsvRecord> results = this.ReadFromCsv(sourceFilePath);

                Run run = this.ConvertToSarif(results, version);

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

        private List<OwaspASVSCsvRecord> ReadFromCsv(string csvFilePath)
        {
            using FileStream input = File.Open(csvFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var textReader = new StreamReader(input);
            using var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);

            return csvReader.GetRecords<OwaspASVSCsvRecord>().ToList();
        }

        private Run ConvertToSarif(List<OwaspASVSCsvRecord> records, string version)
        {
            var supportedTaxonomies = new List<ToolComponentReference>();
            supportedTaxonomies.Add(new ToolComponentReference() { Guid = Constants.CWE_Comprehensive_V44.Guid, Name = Constants.CWE_Comprehensive_V44.Name });
            supportedTaxonomies.Add(new ToolComponentReference() { Guid = Constants.Nist_SP80063B.Guid, Name = Constants.Nist_SP80063B.Name });

            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent toolComponent = new ToolComponent
            {
                Name = Constants.Owasp_ASVS_V402.Name,
                Guid = Constants.Owasp_ASVS_V402.Guid,
                Version = version,
                ReleaseDateUtc = Constants.Owasp_ASVS_V402.ReleaseDate,
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
                Relationships = this.GetRelationships(r.RelatedCwe, r.RelatedNistSP80063B),
            }));

            taxonomies.Add(toolComponent);

            var tool = new Tool { Driver = new ToolComponent { Name = $"OWASP ASVS v{version}" } };

            ExternalPropertyFileReferences externalPropertyFileReferences = new ExternalPropertyFileReferences();
            externalPropertyFileReferences.Taxonomies = new List<ExternalPropertyFileReference>();
            externalPropertyFileReferences.Taxonomies.Add(new ExternalPropertyFileReference()
            {
                Guid = Constants.CWE_Comprehensive_V44.Guid,
                Location = new ArtifactLocation() { Uri = new Uri(Constants.CWE_Comprehensive_V44.Location) }
            });

            externalPropertyFileReferences.Taxonomies.Add(new ExternalPropertyFileReference()
            {
                Guid = Constants.Nist_SP80063B.Guid,
                Location = new ArtifactLocation() { Uri = new Uri(Constants.Nist_SP80063B.Location) }
            });

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies,
                ExternalPropertyFileReferences = externalPropertyFileReferences
            };

            return run;
        }

        private List<ReportingDescriptorRelationship> GetRelationships(string relationshipString, string relationshipStringNistSP80063B)
        {
            List<ReportingDescriptorRelationship> relationships = new List<ReportingDescriptorRelationship>();

            if (!string.IsNullOrWhiteSpace(relationshipString))
            {
                var idList = relationshipString.Split(new char[] { ',', '.', ';' }, StringSplitOptions.TrimEntries).Where(p => p.ToLower() != "none").Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => "CWE-" + p).ToList();

                foreach (string id in idList)
                {
                    relationships.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = id,
                            ToolComponent = new ToolComponentReference { Name = Constants.CWE_Comprehensive_V44.Name, Guid = Constants.CWE_Comprehensive_V44.Guid },
                        },
                        Kinds = new string[] { "relevant" },
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(relationshipStringNistSP80063B))
            {
                var idList = relationshipStringNistSP80063B.Split('/', StringSplitOptions.TrimEntries).Where(p => p.ToLower() != "none").Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

                foreach (string id in idList)
                {
                    relationships.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = id,
                            ToolComponent = new ToolComponentReference { Name = Constants.Nist_SP80063B.Name, Guid = Constants.Nist_SP80063B.Guid },
                        },
                        Kinds = new string[] { "relevant" },
                    });
                }
            }

            return relationships.Count > 0 ? relationships : null;
        }
    }
}
