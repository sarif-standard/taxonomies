# SARIF Taxonomies

This repo stores SARIF Taxonomies

## Data Source

| **Taxonomy** | **Version** | **Target File Name** | **SARIF File** |
-----|-----|-----|-----
CWE | v4.3 | [link](https://cwe.mitre.org/data/xml/cwec/_latest.xml.zip) | [CWE_v4.3.sarif](CWE_v4.3.sarif)
CWE| v4.4 | [link](https://cwe.mitre.org/data/xml/cwec_v4.4.xml.zip) | [CWE_v4.4.sarif](CWE_v4.4.sarif)
Nist | SP800-53 v4 | [link](https://raw.githubusercontent.com/usnistgov/oscal-content/master/nist.gov/SP800-53/rev4/json/NIST_SP-800-53_rev4_catalog.json) | [NIST_SP800-53_v4.sarif](NIST_SP800-53_v4.sarif)
Nist | SP800-53 v5 | [link](https://csrc.nist.gov/CSRC/media/Publications/sp/800-53/rev-5/final/documents/sp800-53r5-control-catalog.xlsx) | [NIST_SP800-53_v5.sarif](NIST_SP800-53_v5.sarif)
Nist | SP800-63B v1 | [link](https://pages.nist.gov/800-63-3/sp800-63b.html) | [NIST_SP800-63B_v1.sarif](NIST_SP800-63B_v1.sarif)
OWASP | v4.0.2 | [link](https://github.com/OWASP/ASVS/raw/v4.0.2/4.0/docs/_en/OWASP%20Application%20Security%20Verification%20Standard%204.0.2-en.csv) | [OWASP_ASVS_v4.0.2.sarif](OWASP_ASVS_v4.0.2.sarif)
WASC | v1.0.0 | [link](http://projects.webappsec.org/Threat%20Classification%20Previous%20Versions) |[WASC_1.00.sarif](WASC_1.00.sarif)
WASC | v2.0.0 | [link](http://projects.webappsec.org/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View) |[WASC_2.00.sarif](WASC_2.00.sarif)

## Tool Usage

Download form official website using the links in Data Source section below. Unzip as needed and put in folder.
e.g. Source folder in the sample command line below.

Generate CWE Sarif file

```bash
generate-cwe --source-file-path "cwec_v4.4.xml" --target-file-path "CWE_v4.4.sarif" --version "4.4" --release-date "2020-12-10"
```

Generate OWASP ASVS Sarif file

```bash
generate-owasp --source-file-path "OWASP Application Security Verification Standard 4.0.2-en.csv" --target-file-path "OWASP_ASVS_v4.0.2.sarif" --version "4.0.2" --release-date "2020-10-01"
```

Generate NIST SP800-53 Sarif file

```bash
generate-nistsp80053 --source-file-path "sp800-53r5-control-catalog.csv" --target-file-path "NIST_SP800-53_v5.sarif" --version "5" --release-date "2020-12-10"
```

Generate NIST SP800-63B Sarif file

```bash
generate-nistsp80063b --source-folder-path "800-63-3-nist-pages\sp800-63b" --target-file-path "NIST_SP800-63B_v1.sarif" --version "1" --release-date "2020-03-02"
```

Generate WASC 1.00 Sarif file

```bash
generate-wasc --source-file-path "wasc_1.00.csv" --target-file-path "..\..\..\..\..\WASC_2.00.sarif" --version "1.00" --release-date "2004-01-01"
```

Generate WASC 2.00 Sarif file

```bash
generate-wasc --source-file-path "http://projects.webappsec.org/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View" --target-file-path "..\..\..\..\..\WASC_2.00.sarif" --version "2.00" --release-date "2010-01-01"
```
## License

Microsoft SARIF Taxonomies are licensed under the [MIT license](https://github.com/microsoft/sarif-visualstudio-extension/blob/main/LICENSE).
