// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Sarif;

using Taxonomy.Common;

namespace Tools.Wasc
{
    public class WascV2Generator : WascGenerator<WascV2Item>
    {
        protected override string TaxonomyGuid => Constants.WASCV2.Guid;

        protected override string TaxonomyName => Constants.WASCV2.Name;

        protected override Uri InformationUri => new Uri("http://projects.webappsec.org/Threat%20Classification");

        protected override bool VerifySource(string sourceFilePath, out Uri sourceUri)
        {
            return Uri.TryCreate(sourceFilePath, UriKind.Absolute, out sourceUri)
                   && (sourceUri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                   || sourceUri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase));
        }

        protected override Task<IList<WascV2Item>> GetWascItemsAsync(Uri sourceUri)
        {
            return new WascDataCralwer().CrawlWascItemsAsync(sourceUri);
        }

        protected override IList<ReportingDescriptor> GetTaxa(List<WascV2Item> items)
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
                        ToolComponent = new ToolComponentReference { Name = Constants.CWE.Name, Guid = Constants.CWE.Guid },
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
                new ToolComponentReference() { Guid = Constants.CWE.Guid, Name = Constants.CWE.Name }
            };

            return new ExternalPropertyFileReferences
            {
                Taxonomies = new List<ExternalPropertyFileReference>
                {
                    new ExternalPropertyFileReference
                    {
                        Guid = Constants.CWE.Guid,
                        Location = new ArtifactLocation() { Uri = new Uri(Constants.CWE.Location) }
                    },
                }
            };
        }
    }
}
