// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis.Sarif;
using Newtonsoft.Json;
using System.IO;

namespace Taxonomy.Common
{
    public class TaxonomyGenerator
    {
        public SarifLog ReadFromSarif(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SarifLog>(jsonString);
        }
    }
}
