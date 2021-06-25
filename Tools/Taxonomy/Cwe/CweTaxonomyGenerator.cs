﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using CsvHelper;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Common;

namespace Taxonomy.Cwe
{
    public class CweTaxonomyGenerator : TaxonomyGenerator
    {
        public bool SaveCsvToSarif(string sourceFilePath, string targetFilePath, string version)
        {
            try
            {
                List<CweCsvRecord> records = this.ReadFromCsv(sourceFilePath);

                Run run = this.ConvertToSarif(records, version);

                SarifLog log = new SarifLog
                {
                    Runs = new Run[] { run }
                };

                File.WriteAllText(targetFilePath, JsonConvert.SerializeObject(log, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public bool SaveXmlToSarif(string sourceFilePath, string targetFilePath, string version, string type)
        {
            try
            {
                Weakness_Catalog results = this.ReadFromXml(sourceFilePath);

                Run run = this.ConvertToSarif(results, version, type);

                SarifLog log = new SarifLog
                {
                    Runs = new Run[] { run }
                };

                File.WriteAllText(targetFilePath, JsonConvert.SerializeObject(log, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public bool AddOwaspRelationshipToSarif(string cweSarifPath, string owaspSarifPath, string targetFilePath)
        {
            try
            {
                SarifLog cweSarif = ReadFromSarif(cweSarifPath);
                SarifLog owaspSarif = ReadFromSarif(owaspSarifPath);
                foreach (ReportingDescriptor taxon in owaspSarif.Runs[0].Taxonomies[0].Taxa)
                {
                    if (taxon.Relationships != null)
                    {
                        foreach (ReportingDescriptorRelationship relationship in taxon.Relationships)
                        {
                            if (relationship.Target.ToolComponent.Name == "CWE")
                            {
                                var reportingDescriptorRelationship = new ReportingDescriptorRelationship();
                                reportingDescriptorRelationship.Target = new ReportingDescriptorReference() { Id = taxon.Id };
                                reportingDescriptorRelationship.Target.ToolComponent = new ToolComponentReference() { Guid = Constants.Owasp_ASVS_V402.Guid, Name = Constants.Owasp_ASVS_V402.Name };
                                reportingDescriptorRelationship.Kinds = new List<string>() { "relevant" };
                                IList<ReportingDescriptorRelationship> existingRelationships = cweSarif.Runs[0].Taxonomies[0].Taxa.First(t => t.Id == relationship.Target.Id).Relationships;
                                if (existingRelationships != null && !existingRelationships.Any(r => r.Target.Id == reportingDescriptorRelationship.Target.Id))
                                {
                                    existingRelationships.Add(reportingDescriptorRelationship);
                                }
                            }
                        }
                    }
                }

                cweSarif.Runs[0].Taxonomies[0].SupportedTaxonomies.Add(new ToolComponentReference() { Guid = Constants.Owasp_ASVS_V402.Guid, Name = Constants.Owasp_ASVS_V402.Name });

                ExternalPropertyFileReferences externalPropertyFileReferences = new ExternalPropertyFileReferences();
                externalPropertyFileReferences.Taxonomies = new List<ExternalPropertyFileReference>();
                externalPropertyFileReferences.Taxonomies.Add(new ExternalPropertyFileReference()
                {
                    Guid = Constants.Owasp_ASVS_V402.Guid,
                    Location = new ArtifactLocation() { Uri = new Uri(Constants.Owasp_ASVS_V402.Location) }
                });
                cweSarif.Runs[0].ExternalPropertyFileReferences = externalPropertyFileReferences;

                File.WriteAllText(targetFilePath, JsonConvert.SerializeObject(cweSarif, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        private List<CweCsvRecord> ReadFromCsv(string filePath)
        {
            using FileStream input = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var textReader = new StreamReader(input);
            using var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);

            return csvReader.GetRecords<CweCsvRecord>().ToList();
        }

        private Weakness_Catalog ReadFromXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Weakness_Catalog));
            return (Weakness_Catalog)serializer.Deserialize(new XmlTextReader(filePath));
        }

        private Run ConvertToSarif(List<CweCsvRecord> records, string version)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent cweTaxonomy = new ToolComponent
            {
                Name = Constants.CWE_Comprehensive_V43.Name,
                Guid = Constants.CWE_Comprehensive_V43.Guid,
                Version = version,
                ReleaseDateUtc = Constants.CWE_Comprehensive_V43.ReleaseDate,
                InformationUri = new Uri("https://cwe.mitre.org/data/published/cwe_v4.3.pdf"),
                DownloadUri = new Uri("https://cwe.mitre.org/data/xml/cwec_v4.3.xml.zip"),
                Organization = "MITRE",
                ShortDescription = new MultiformatMessageString { Text = "The MITRE Common Weakness Enumeration" },
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                MinimumRequiredLocalizedDataSemanticVersion = version,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = new List<ToolComponentReference>(),
            };

            records.ForEach(r => cweTaxonomy.Taxa.Add(new ReportingDescriptor
            {
                Id = $"CWE-{r.CweId}",
                Name = r.Name,
                ShortDescription = new MultiformatMessageString { Text = r.Description },
                FullDescription = string.IsNullOrEmpty(r.ExtendedDescription) ? null : new MultiformatMessageString { Text = r.ExtendedDescription },
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
                Relationships = this.GetRelationships(r.RelatedWeaknesses),
            }));

            taxonomies.Add(cweTaxonomy);

            var tool = new Tool { Driver = new ToolComponent { Name = $"CWE v{version}" } };

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies
            };

            return run;
        }

        private Run ConvertToSarif(Weakness_Catalog cweXml, string version, string type)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent cweTaxonomy = new ToolComponent
            {
                Version = version,
                Organization = "MITRE",
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                MinimumRequiredLocalizedDataSemanticVersion = version,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = new List<ToolComponentReference>(),
            };

            switch (type.ToLower())
            {
                case "comprehensive":
                    switch (version.ToLower())
                    {
                        case "4.3":
                            cweTaxonomy.Name = Constants.CWE_Comprehensive_V43.Name;
                            cweTaxonomy.Guid = Constants.CWE_Comprehensive_V43.Guid;
                            cweTaxonomy.ReleaseDateUtc = Constants.CWE_Comprehensive_V43.ReleaseDate;
                            cweTaxonomy.InformationUri = new Uri("https://cwe.mitre.org/data/published/cwe_v4.3.pdf");
                            cweTaxonomy.DownloadUri = new Uri("https://cwe.mitre.org/data/xml/cwec_v4.3.xml.zip");
                            cweTaxonomy.ShortDescription = new MultiformatMessageString { Text = "The MITRE Common Weakness Enumeration" };
                            break;
                        case "4.4":
                            cweTaxonomy.Name = Constants.CWE_Comprehensive_V44.Name;
                            cweTaxonomy.Guid = Constants.CWE_Comprehensive_V44.Guid;
                            cweTaxonomy.ReleaseDateUtc = Constants.CWE_Comprehensive_V44.ReleaseDate;
                            cweTaxonomy.InformationUri = new Uri("https://cwe.mitre.org/data/published/cwe_v4.4.pdf");
                            cweTaxonomy.DownloadUri = new Uri("https://cwe.mitre.org/data/xml/cwec_v4.4.xml.zip");
                            cweTaxonomy.ShortDescription = new MultiformatMessageString { Text = "The MITRE Common Weakness Enumeration" };
                            break;
                        default:
                            break;
                    }
                    break;
                case "top25":
                    switch (version.ToLower())
                    {
                        case "2019":
                            cweTaxonomy.Name = Constants.CWE_Top_25_2019.Name;
                            cweTaxonomy.Guid = Constants.CWE_Top_25_2019.Guid;
                            cweTaxonomy.ReleaseDateUtc = Constants.CWE_Top_25_2019.ReleaseDate;
                            cweTaxonomy.InformationUri = new Uri("https://cwe.mitre.org/data/slices/1200.html");
                            cweTaxonomy.DownloadUri = new Uri("https://cwe.mitre.org/data/xml/views/1200.xml.zip");
                            cweTaxonomy.ShortDescription = new MultiformatMessageString { Text = "The MITRE Common Weakness Enumeration Top 25" };
                            break;
                        case "2020":
                            cweTaxonomy.Name = Constants.CWE_Top_25_2020.Name;
                            cweTaxonomy.Guid = Constants.CWE_Top_25_2020.Guid;
                            cweTaxonomy.ReleaseDateUtc = Constants.CWE_Top_25_2020.ReleaseDate;
                            cweTaxonomy.InformationUri = new Uri("https://cwe.mitre.org/data/slices/1350.html");
                            cweTaxonomy.DownloadUri = new Uri("https://cwe.mitre.org/data/xml/views/1350.xml.zip");
                            cweTaxonomy.ShortDescription = new MultiformatMessageString { Text = "The MITRE Common Weakness Enumeration Top 25" };
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            var taxa = new List<ReportingDescriptor>();

            foreach (WeaknessType r in cweXml.Weaknesses)
            {
                taxa.Add(new ReportingDescriptor
                {
                    Id = $"CWE-{r.ID}",
                    Name = r.Name,
                    ShortDescription = new MultiformatMessageString { Text = r.Description },
                    FullDescription = string.IsNullOrEmpty(r.Extended_Description?.ToDescription()) ? null : new MultiformatMessageString { Text = r.Extended_Description?.ToDescription() },
                    DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
                    Relationships = this.GetRelationships(r.Related_Weaknesses),
                });
            }

            foreach (CategoryType r in cweXml.Categories)
            {
                taxa.Add(new ReportingDescriptor
                {
                    Id = $"CWE-{r.ID}",
                    Name = r.Name,
                    ShortDescription = null,
                    FullDescription = string.IsNullOrEmpty(r.Summary?.ToDescription()) ? null : new MultiformatMessageString { Text = r.Summary?.ToDescription() },
                    DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
                    Relationships = this.GetRelationships(r.Relationships?.Items, r.ID),
                });
            }

            foreach (ViewType r in cweXml.Views)
            {
                taxa.Add(new ReportingDescriptor
                {
                    Id = $"CWE-{r.ID}",
                    Name = r.Name,
                    ShortDescription = null,
                    FullDescription = string.IsNullOrEmpty(r.Objective?.ToDescription()) ? null : new MultiformatMessageString { Text = r.Objective?.ToDescription() },
                    DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
                    Relationships = this.GetRelationships(r.Members?.Items, r.ID),
                });
            }

            cweTaxonomy.Taxa = taxa.OrderBy(o => int.Parse(o.Id.Replace("CWE-", ""))).ToList();

            taxonomies.Add(cweTaxonomy);

            var tool = new Tool { Driver = new ToolComponent { Name = cweTaxonomy.Name } };

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

            // example relationship
            // ::NATURE:ChildOf:CWE ID:707:VIEW ID:1000:ORDINAL:Primary::NATURE:PeerOf:CWE ID:345:VIEW ID:1000:ORDINAL:Primary::NATURE:CanPrecede:CWE ID:22:VIEW ID:1000::NATURE:CanPrecede:CWE ID:41:VIEW ID:1000::NATURE:CanPrecede:CWE ID:74:VIEW ID:1000::NATURE:CanPrecede:CWE ID:119:VIEW ID:1000::NATURE:CanPrecede:CWE ID:770:VIEW ID:1000::
            string[] relationships = relationshipString.Split("::", StringSplitOptions.TrimEntries);
            var map = new Dictionary<string, CweRelationship>();
            foreach (string rel in relationships)
            {
                if (!string.IsNullOrEmpty(rel))
                {
                    var cweRel = new CweRelationship(rel);
                    if (!map.ContainsKey(cweRel.ToString()))
                    {
                        map.Add(cweRel.ToString(), cweRel);
                    }
                }
            }

            List<ReportingDescriptorRelationship> rels = new List<ReportingDescriptorRelationship>();
            foreach (KeyValuePair<string, CweRelationship> entry in map)
            {
                rels.Add(new ReportingDescriptorRelationship
                {
                    Target = new ReportingDescriptorReference
                    {
                        Id = entry.Value.CweId,
                    },
                    Kinds = new string[] { entry.Value.Kinds },
                });
            }

            return rels;
        }

        private List<ReportingDescriptorRelationship> GetRelationships(RelatedWeaknessesTypeRelated_Weakness[] related)
        {
            if (related == null || related.Length == 0)
            {
                return null;
            }

            related = related.OrderBy(o => int.Parse(o.CWE_ID)).ToArray();

            List<ReportingDescriptorRelationship> rels = new List<ReportingDescriptorRelationship>();
            foreach (RelatedWeaknessesTypeRelated_Weakness entry in related)
            {
                if (!rels.Any(r => r.Target.Id == $"CWE-{entry.CWE_ID}" && r.Kinds.Contains(entry.Nature.ToSarifRelationship())))
                {
                    rels.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = $"CWE-{entry.CWE_ID}",
                            ToolComponent = new ToolComponentReference { Name = "CWE" },
                        },
                        Kinds = new string[] { entry.Nature.ToSarifRelationship() },
                    });
                }
            }

            return rels;
        }

        private List<ReportingDescriptorRelationship> GetRelationships(MemberType[] related, string currentId)
        {
            if (related == null || related.Length == 0)
            {
                return null;
            }

            related = related.OrderBy(o => int.Parse(o.CWE_ID)).ToArray();

            List<ReportingDescriptorRelationship> relationships = new List<ReportingDescriptorRelationship>();
            foreach (MemberType entry in related)
            {
                if (!relationships.Any(r => r.Target.Id == $"CWE-{entry.CWE_ID}"))
                {
                    relationships.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = $"CWE-{entry.CWE_ID}",
                        },
                        Kinds = new string[] { "subset" },
                    });
                }
                if (!relationships.Any(r => r.Target.Id == $"CWE-{entry.View_ID}") && currentId != entry.View_ID)
                {
                    relationships.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = $"CWE-{entry.View_ID}",
                        },
                        Kinds = new string[] { "superset" },
                    });
                }
            }

            relationships = relationships.OrderByDescending(o => o.Kinds[0]).ThenBy(o => int.Parse(o.Target.Id.Replace("CWE-", ""))).ToList();

            return relationships;
        }
    }
}
