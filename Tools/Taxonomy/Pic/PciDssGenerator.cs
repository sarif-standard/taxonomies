﻿// Copyright (c) Microsoft. All rights reserved.
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

using Tools.Common;

namespace Tools.Pci
{
    public class PciDssGenerator : GeneratorBase<PciDssItem>
    {
        #region overrides

        private ToolComponent tool;

        protected override ToolComponent ToolComponent
        {
            get
            {
                if (this.tool == null)
                {
                    this.tool = new ToolComponent
                    {
                        Name = Constants.PCI_DSS_V321.Name,
                        Guid = Constants.PCI_DSS_V321.Guid,
                        ReleaseDateUtc = Constants.PCI_DSS_V321.ReleaseDate,
                        InformationUri = new Uri("https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-2-1.pdf"),
                        DownloadUri = new Uri("https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-2-1.pdf"),
                        Organization = "PCI Security Standards Council",
                        ShortDescription = new MultiformatMessageString { Text = "Payment Card Industry (PCI) Data Security Standard" },
                        Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                        IsComprehensive = true,
                    };
                }
                return this.tool;
            }
        }

        protected override bool VerifySource(string sourceFilePath, out Uri sourceUri)
        {
            return Uri.TryCreate(sourceFilePath, UriKind.RelativeOrAbsolute, out sourceUri)
                   && File.Exists(sourceFilePath);
        }

        protected override Task<IList<PciDssItem>> GetSourceItemsAsync(Uri sourceUri)
        {
            return this.ReadFromCsvAsync(sourceUri.OriginalString);
        }

        protected override IList<ReportingDescriptor> ConvertSourceItemToTaxa(List<PciDssItem> items)
        {
            return items.Select(r => new ReportingDescriptor
            {
                Id = $"{r.Category} {r.Id}",
                Name = string.IsNullOrEmpty(r.Name) ? $"{r.Category} {r.Id}" : r.Name,
                FullDescription = string.IsNullOrEmpty(r.Description) ? null : new MultiformatMessageString { Text = r.Description },
                DefaultConfiguration = new ReportingConfiguration { Level = FailureLevel.Warning }, // default
                // no relationship defined
            }).ToList();
        }

        protected override ExternalPropertyFileReferences GetTaxonomyReference(ToolComponent tool)
        {
            // no reference taxonomy defined
            return null;
        }
        #endregion

        private async Task<IList<PciDssItem>> ReadFromCsvAsync(string filePath)
        {
            using FileStream input = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var textReader = new StreamReader(input);
            using var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);

            return await csvReader.GetRecordsAsync<PciDssItem>().ToListAsync();
        }
    }
}