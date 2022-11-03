// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Sarif;

using Taxonomy.Common;

using Tools.Common;

namespace Tools.Wasc
{
    public class WascV2Generator : GeneratorBase<WascV2Item>
    {
        private ToolComponent tool;

        protected override ToolComponent ToolComponent
        {
            get
            {
                if (this.tool == null)
                {
                    this.tool = new ToolComponent
                    {
                        Name = Constants.WASC_V2.Name,
                        Guid = Constants.WASC_V2.Guid,
                        ReleaseDateUtc = Constants.WASC_V2.ReleaseDate,
                        InformationUri = new Uri("http://projects.webappsec.org/Threat%20Classification"),
                        Organization = "Web Application Security Consortium",
                        ShortDescription = new MultiformatMessageString { Text = "The WASC Threat Classification" },
                        Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                        IsComprehensive = true,
                    };
                }
                return this.tool;
            }
        }

        protected override bool VerifySource(string sourceFilePath, out Uri sourceUri)
        {
            return Uri.TryCreate(sourceFilePath, UriKind.Absolute, out sourceUri)
                   && (sourceUri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                   || sourceUri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase));
        }

        protected override Task<IList<WascV2Item>> GetSourceItemsAsync(Uri sourceUri)
        {
            return new WascDataCralwer().CrawlWascItemsAsync(sourceUri);
        }

        protected override IList<ReportingDescriptor> ConvertSourceItemToTaxa(List<WascV2Item> items)
        {
            return items.Select(r => new ReportingDescriptor
            {
                Id = r.WascId,
                Name = r.WascName,
                FullDescription = string.IsNullOrEmpty(r.Description) ? null : new MultiformatMessageString { Text = r.Description },
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning }, // default
                Relationships = this.GetRelationships(r.CweIds),
            }).ToList();
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
                        ToolComponent = new ToolComponentReference { Name = Constants.CWE_Comprehensive["4.4"].Name, Guid = Constants.CWE_Comprehensive["4.4"].Guid },
                    },
                    Kinds = new string[] { "relevant" },
                });
            }

            return relationships.Count > 0 ? relationships : null;
        }

        protected override ExternalPropertyFileReferences GetTaxonomyReference(ToolComponent tool)
        {
            // need to add other relationship once the referenced taxonomies are created.

            tool.SupportedTaxonomies = new List<ToolComponentReference>
            {
                new ToolComponentReference() { Guid = Constants.CWE_Comprehensive["4.4"].Guid, Name = Constants.CWE_Comprehensive["4.4"].Name }
            };

            return new ExternalPropertyFileReferences
            {
                Taxonomies = new List<ExternalPropertyFileReference>
                {
                    new ExternalPropertyFileReference
                    {
                        Guid = Constants.CWE_Comprehensive["4.4"].Guid,
                        Location = new ArtifactLocation() { Uri = new Uri(Constants.CWE_Comprehensive["4.4"].Location) }
                    },
                }
            };
        }
    }
}
