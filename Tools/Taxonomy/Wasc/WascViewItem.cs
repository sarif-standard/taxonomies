// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Tools.Wasc
{
    public class WascViewItem
    {
        public string WascId { get; set; }
        public string WascName { get; set; }
        public string WascLink { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> CweIds { get; set; }
        public IEnumerable<string> CapecIds { get; set; }
        public IEnumerable<string> Sans_Cwe_Top_25_2009 { get; set; }
        public IEnumerable<string> Owasp_Top_Ten_2010 { get; set; }
        public IEnumerable<string> Owasp_Top_Ten_2007 { get; set; }
        public IEnumerable<string> Owasp_Top_Ten_2004 { get; set; }

    }
}
