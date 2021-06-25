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

using Tools.Common;

namespace Tools.Pci
{
    public class PciDssGenerator : GeneratorBase<PciDssItem>
    {
        private readonly string version;
        public PciDssGenerator(string version)
        {
            this.version = version;
        }

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
                        Organization = "PCI Security Standards Council",
                        ShortDescription = new MultiformatMessageString { Text = "Payment Card Industry (PCI) Data Security Standard" },
                        Contents = ToolComponentContents.LocalizedData | ToolComponentContents.NonLocalizedData,
                        IsComprehensive = true,
                    };

                    switch (this.version)
                    {
                        case "3.2.1":
                            this.tool.Name = Constants.PCI_DSS_V3_2_1.Name;
                            this.tool.Guid = Constants.PCI_DSS_V3_2_1.Guid;
                            this.tool.ReleaseDateUtc = Constants.PCI_DSS_V3_2_1.ReleaseDate;
                            this.tool.InformationUri = new Uri("https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-2-1.pdf");
                            this.tool.DownloadUri = new Uri("https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-2-1.pdf");
                            break;
                        case "3.2":
                            this.tool.Name = Constants.PCI_DSS_V3_2.Name;
                            this.tool.Guid = Constants.PCI_DSS_V3_2.Guid;
                            this.tool.ReleaseDateUtc = Constants.PCI_DSS_V3_2.ReleaseDate;
                            this.tool.InformationUri = new Uri("https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-2.pdf");
                            this.tool.DownloadUri = new Uri("https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-2.pdf");
                            break;
                        case "3.1":
                            this.tool.Name = Constants.PCI_DSS_V3_1.Name;
                            this.tool.Guid = Constants.PCI_DSS_V3_1.Guid;
                            this.tool.ReleaseDateUtc = Constants.PCI_DSS_V3_1.ReleaseDate;
                            this.tool.InformationUri = new Uri("https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-1.pdf");
                            this.tool.DownloadUri = new Uri("https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-1.pdf");
                            break;
                        default:
                            throw new Exception("Not supported version.");
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
