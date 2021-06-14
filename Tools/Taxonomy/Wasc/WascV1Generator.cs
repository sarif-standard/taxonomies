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

namespace Tools.Wasc
{
    public class WascV1Generator : GeneratorBase<WascV1Item>
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
                        Name = Constants.WASC_V1.Name,
                        Guid = Constants.WASC_V1.Guid,
                        ReleaseDateUtc = Constants.WASC_V1.ReleaseDate,
                        InformationUri = new Uri("http://projects.webappsec.org/Threat%20Classification%20Previous%20Versions"),
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
            return Uri.TryCreate(sourceFilePath, UriKind.RelativeOrAbsolute, out sourceUri)
                   && File.Exists(sourceFilePath);
        }

        protected override Task<IList<WascV1Item>> GetSourceItemsAsync(Uri sourceUri)
        {
            return this.ReadFromCsvAsync(sourceUri.OriginalString);
        }

        private async Task<IList<WascV1Item>> ReadFromCsvAsync(string filePath)
        {
            using FileStream input = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var textReader = new StreamReader(input);
            using var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);

            return await csvReader.GetRecordsAsync<WascV1Item>().ToListAsync();
        }

        protected override IList<ReportingDescriptor> ConvertSourceItemToTaxa(List<WascV1Item> items)
        {
            return items.Select(r => new ReportingDescriptor
            {
                Id = r.WascId,
                Name = r.WascName,
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
    }
}
