// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CsvHelper;

using Microsoft.CodeAnalysis.Sarif;

using Taxonomy.Common;

namespace Tools.Wasc
{
    public class WascV1Generator : WascGenerator<WascV1Item>
    {
        protected override string TaxonomyGuid => Constants.WASCV1.Guid;

        protected override string TaxonomyName => Constants.WASCV1.Name;

        protected override Uri InformationUri => new Uri("http://projects.webappsec.org/Threat%20Classification%20Previous%20Versions");

        protected override bool VerifySource(string sourceFilePath, out Uri sourceUri)
        {
            return Uri.TryCreate(sourceFilePath, UriKind.Absolute, out sourceUri)
                   && File.Exists(sourceFilePath);
        }

        protected override Task<IList<WascV1Item>> GetWascItemsAsync(Uri sourceUri)
        {
            return this.ReadFromCsvAsync(sourceUri.LocalPath);
        }

        private async Task<IList<WascV1Item>> ReadFromCsvAsync(string filePath)
        {
            using FileStream input = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var textReader = new StreamReader(input);
            using var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);

            return await csvReader.GetRecordsAsync<WascV1Item>().ToListAsync();
        }

        protected override IList<ReportingDescriptor> GetTaxa(List<WascV1Item> items)
        {
            return items.Select(r => new ReportingDescriptor
            {
                Id = r.WascId,
                Name = r.WascName,
                FullDescription = string.IsNullOrEmpty(r.Description) ? null : new MultiformatMessageString { Text = r.Description },
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning }, // default
            }).ToList();
        }

        protected override ExternalPropertyFileReferences GetTaxonomyReference(ToolComponent tool)
        {
            // no reference taxonomy defined
            return null;
        }
    }
}
