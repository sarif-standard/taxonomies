// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Markdig.Syntax;
using Microsoft.CodeAnalysis.Sarif;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Taxonomy.Common;

namespace Taxonomy
{
    public class NistSP80063BTaxonomyGenerator : TaxonomyGenerator
    {
        public List<NistSP80063BRecord> ReadFromMd(string folderPath)
        {
            DirectoryInfo d = new DirectoryInfo(folderPath);
            FileInfo[] files = d.GetFiles("sec*.md");
            files = files.OrderBy(f => Int32.Parse(Regex.Match(f.Name, @"\d+").Value)).ToArray();
            List<string> allHeaders = new List<string>();

            foreach (var file in files)
            {
                string text = File.ReadAllText(file.FullName);
                MarkdownDocument doc = Markdig.Markdown.Parse(text);
                var headerBlocks = doc.Where(d => d.GetType() == typeof(HeadingBlock)).ToList();
                var headerTexts = headerBlocks.Select(h =>
                char.IsDigit(((LeafBlock)h).Inline.LastChild.ToString().Trim()[0]) ?
                ((LeafBlock)h).Inline.LastChild.ToString().Trim() :
                ((LeafBlock)h).Inline.FirstChild.ToString().Trim() + " " + ((LeafBlock)h).Inline.LastChild.ToString().Trim()).ToList();
                allHeaders.AddRange(headerTexts);
            }

            files = d.GetFiles("app*.md");
            foreach (var file in files)
            {
                string text = File.ReadAllText(file.FullName);
                MarkdownDocument doc = Markdig.Markdown.Parse(text);
                var headerBlocks = doc.Where(d => d.GetType() == typeof(HeadingBlock)).ToList();
                var headerTexts = headerBlocks.Where(h =>
                ((LeafBlock)h).Inline.LastChild.ToString().Trim().StartsWith("A.")).Select(h => ((LeafBlock)h).Inline.LastChild.ToString().Trim()).ToList();
                allHeaders.AddRange(headerTexts);
            }

            List<NistSP80063BRecord> records = new List<NistSP80063BRecord>();
            foreach (var header in allHeaders)
            {
                var splitted = header.Split(new[] { ' ' }, 2);
                records.Add(new NistSP80063BRecord() { Id = splitted[0], Name = splitted[1] });
            }

            return records;
        }

        public void SaveToSarif(string sourceFolderPath, string targetFilePath)
        {
            var results = this.ReadFromMd(sourceFolderPath);

            var run = this.ConvertToSarif(results);

            SarifLog log = new SarifLog
            {
                Runs = new Run[] { run }
            };

            File.WriteAllText(targetFilePath, JsonConvert.SerializeObject(log, Formatting.Indented));
        }

        public Run ConvertToSarif(List<NistSP80063BRecord> records)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent toolComponent = new ToolComponent
            {
                Name = "NIST SP800-63B",
                Guid = Constants.Guid.NistSP80063B,
                Version = "1",
                ReleaseDateUtc = "2020-03-02",
                InformationUri = new Uri("https://pages.nist.gov/800-63-3/sp800-63b.html"),
                DownloadUri = new Uri("https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-63b.pdf"),
                Organization = "National Institute of Standards and Technology",
                ShortDescription = new MultiformatMessageString { Text = "Digital Identity Guidelines" },
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = new List<ToolComponentReference>(),
            };

            records.ForEach(r => toolComponent.Taxa.Add(new ReportingDescriptor
            {
                Id = r.Id,
                Name = r.Name,
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
            }));

            taxonomies.Add(toolComponent);

            var tool = new Tool { Driver = new ToolComponent { Name = "NIST SP800-63B v1" } };

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies
            };

            return run;
        }
    }
}
