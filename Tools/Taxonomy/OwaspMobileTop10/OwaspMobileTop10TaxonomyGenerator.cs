// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Markdig.Syntax;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Common;

namespace Taxonomy
{
    public class OwaspMobileTop10TaxonomyGenerator : TaxonomyGenerator
    {
        public bool SaveToSarif(string sourceFolderPath, string targetFilePath, string version)
        {
            try
            {
                List<OwaspMobileTop10Record> results = this.ReadFromMd(sourceFolderPath);

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

        private List<OwaspMobileTop10Record> ReadFromMd(string folderPath)
        {
            List<OwaspMobileTop10Record> records = new List<OwaspMobileTop10Record>();
            DirectoryInfo d = new DirectoryInfo(folderPath);
            FileInfo[] files = d.GetFiles("m*-*.md");
            files = files.OrderBy(f => int.Parse(Regex.Match(f.Name, @"\d+").Value)).ToArray();

            foreach (FileInfo file in files)
            {
                List<string> allHeaders = new List<string>();
                string text = File.ReadAllText(file.FullName);
                MarkdownDocument doc = Markdig.Markdown.Parse(text);
                var headerBlocks = doc.Where(d => d.GetType() == typeof(HeadingBlock)).ToList();
                string titleText = headerBlocks.Select(h => ((LeafBlock)h).Inline.FirstChild.ToString().Trim()).First();
                Match match = Regex.Match(titleText, "title: \"(?<id>.*): (?<name>.*)\"");
                var headerTexts = headerBlocks.Select(h => ((LeafBlock)h).Inline.FirstChild.ToString().Trim()).Skip(1).ToList();
                allHeaders.AddRange(headerTexts);
                records.Add(new OwaspMobileTop10Record()
                {
                    Id = match.Groups["id"].Value,
                    Name = match.Groups["name"].Value,
                    ShortDescription = string.Join("\r\n", allHeaders)
                });
            }

            return records;
        }

        private Run ConvertToSarif(List<OwaspMobileTop10Record> records, string version)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent toolComponent = new ToolComponent
            {
                Version = version,
                Name = version switch
                {
                    "2014" => Constants.OwaspMobileTop102014.Name,
                    "2016" => Constants.OwaspMobileTop102016.Name,
                    _ => throw new NotImplementedException()
                },
                Guid = version switch
                {
                    "2014" => Constants.OwaspMobileTop102014.Guid,
                    "2016" => Constants.OwaspMobileTop102016.Guid,
                    _ => throw new NotImplementedException()
                },
                InformationUri = new Uri(version switch
                {
                    "2014" => Constants.OwaspMobileTop102014.Location,
                    "2016" => Constants.OwaspMobileTop102016.Location,
                    _ => throw new NotImplementedException()
                }),
                DownloadUri = new Uri(version switch
                {
                    "2014" => Constants.OwaspMobileTop102014.Location,
                    "2016" => Constants.OwaspMobileTop102016.Location,
                    _ => throw new NotImplementedException()
                }),
                Organization = "OWASP Foundation",
                ShortDescription = new MultiformatMessageString { Text = $"OWASP Mobile Top 10 {version}" },
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

            var tool = new Tool { Driver = new ToolComponent { Name = $"OWASP Mobile Top 10 {version}" } };

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies
            };

            return run;
        }
    }
}
