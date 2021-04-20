// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CsvHelper.Configuration.Attributes;

namespace Taxonomy
{
    public class OwaspASVSCsvRecord
    {
        [Name("req_id")]
        public string Id { get; set; }

        [Name("req_description")]
        public string FullDescription { get; set; }

        [Name("cwe")]
        public string RelatedCwe { get; set; }

        [Name("nist")]
        public string RelatedNistSP80063B { get; set; }
    }
}
