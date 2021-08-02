// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Common;

namespace Taxonomy.Disa
{
    public class DisaTaxonomyGenerator : TaxonomyGenerator
    {
        public bool SaveXmlToSarif(string sourceFilePath, string targetFilePath, string version, string type)
        {
            try
            {
                CCIListType results = this.ReadFromXml(sourceFilePath);

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

        private CCIListType ReadFromXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CCIListType));
            return (CCIListType)serializer.Deserialize(new XmlTextReader(filePath));
        }

        private Run ConvertToSarif(CCIListType xml, string version, string type)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent taxonomy = new ToolComponent
            {
                Version = version,
                Organization = "DISA Field Security Operations",
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                MinimumRequiredLocalizedDataSemanticVersion = version,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = new List<ToolComponentReference>(),
            };

            switch (type.ToLower())
            {
                case "cci":
                    switch (version.ToLower())
                    {
                        case "2":
                            taxonomy.Name = Constants.DISA_CCI_V2.Name;
                            taxonomy.Guid = Constants.DISA_CCI_V2.Guid;
                            taxonomy.ReleaseDateUtc = Constants.DISA_CCI_V2.ReleaseDate;
                            taxonomy.InformationUri =
                                new Uri("https://dl.dod.cyber.mil/wp-content/uploads/stigs/zip/u_draft_cci_specification_v2r0.2.zip");
                            taxonomy.DownloadUri =
                                new Uri("https://dl.dod.cyber.mil/wp-content/uploads/stigs/zip/u_cci_list.zip");
                            taxonomy.ShortDescription =
                                new MultiformatMessageString { Text = "DISA Control Correlation Identifier (CCI) Version 2" };
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            var taxa = new List<ReportingDescriptor>();

            foreach (ItemType item in xml.cci_items)
            {
                taxa.Add(new ReportingDescriptor
                {
                    Id = $"{item.id}",
                    ShortDescription = new MultiformatMessageString { Text = item.definition },
                    DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
                    Relationships = this.GetRelationships(item.references),
                });
            }

            taxonomy.Taxa = taxa.OrderBy(o => int.Parse(o.Id.Replace("CCI-", ""))).ToList();
            taxonomies.Add(taxonomy);

            var tool = new Tool { Driver = new ToolComponent { Name = taxonomy.Name } };

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies
            };

            return run;
        }

        private List<ReportingDescriptorRelationship> GetRelationships(ReferenceType[] related)
        {
            if (related == null || related.Length == 0)
            {
                return null;
            }

            related = related.Where(o => o.title == "NIST SP 800-53 Revision 4").OrderBy(o => o.index).ToArray();

            List<ReportingDescriptorRelationship> rels = new List<ReportingDescriptorRelationship>();
            foreach (ReferenceType entry in related)
            {
                string targetId = entry.index.Split(' ').FirstOrDefault();
                if (!rels.Any(r => r.Target.Id == targetId))
                {
                    rels.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = targetId,
                            ToolComponent = new ToolComponentReference { Name = Constants.Nist_SP80053_V4.Name, Guid = Constants.Nist_SP80053_V4.Guid },
                        },
                        Kinds = new string[] { "relevant" },
                    });
                }
            }

            return rels;
        }
    }
}
