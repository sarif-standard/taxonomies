// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CsvHelper.Configuration.Attributes;

using Tools.Common;

namespace Tools.Pci
{
    public class PciSsfItem : TaxaItemBase
    {
        [Name("Category")]
        public string Category { get; set; }

        [Name("Id")]
        public string Id { get; set; }

        [Name("Name")]
        public string Name { get; set; }

        [Name("Description")]
        public string Description { get; set; }
    }
}
