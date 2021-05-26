# SARIF Taxonomies

This repo stores SARIF Taxonomies

## Data Source

| **Source Url** | **Target File Name** | **SARIF File** |
-----|-----|-----
CWE | [link](https://cwe.mitre.org/data/xml/cwec\_latest.xml.zip)| [CWE_v4.4.sarif](CWE_v4.4.sarif)
Nist | [link](https://csrc.nist.gov/CSRC/media/Publications/sp/800-53/rev-5/final/documents/sp800-53r5-control-catalog.xlsx) | [NIST_SP800-53_v5.sarif](NIST_SP800-53_v5.sarif)
OWASP | [link](https://github.com/OWASP/ASVS/raw/v4.0.2/4.0/docs/_en/OWASP%20Application%20Security%20Verification%20Standard%204.0.2-en.csv) | [OWASP_ASVS_v4.0.2.sarif](OWASP_ASVS_v4.0.2.sarif)

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

## License

Microsoft SARIF Taxonomies are licensed under the [MIT license](https://github.com/microsoft/sarif-visualstudio-extension/blob/main/LICENSE).
