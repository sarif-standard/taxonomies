// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

namespace Taxonomy.Common
{
    public class TaxonomyGenerator
    {
        protected SarifLog ReadFromSarif(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SarifLog>(jsonString);
        }
    }
}
