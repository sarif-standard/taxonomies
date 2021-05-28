// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Taxonomy.Common
{
    public static class Constants
    {
        private static class Guid
        {
            public static string Cwe = "FFC64C90-42B6-44CE-8BEB-F6B7DAE649E5";
            public static string NistSP80053V5 = "AAFBAB93-5201-419E-8443-D4925C542398";
            public static string NistSP80053V4 = "EE4E6942-6346-45EE-BBA2-9998A214D80E";
            public static string NistSP80063B = "CC3BE6A5-E774-41CF-B74C-C928269B6778";
            public static string Owasp = "BBEA1F18-F56A-4202-B9A9-3FC348B81E5A";
            public static string WASCV1 = "E30814F7-D50D-4936-9B0C-B80ACD412434";
            public static string WASCV2 = "982D1AD0-AEAB-4960-BFCE-A18953EFD6D6";
        }

        public static TaxonomyData CWE = new TaxonomyData
        {
            Guid = Guid.Cwe,
            Name = "CWE",
            Location = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/CWE_v4.4.sarif"
        };

        public static TaxonomyData NistSP80053V4 = new TaxonomyData
        {
            Guid = Guid.NistSP80053V4,
            Name = "NIST",
            Location = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/NIST_SP800-53_v4.sarif"
        };

        public static TaxonomyData NistSP80053V5 = new TaxonomyData
        {
            Guid = Guid.NistSP80053V5,
            Name = "NIST",
            Location = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/NIST_SP800-53_v5.sarif"
        };

        public static TaxonomyData NistSP80063B = new TaxonomyData
        {
            Guid = Guid.NistSP80063B,
            Name = "Nist SP800-63B",
            Location = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/NIST_SP800-63B_v1.sarif"
        };

        public static TaxonomyData Owasp = new TaxonomyData
        {
            Guid = Guid.Owasp,
            Name = "OWASP",
            Location = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/OWASP_ASVS_v4.0.2.sarif"
        };

        public static TaxonomyData WASCV1 = new TaxonomyData
        {
            Guid = Guid.WASCV1,
            Name = "WASC 1.00",
            Location = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/WASC_1.00.sarif"
        };

        public static TaxonomyData WASCV2 = new TaxonomyData
        {
            Guid = Guid.WASCV2,
            Name = "WASC 2.00",
            Location = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/WASC_2.00.sarif"
        };
    }

    public class TaxonomyData
    {
        public string Name;
        public string Guid;
        public string Location;
    }
}
