// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

namespace Tools.Wasc
{
    public abstract class WascGenerator<T> where T : WascBaseItem
    {
        protected abstract string TaxonomyGuid { get; }

        protected abstract string TaxonomyName { get; }

        protected abstract Uri InformationUri { get; }

        protected abstract bool VerifySource(string sourceFilePath, out Uri sourceUri);

        protected abstract Task<IList<T>> GetWascItemsAsync(Uri sourceUri);

        protected abstract IList<ReportingDescriptor> GetTaxa(List<T> items);

        protected abstract ExternalPropertyFileReferences GetTaxonomyReference(ToolComponent tool);

        public async Task<bool> SaveToSarifAsync(string sourceFilePath, string targetFilePath, string version, string releaseDateUtc)
        {
            if (!this.VerifySource(sourceFilePath, out Uri sourceUri))
            {
                Console.WriteLine("SourceFilePath is not valid.");
                return false;
            }

            try
            {
                IList<T> items = await this.GetWascItemsAsync(sourceUri);

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

        private Run ConvertToSarif(List<T> items, string version, string releaseDateUtc)
        {
            IList<ToolComponent> taxonomies = new List<ToolComponent>
            {
                new ToolComponent
                {
                    Name = this.TaxonomyName,
                    Guid = this.TaxonomyGuid,
                    Version = version,
                    ReleaseDateUtc = releaseDateUtc,
                    InformationUri = this.InformationUri,
                    Organization = "Web Application Security Consortium",
                    ShortDescription = new MultiformatMessageString { Text = "The WASC Threat Classification" },
                    Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                    IsComprehensive = true,
                    Taxa = this.GetTaxa(items),
                }
            };

            var run = new Run
            {
                Tool = new Tool { Driver = new ToolComponent { Name = this.TaxonomyName } },
                Taxonomies = taxonomies,
                ExternalPropertyFileReferences = this.GetTaxonomyReference(taxonomies.First()),
            };

            return run;
        }
    }
}
