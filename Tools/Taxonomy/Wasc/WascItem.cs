// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using CsvHelper.Configuration.Attributes;

namespace Tools.Wasc
{
    public class WascBaseItem
    {
        [Name("Id")]
        public string WascId { get; set; }

        [Name("Name")]
        public string WascName { get; set; }

        [Name("Description")]
        public string Description { get; set; }
    }

    public class WascV1Item : WascBaseItem
    {
        [Name("Class")]
        public string Category { get; set; }
    }

    public class WascV2Item : WascBaseItem
    {
        public string WascLink { get; set; }
        public IEnumerable<string> CweIds { get; set; }
        public IEnumerable<string> CapecIds { get; set; }
        public IEnumerable<string> Sans_Cwe_Top_25_2009 { get; set; }
        public IEnumerable<string> Owasp_Top_Ten_2010 { get; set; }
        public IEnumerable<string> Owasp_Top_Ten_2007 { get; set; }
        public IEnumerable<string> Owasp_Top_Ten_2004 { get; set; }

    }
}
