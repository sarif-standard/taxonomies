// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Common;

namespace Taxonomy
{
    public class OwaspTop10TaxonomyGenerator : TaxonomyGenerator
    {
        public bool SaveToSarif(string sourceUri, string targetFilePath, string version)
        {
            try
            {
                List<OwaspTop10Record> results = this.ReadFromUri(sourceUri);

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

        private List<OwaspTop10Record> ReadFromUri(string sourceUri)
        {
            List<OwaspTop10Record> records = new List<OwaspTop10Record>();
            HttpClient client = new HttpClient();
            string text = client.GetStringAsync(sourceUri).Result;
            var idNameAndDescriptions = Regex.Split(text, "(.*\\[.*\\]\\(.*\\))").ToList();

            if (idNameAndDescriptions.Count > 3 && idNameAndDescriptions.Count % 2 != 0)
            {
                for (int i = 1; i < idNameAndDescriptions.Count; i += 2)
                {
                    Match idAndName;
                    idAndName = Regex.Match(idNameAndDescriptions[i], "\\[(?<id>.*)-(?<name>.*)\\]\\((?<url>.*)\\)");

                    if (idAndName.Captures.Count == 0)
                    {
                        idAndName = Regex.Match(idNameAndDescriptions[i], "\\[(?<id>.*) 2004 (?<name>.*)\\]\\((?<url>.*)\\)");
                    }

                    records.Add(new OwaspTop10Record()
                    {
                        Id = idAndName.Groups["id"].Value.Trim(),
                        Name = idAndName.Groups["name"].Value.Trim(),
                        ShortDescription = idNameAndDescriptions[i + 1].Trim()
                    });
                }
            }

            return records;
        }

        private Run ConvertToSarif(List<OwaspTop10Record> records, string version)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent toolComponent = new ToolComponent
            {
                Version = version,
                Name = version switch
                {
                    "2004" => Constants.Owasp_Top_10_2004.Name,
                    "2007" => Constants.Owasp_Top_10_2007.Name,
                    "2010" => Constants.Owasp_Top_10_2010.Name,
                    "2013" => Constants.Owasp_Top_10_2013.Name,
                    "2017" => Constants.Owasp_Top_10_2017.Name,
                    _ => throw new NotImplementedException()
                },
                Guid = version switch
                {
                    "2004" => Constants.Owasp_Top_10_2004.Guid,
                    "2007" => Constants.Owasp_Top_10_2007.Guid,
                    "2010" => Constants.Owasp_Top_10_2010.Guid,
                    "2013" => Constants.Owasp_Top_10_2013.Guid,
                    "2017" => Constants.Owasp_Top_10_2017.Guid,
                    _ => throw new NotImplementedException()
                },
                InformationUri = new Uri("https://owasp.org/www-project-top-ten/"),
                Organization = "OWASP Foundation",
                ShortDescription = new MultiformatMessageString { Text = $"OWASP Top 10 {version}" },
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
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
            }));

            taxonomies.Add(toolComponent);

            var tool = new Tool { Driver = new ToolComponent { Name = $"OWASP Top 10 {version}" } };

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies
            };

            return run;
        }
    }
}
