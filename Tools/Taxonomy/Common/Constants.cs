// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Taxonomy.Common
{
    public static class Constants
    {
        private const string REPO_PATH = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/";

        private static class Guid
        {
            public static string Cwe = "FFC64C90-42B6-44CE-8BEB-F6B7DAE649E5";
            public static string NistSP80053V5 = "AAFBAB93-5201-419E-8443-D4925C542398";
            public static string NistSP80053V4 = "EE4E6942-6346-45EE-BBA2-9998A214D80E";
            public static string NistSP80063B = "CC3BE6A5-E774-41CF-B74C-C928269B6778";
            public static string OwaspASVSV402 = "BBEA1F18-F56A-4202-B9A9-3FC348B81E5A";
            public static string OwaspMobileTop102014 = "F539BA7E-3C82-4C60-B8C9-EC151E27E140";
            public static string OwaspMobileTop102016 = "62118DBB-B3B1-4489-BE8E-938A6C071CED";
            public static string PCI_SSF_V1 = "8DD3BF80-F4D3-46F2-8E9E-6BB801E973F6";
            public static string WASCV1 = "E30814F7-D50D-4936-9B0C-B80ACD412434";
            public static string WASCV2 = "982D1AD0-AEAB-4960-BFCE-A18953EFD6D6";
        }

        public static TaxonomyData CWE = new TaxonomyData
        {
            Guid = Guid.Cwe,
            Name = "CWE",
            Location = REPO_PATH + "CWE_v4.4.sarif",
            ReleaseDate = "2020-12-10",
        };

        public static TaxonomyData NistSP80053V4 = new TaxonomyData
        {
            Guid = Guid.NistSP80053V4,
            Name = "NIST",
            Location = REPO_PATH + "NIST_SP800-53_v4.sarif",
            ReleaseDate = "2015-01-22",
        };

        public static TaxonomyData NistSP80053V5 = new TaxonomyData
        {
            Guid = Guid.NistSP80053V5,
            Name = "NIST",
            Location = REPO_PATH + "NIST_SP800-53_v5.sarif",
            ReleaseDate = "2020-12-10",
        };

        public static TaxonomyData NistSP80063B = new TaxonomyData
        {
            Guid = Guid.NistSP80063B,
            Name = "Nist SP800-63B",
            Location = REPO_PATH + "NIST_SP800-63B_v1.sarif",
            ReleaseDate = "2020-03-02",
        };

        public static TaxonomyData OwaspASVSV402 = new TaxonomyData
        {
            Guid = Guid.OwaspASVSV402,
            Name = "OWASP",
            Location = REPO_PATH + "OWASP_ASVS_v4.0.2.sarif",
            ReleaseDate = "2020-10-01",
        };

        public static TaxonomyData PCI_SSF_V1 = new TaxonomyData
        {
            Guid = Guid.PCI_SSF_V1,
            Name = "PCI SSF V1.1",
            Location = REPO_PATH + "PCI_SSF_v1.1.sarif",
            ReleaseDate = "2021-02-01",
        };

        public static TaxonomyData OwaspMobileTop102014 = new TaxonomyData
        {
            Guid = Guid.OwaspMobileTop102014,
            Name = "OWASP Mobile Top 10 2014",
            Location = REPO_PATH + "OWASP_MobileTop10_v2014.sarif"
        };

        public static TaxonomyData OwaspMobileTop102016 = new TaxonomyData
        {
            Guid = Guid.OwaspMobileTop102016,
            Name = "OWASP Mobile Top 10 2016",
            Location = REPO_PATH + "OWASP_MobileTop10_v2016.sarif"
        };

        public static TaxonomyData WASCV1 = new TaxonomyData
        {
            Guid = Guid.WASCV1,
            Name = "WASC 1.00",
            Location = REPO_PATH + "WASC_1.00.sarif",
            ReleaseDate = "2004-01-01",
        };

        public static TaxonomyData WASCV2 = new TaxonomyData
        {
            Guid = Guid.WASCV2,
            Name = "WASC 2.00",
            Location = REPO_PATH + "WASC_2.00.sarif",
            ReleaseDate = "2010-01-01",
        };
    }

    public class TaxonomyData
    {
        public string Name;
        public string Guid;
        public string Location;
        public string ReleaseDate;
    }
}
