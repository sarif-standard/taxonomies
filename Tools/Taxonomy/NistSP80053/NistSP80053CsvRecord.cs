// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CsvHelper.Configuration.Attributes;

namespace Taxonomy
{
    public class NistSP80053CsvRecord
    {
        [Name("Control Identifier")]
        public string Id { get; set; }

        [Name("Control (or Control Enhancement) Name")]
        public string Name { get; set; }

        [Name("Control Text")]
        public string ShortDescription { get; set; }

        [Name("Discussion")]
        public string FullDescription { get; set; }

        [Name("Related Controls")]
        public string Related { get; set; }
    }
}
