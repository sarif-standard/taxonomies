# SARIF Taxonomies

This repo stores SARIF Taxonomies

## Tool Usage

Download form offical website using the links in Data Source section below. Uzip as needed and put in folder. 
e.g. Source folder in the sample commandline below.

Generate CWE Sarif file
generate-cwe --source-file-path "..\..\..\..\Source\cwec_v4.4.xml" --target-file-path "..\..\..\..\..\CWE_v4.4.sarif" --version "4.4" --release-date "2020-12-10"

Generate OWASP ASVS Sarif file
generate-owasp --source-file-path "..\..\..\..\Source\OWASP Application Security Verification Standard 4.0.2-en.csv" --target-file-path "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" --version "4.0.2" --release-date "2020-10-01"

Generate NIST SP800-53 Sarif file
generate-nistsp80053 --source-file-path "..\..\..\..\Source\sp800-53r5-control-catalog.csv" --target-file-path "..\..\..\..\..\NIST_SP800-53_v5.sarif" --version "5" --release-date "2020-12-10"

Generate NIST SP800-63B Sarif file
generate-nistsp80063b --Source-folder-path "..\..\..\..\Source\800-63-3-nist-pages\sp800-63b" --target-file-path "..\..\..\..\..\NIST_SP800-63B_v1.sarif" --version "1" --release-date "2020-03-02"

Generate WASC 2.00 Sarif file
generate-wasc --source-file-path "http://projects.webappsec.org/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View" --target-file-path "..\..\..\..\..\WASC_2.00.sarif" --version "2.00" --release-date "2010-01-01"

## Data Source

 |**Source Url**|**Target File Name**
-----|-----|-----
CWE|https://cwe.mitre.org/data/xml/cwec\_latest.xml.zip|CWE\_v4.4.sarif
Nist|https://csrc.nist.gov/CSRC/media/Publications/sp/800-53/rev-5/final/documents/sp800-53r5-control-catalog.xlsx|NIST\_SP800-53\_v5.sarif
Owasp|https://github.com/OWASP/ASVS/raw/v4.0.2/4.0/docs\_en/OWASP%20Application%20Security%20Verification%20Standard%204.0.2-en.csv|OWASP\_ASVS\_v4.0.2.sarif
WASC|http://projects.webappsec.org/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View|WASC\_2.00.sarif

## License

Microsoft SARIF Taxonomies are licensed under the [MIT license](https://github.com/microsoft/sarif-visualstudio-extension/blob/main/LICENSE).