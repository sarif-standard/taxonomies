// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Common;

using Tools.NistSP80053;

namespace Taxonomy
{
    public class NistSP80053JsonTaxonomyGenerator : TaxonomyGenerator
    {
        public bool SaveToSarif(string sourceFilePath, string targetFilePath, string version)
        {
            try
            {
                NistSP80053JsonRecord results = this.ReadFromJson(sourceFilePath);

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

        private NistSP80053JsonRecord ReadFromJson(string jsonFilePath)
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            NistSP80053JsonRecord json = JsonConvert.DeserializeObject<NistSP80053JsonRecord>(jsonString);
            return json;
        }

        private Run ConvertToSarif(NistSP80053JsonRecord records, string version)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent toolComponent = new ToolComponent
            {
                Name = Constants.NistSP80053V4.Name,
                Guid = Constants.NistSP80053V4.Guid,
                Version = version,
                ReleaseDateUtc = Constants.NistSP80053V4.ReleaseDate,
                InformationUri = new Uri("https://csrc.nist.gov/publications/detail/sp/800-53/rev-4/final"),
                DownloadUri = new Uri("https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-53r4.pdf"),
                Organization = "National Institute of Standards and Technology",
                ShortDescription = new MultiformatMessageString { Text = "Security and Privacy Controlsfor Information Systems and Organizations" },
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = new List<ToolComponentReference>(),
            };

            List<Control> controls = records.catalog.groups.SelectMany(g => g.controls).ToList();

            controls.ForEach(c => toolComponent.Taxa.Add(new ReportingDescriptor
            {
                Id = c.id.ToUpper(),
                Name = c.title,
                FullDescription = GetFullDescription(c),
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
                Relationships = GetRelationships(c),
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

        private MultiformatMessageString GetFullDescription(Control c)
        {
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (c.@params != null && c.@params.Length > 0)
            {
                foreach (Param param in c.@params)
                {
                    paramDict.TryAdd("{{ insert: param, " + param.id + " }}", param.label);
                }
            }

            StringBuilder sb = new StringBuilder();

            if (c.parts != null && c.parts.Length > 0)
            {
                foreach (Part part in c.parts)
                {
                    if (!string.IsNullOrWhiteSpace(part.prose))
                    {
                        sb.AppendLine(part.prose.Trim());
                    }
                    if (part.parts != null && part.parts.Length > 0)
                    {
                        foreach (Part1 part1 in part.parts)
                        {
                            if (!string.IsNullOrWhiteSpace(part1.prose))
                            {
                                sb.AppendLine(part1.prose.Trim());
                            }
                        }
                    }
                }
            }

            string result = paramDict.Aggregate(sb.ToString(), (current, value) => current.Replace(value.Key, value.Value));
            return string.IsNullOrWhiteSpace(result) ? null : new MultiformatMessageString { Text = result };
        }

        private List<ReportingDescriptorRelationship> GetRelationships(Control c)
        {
            if (c.parts == null || c.parts.Length == 0)
            {
                return null;
            }

            var links = c.parts.Where(p => p.links != null).SelectMany(p => p.links).ToList();

            List<ReportingDescriptorRelationship> relationships = new List<ReportingDescriptorRelationship>();
            foreach (Link2 link in links)
            {
                if (!relationships.Any(r => r.Target.Id == link.href.Replace("#", "").ToUpper()))
                {
                    relationships.Add(new ReportingDescriptorRelationship
                    {
                        Target = new ReportingDescriptorReference
                        {
                            Id = link.href.Replace("#", "").ToUpper(),
                        },
                        Kinds = new string[] { "relevant" },
                    });
                }
            }

            return relationships;
        }
    }
}
