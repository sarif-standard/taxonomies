// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using HtmlAgilityPack;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Common;

namespace Taxonomy
{
    public class GdprTaxonomyGenerator : TaxonomyGenerator
    {
        public bool SaveToSarif(string sourceFilePath, string targetFilePath, string version, string releaseDateUtc)
        {
            try
            {
                List<GdprRecord> results = this.ReadFromHtml(sourceFilePath);

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

        private List<GdprRecord> ReadFromHtml(string sourceFilePath)
        {
            List<GdprRecord> records = new List<GdprRecord>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(sourceFilePath);
            List<HtmlNode> idNodes = document.DocumentNode.SelectNodes("//p[@class='title-article-norm']").ToList();
            List<HtmlNode> nameNodes = document.DocumentNode.SelectNodes("//p[@class='stitle-article-norm']").ToList();

            if (idNodes.Count == 0 || nameNodes.Count == 0 || idNodes.Count != nameNodes.Count)
            {
                throw new Exception("error parsing html");
            }

            for (int i = 0; i < idNodes.Count; i++)
            {
                GdprRecord record = new GdprRecord();
                record.Id = "GDPR-" + idNodes[i].InnerText.Replace("Article ", "");
                record.Name = nameNodes[i].InnerText;
                records.Add(record);
            }

            return records;
        }

        private Run ConvertToSarif(List<GdprRecord> records, string version, string releaseDateUtc)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            ToolComponent toolComponent = new ToolComponent
            {
                Name = "GDPR",
                Guid = Constants.Guid.Gdpr,
                Version = version,
                ReleaseDateUtc = releaseDateUtc,
                InformationUri = new Uri("https://gdpr-info.eu/"),
                DownloadUri = new Uri("https://eur-lex.europa.eu/legal-content/EN/TXT/HTML/?uri=CELEX:02016R0679-20160504&from=EN"),
                Organization = "European Parliament",
                ShortDescription = new MultiformatMessageString { Text = "REGULATION (EU) 2016/679 OF THE EUROPEAN PARLIAMENT AND OF THE COUNCIL" },
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = new List<ToolComponentReference>(),
            };

            records.ForEach(c => toolComponent.Taxa.Add(new ReportingDescriptor
            {
                Id = c.Id.ToUpper(),
                Name = c.Name,
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning },
            }));

            taxonomies.Add(toolComponent);

            var tool = new Tool { Driver = new ToolComponent { Name = $"GDPR" } };

            Run run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies
            };

            return run;
        }
    }
}
