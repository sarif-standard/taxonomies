// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

namespace Tools.Common
{
    public abstract class GeneratorBase<T> where T : TaxaItemBase
    {
        protected abstract ToolComponent ToolComponent { get; }

        protected abstract bool VerifySource(string sourceFilePath, out Uri sourceUri);

        protected abstract Task<IList<T>> GetSourceItemsAsync(Uri sourceUri);

        /// <summary>
        /// Convert source data into taxa of current taxonomy.
        /// Sub class need to create relationships to other taxonomies in the overriden method.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected abstract IList<ReportingDescriptor> ConvertSourceItemToTaxa(List<T> items);

        protected abstract ExternalPropertyFileReferences GetTaxonomyReference(ToolComponent tool);

        public virtual async Task<bool> SaveToSarifAsync(string sourceFilePath, string targetFilePath, string version)
        {
            // verify taxonomy data input source
            if (!this.VerifySource(sourceFilePath, out Uri sourceUri))
            {
                Console.WriteLine("SourceFilePath is not valid.");
                return false;
            }

            try
            {
                // read taxonomy data from source
                IList<T> items = await this.GetSourceItemsAsync(sourceUri);

                // create sarif log from taxonomy data
                var log = new SarifLog
                {
                    Runs = new Run[] { this.ConvertToSarif(items.ToList(), version) }
                };

                // save to sarif file
                SerializeSarifLog(targetFilePath, log);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        protected virtual void SerializeSarifLog(string targetFilePath, SarifLog log)
        {
            try
            {
                File.WriteAllText(targetFilePath, JsonConvert.SerializeObject(log, Formatting.Indented));
            }
            catch
            {
                Console.WriteLine("Failed to save taxonomy sarif file.");
                throw;
            }
        }

        protected virtual Run ConvertToSarif(List<T> items, string version)
        {
            if (this.ToolComponent == null)
            {
                throw new ArgumentException("ToolComponment need to be populated in sub class");
            }

            this.ToolComponent.Version = version;
            this.ToolComponent.Taxa = this.ConvertSourceItemToTaxa(items);

            IList<ToolComponent> taxonomies = new List<ToolComponent>
            {
                this.ToolComponent,
            };

            var run = new Run
            {
                Tool = new Tool { Driver = new ToolComponent { Name = this.ToolComponent.Name } },
                Taxonomies = taxonomies,
                ExternalPropertyFileReferences = this.GetTaxonomyReference(taxonomies.First()),
            };

            return run;
        }
    }
}
