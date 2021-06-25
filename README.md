# SARIF Taxonomies

This repo stores SARIF Taxonomies

## Data Source

| **Taxonomy** | **Version** | **Souce File** | **SARIF File** |
-----|-----|-----|-----
CWE | v4.3 | [link](https://cwe.mitre.org/data/xml/cwec/cwec_v4.3.xml.zip) | [CWE_v4.3.sarif](CWE_v4.3.sarif)
CWE| v4.4 | [link](https://cwe.mitre.org/data/xml/cwec_v4.4.xml.zip) | [CWE_v4.4.sarif](CWE_v4.4.sarif)
Nist | SP800-53 v4 | [link](https://raw.githubusercontent.com/usnistgov/oscal-content/master/nist.gov/SP800-53/rev4/json/NIST_SP-800-53_rev4_catalog.json) | [NIST_SP800-53_v4.sarif](NIST_SP800-53_v4.sarif)
Nist | SP800-53 v5 | [link](https://csrc.nist.gov/CSRC/media/Publications/sp/800-53/rev-5/final/documents/sp800-53r5-control-catalog.xlsx) | [NIST_SP800-53_v5.sarif](NIST_SP800-53_v5.sarif)
Nist | SP800-63B v1 | [link](https://pages.nist.gov/800-63-3/sp800-63b.html) | [NIST_SP800-63B_v1.sarif](NIST_SP800-63B_v1.sarif)
OWASP | ASVS v4.0.2 | [link](https://github.com/OWASP/ASVS/raw/v4.0.2/4.0/docs/_en/OWASP%20Application%20Security%20Verification%20Standard%204.0.2-en.csv) | [OWASP_ASVS_v4.0.2.sarif](OWASP_ASVS_v4.0.2.sarif)
OWASP | Mobile Top 10 v2014 | [link](https://github.com/OWASP/www-project-mobile-top-10/tree/master/2014-risks) | [OWASP_MobileTop10_v2014.sarif](OWASP_MobileTop10_v2014.sarif)
OWASP | Mobile Top 10 v2016 | [link](https://github.com/OWASP/www-project-mobile-top-10/tree/master/2016-risks) | [OWASP_MobileTop10_v2016.sarif](OWASP_MobileTop10_v2016.sarif)
OWASP | Top 10 v2004 | [link](https://raw.githubusercontent.com/owasp-top/owasp-top-2004/master/README.md) | [OWASP_Top10_v2004.sarif](OWASP_Top10_v2004.sarif)
OWASP | Top 10 v2007 | [link](https://raw.githubusercontent.com/owasp-top/owasp-top-2007/master/README.md) | [OWASP_Top10_v2007.sarif](OWASP_Top10_v2007.sarif)
OWASP | Top 10 v2010 | [link](https://raw.githubusercontent.com/owasp-top/owasp-top-2010/master/README.md) | [OWASP_Top10_v2010.sarif](OWASP_Top10_v2010.sarif)
OWASP | Top 10 v2013 | [link](https://raw.githubusercontent.com/owasp-top/owasp-top-2013/master/README.md) | [OWASP_Top10_v2013.sarif](OWASP_Top10_v2013.sarif)
OWASP | Top 10 v2017 | [link](https://raw.githubusercontent.com/owasp-top/owasp-top-2017/master/README.md) | [OWASP_Top10_v2017.sarif](OWASP_Top10_v2017.sarif)
PCI | SSF V1.1 | [link](https://www.pcisecuritystandards.org/documents/PCI-Secure-Software-Standard-v1_1.pdf) | [PCI_SSF_V1.1.sarif](PCI_SSF_V1.1.sarif)
PCI | DSS V3.2.1 | [link](https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-2-1.pdf) | [PCI_DSS_V3.2.1.sarif](PCI_DSS_V3.2.1.sarif)
WASC | v1.0.0 | [link](http://projects.webappsec.org/Threat%20Classification%20Previous%20Versions) |[WASC_1.00.sarif](WASC_1.00.sarif)
WASC | v2.0.0 | [link](http://projects.webappsec.org/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View) |[WASC_2.00.sarif](WASC_2.00.sarif)

## Tool Usage

Download form official website using the links in Data Source section above. Unzip as needed.
Execute the tool with proper parameters, samples below.

Generate CWE Sarif file

```bash
generate-cwe --source-file-path "cwec_v4.4.xml" --target-file-path "CWE_v4.4.sarif" --version "4.4"
```

Generate OWASP ASVS Sarif file

```bash
generate-owasp --type asvs --source-file-path "OWASP Application Security Verification Standard 4.0.2-en.csv" --target-file-path "OWASP_ASVS_v4.0.2.sarif" --version "4.0.2"
```

Generate OWASP Mobile Top 10 Sarif file

```bash
generate-owasp --type mobiletop10 --source-file-path "\www-project-mobile-top-10-master\2014-risks" --target-file-path "OWASP_MobileTop10_v2014.sarif" --version "2014"
```

Generate OWASP Top 10 Sarif file

```bash
generate-owasp --type top10 --source-file-path "https://raw.githubusercontent.com/owasp-top/owasp-top-2004/master/README.md" --target-file-path "OWASP_Top10_v2004.sarif" --version "2004"
```

Generate NIST SP800-53 Sarif file

```bash
generate-nist --type sp80053 --source-file-path "sp800-53r5-control-catalog.csv" --target-file-path "NIST_SP800-53_v5.sarif" --version "5"
```

Generate NIST SP800-63B Sarif file

```bash
generate-nist --type sp80063b --source-folder-path "800-63-3-nist-pages\sp800-63b" --target-file-path "NIST_SP800-63B_v1.sarif" --version "1"
```

Generate PCI SSF 1.1 Sarif file

```bash
generate-pci --type ssf --source-file-path "pci_ssf_v1.1.csv" --target-file-path "..\..\..\..\..\PCI_SSF_V1.1.sarif" --version "1.1"
```

Generate PCI DSS 3.2.1 Sarif file

```bash
generate-pci --type ssf --source-file-path "pci_dss_v3.2.1.csv" --target-file-path "..\..\..\..\..\PCI_DSS_V3.2.1.sarif" --version "3.2.1"
```

Generate WASC 1.00 (WASC 24 + 2) Sarif file

```bash
generate-wasc --source-file-path "wasc_1.00.csv" --target-file-path "..\..\..\..\..\WASC_2.00.sarif" --version "1.00"
```

Generate WASC 2.00 Sarif file

```bash
generate-wasc --source-file-path "http://projects.webappsec.org/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View" --target-file-path "..\..\..\..\..\WASC_2.00.sarif" --version "2.00"
```
## License

Microsoft SARIF Taxonomies are licensed under the [MIT license](https://github.com/microsoft/sarif-visualstudio-extension/blob/main/LICENSE).
