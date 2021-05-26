// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Common;

namespace Tools.Wasc
{
    public class WascTaxonmoyGenerator : TaxonomyGenerator
    {
        public async Task<bool> SaveToSarifAsync(string sourceFilePath, string targetFilePath, string version, string releaseDateUtc)
        {
            if (!Uri.TryCreate(sourceFilePath, UriKind.Absolute, out Uri sourceUri)
                || (!sourceUri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                    && !sourceUri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("SourceFilePath is not a valid URL");
                return false;
            }

            try
            {
                IList<WascViewItem> items = await new WascDataCralwer().CrawlWascItemsAsync(sourceUri);

                Run run = this.ConvertToSarif(items.ToList(), version, releaseDateUtc);

                var log = new SarifLog
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

        private Run ConvertToSarif(List<WascViewItem> items, string version, string releaseDateUtc)
        {
            var supportedTaxonomies = new List<ToolComponentReference>
            {
                new ToolComponentReference() { Guid = Constants.Guid.Cwe, Name = "CWE" }
            };

            IList<ToolComponent> taxonomies = new List<ToolComponent>();
            var toolComponent = new ToolComponent
            {
                Name = "WASC",
                Guid = Constants.Guid.WASC,
                Version = version,
                ReleaseDateUtc = releaseDateUtc,
                InformationUri = new Uri("http://projects.webappsec.org/Threat%20Classification"),
                Organization = "Web Application Security Consortium",
                ShortDescription = new MultiformatMessageString { Text = "The WASC Threat Classification" },
                Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                IsComprehensive = true,
                Taxa = new List<ReportingDescriptor>(),
                SupportedTaxonomies = supportedTaxonomies,
            };

            items.ForEach(r => toolComponent.Taxa.Add(new ReportingDescriptor
            {
                Id = r.WascId,
                FullDescription = string.IsNullOrEmpty(r.Description) ? null : new MultiformatMessageString { Text = r.Description },
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning }, // default
                Relationships = this.GetRelationships(r.CweIds),
            }));

            taxonomies.Add(toolComponent);

            var tool = new Tool { Driver = new ToolComponent { Name = $"WASC {version}" } };

            // need to add other relationship once the referenced taxonomies are created.
            var externalPropertyFileReferences = new ExternalPropertyFileReferences
            {
                Taxonomies = new List<ExternalPropertyFileReference>
                {
                    new ExternalPropertyFileReference
                    {
                        Guid = Constants.Guid.Cwe,
                        Location = new ArtifactLocation() { Uri = new Uri("https://raw.githubusercontent.com/sarif-standard/taxonomies/main/CWE_v4.4.sarif") }
                    },
                }
            };

            var run = new Run
            {
                Tool = tool,
                Taxonomies = taxonomies,
                ExternalPropertyFileReferences = externalPropertyFileReferences,
            };

            return run;
        }

        private List<ReportingDescriptorRelationship> GetRelationships(IEnumerable<string> cweRelationships)
        {
            var relationships = new List<ReportingDescriptorRelationship>();

            foreach (string id in cweRelationships)
            {
                relationships.Add(new ReportingDescriptorRelationship
                {
                    Target = new ReportingDescriptorReference
                    {
                        Id = $"CWE-{id}",
                        ToolComponent = new ToolComponentReference { Name = "CWE", Guid = Constants.Guid.Cwe },
                    },
                    Kinds = new string[] { "relevant" },
                });
            }

            return relationships.Count > 0 ? relationships : null;
        }
    }
}
