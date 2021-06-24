// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Taxonomy.Common
{
    public static class Constants
    {
        private const string REPO_PATH = "https://raw.githubusercontent.com/sarif-standard/taxonomies/main/";

        private static class Guid
        {
            public static string CWE_Comprehensive_V43 = "FFC64C90-42B6-44CE-8BEB-F6B7DAE649E5";
            public static string CWE_Comprehensive_V44 = "7D699257-C37D-4A10-9C98-D7DB481F1A8B";
            public static string CWE_Top_25_2019 = "B37033B9-1D33-482B-AD09-C6E788340839";
            public static string CWE_Top_25_2020 = "4D54E275-787C-493E-9F1A-4149C1A906C0";
            public static string Nist_SP80053_V4 = "EE4E6942-6346-45EE-BBA2-9998A214D80E";
            public static string Nist_SP80053_V5 = "AAFBAB93-5201-419E-8443-D4925C542398";
            public static string Nist_SP80063B = "CC3BE6A5-E774-41CF-B74C-C928269B6778";
            public static string Owasp_ASVS_V402 = "BBEA1F18-F56A-4202-B9A9-3FC348B81E5A";
            public static string Owasp_Mobile_Top_10_2014 = "F539BA7E-3C82-4C60-B8C9-EC151E27E140";
            public static string Owasp_Mobile_Top_10_2016 = "62118DBB-B3B1-4489-BE8E-938A6C071CED";
            public static string Owasp_Top_10_2004 = "35A384BD-CA28-449B-8ACC-60873A53E5CB";
            public static string Owasp_Top_10_2007 = "757DB388-A146-441F-BE0D-150395517E18";
            public static string Owasp_Top_10_2010 = "FA6B4314-F96C-4F59-B270-BF36180CFC05";
            public static string Owasp_Top_10_2013 = "89C67EDA-0925-48FF-893D-8089C98ED416";
            public static string Owasp_Top_10_2017 = "C783E54B-A1A8-4C3A-B7C5-CEFF77D3A8C3";
            public static string PCI_SSF_V1 = "8DD3BF80-F4D3-46F2-8E9E-6BB801E973F6";
            public static string WASC_V1 = "E30814F7-D50D-4936-9B0C-B80ACD412434";
            public static string WASC_V2 = "982D1AD0-AEAB-4960-BFCE-A18953EFD6D6";
        }

        public static TaxonomyData CWE_Comprehensive_V43 = new TaxonomyData
        {
            Guid = Guid.CWE_Comprehensive_V43,
            Name = "CWE",
            Location = REPO_PATH + "CWE_v4.3.sarif",
            ReleaseDate = "2020-12-10",
        };

        public static TaxonomyData CWE_Comprehensive_V44 = new TaxonomyData
        {
            Guid = Guid.CWE_Comprehensive_V44,
            Name = "CWE",
            Location = REPO_PATH + "CWE_v4.4.sarif",
            ReleaseDate = "2021-03-15",
        };

        public static TaxonomyData CWE_Top_25_2019 = new TaxonomyData
        {
            Guid = Guid.CWE_Top_25_2019,
            Name = "CWE Top 25 2019",
            Location = REPO_PATH + "CWE_Top25_v2019.sarif",
            ReleaseDate = "2019-09-18",
        };

        public static TaxonomyData CWE_Top_25_2020 = new TaxonomyData
        {
            Guid = Guid.CWE_Top_25_2020,
            Name = "CWE Top 25 2020",
            Location = REPO_PATH + "CWE_Top25_v2020.sarif",
            ReleaseDate = "2020-08-20",
        };

        public static TaxonomyData Nist_SP80053_V4 = new TaxonomyData
        {
            Guid = Guid.Nist_SP80053_V4,
            Name = "NIST",
            Location = REPO_PATH + "NIST_SP800-53_v4.sarif",
            ReleaseDate = "2015-01-22",
        };

        public static TaxonomyData Nist_SP80053_V5 = new TaxonomyData
        {
            Guid = Guid.Nist_SP80053_V5,
            Name = "NIST",
            Location = REPO_PATH + "NIST_SP800-53_v5.sarif",
            ReleaseDate = "2020-12-10",
        };

        public static TaxonomyData Nist_SP80063B = new TaxonomyData
        {
            Guid = Guid.Nist_SP80063B,
            Name = "Nist SP800-63B",
            Location = REPO_PATH + "NIST_SP800-63B_v1.sarif",
            ReleaseDate = "2020-03-02",
        };

        public static TaxonomyData Owasp_ASVS_V402 = new TaxonomyData
        {
            Guid = Guid.Owasp_ASVS_V402,
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

        public static TaxonomyData Owasp_Mobile_Top_10_2014 = new TaxonomyData
        {
            Guid = Guid.Owasp_Mobile_Top_10_2014,
            Name = "OWASP Mobile Top 10 2014",
            Location = REPO_PATH + "OWASP_MobileTop10_v2014.sarif"
        };

        public static TaxonomyData Owasp_Mobile_Top_10_2016 = new TaxonomyData
        {
            Guid = Guid.Owasp_Mobile_Top_10_2016,
            Name = "OWASP Mobile Top 10 2016",
            Location = REPO_PATH + "OWASP_MobileTop10_v2016.sarif"
        };

        public static TaxonomyData Owasp_Top_10_2004 = new TaxonomyData
        {
            Guid = Guid.Owasp_Top_10_2004,
            Name = "OWASP Top 10 2004",
            Location = REPO_PATH + "OWASP_Top10_v2004.sarif"
        };

        public static TaxonomyData Owasp_Top_10_2007 = new TaxonomyData
        {
            Guid = Guid.Owasp_Top_10_2007,
            Name = "OWASP Top 10 2007",
            Location = REPO_PATH + "OWASP_Top10_v2007.sarif"
        };

        public static TaxonomyData Owasp_Top_10_2010 = new TaxonomyData
        {
            Guid = Guid.Owasp_Top_10_2010,
            Name = "OWASP Top 10 2010",
            Location = REPO_PATH + "OWASP_Top10_v2010.sarif"
        };

        public static TaxonomyData Owasp_Top_10_2013 = new TaxonomyData
        {
            Guid = Guid.Owasp_Top_10_2013,
            Name = "OWASP Top 10 2013",
            Location = REPO_PATH + "OWASP_Top10_v2013.sarif"
        };

        public static TaxonomyData Owasp_Top_10_2017 = new TaxonomyData
        {
            Guid = Guid.Owasp_Top_10_2017,
            Name = "OWASP Top 10 2017",
            Location = REPO_PATH + "OWASP_Top10_v2017.sarif"
        };

        public static TaxonomyData WASC_V1 = new TaxonomyData
        {
            Guid = Guid.WASC_V1,
            Name = "WASC 1.00",
            Location = REPO_PATH + "WASC_1.00.sarif",
            ReleaseDate = "2004-01-01",
        };

        public static TaxonomyData WASC_V2 = new TaxonomyData
        {
            Guid = Guid.WASC_V2,
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
