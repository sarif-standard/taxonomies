# SARIF Taxonomies

This repo stores SARIF Taxonomies

## Tool Usage

Download form offical website using the links in Data Source section below. Uzip as needed and put in folder. 
e.g. Source folder in the sample commandline below.

Generate CWE Sarif file
generate-cwe "..\..\..\..\Source\cwec_v4.4.xml" "..\..\..\..\..\CWE_v4.4.sarif" "4.4" "2020-12-10"

Generate OWASP ASVS Sarif file
generate-owasp "..\..\..\..\Source\OWASP Application Security Verification Standard 4.0.2-en.csv" "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif" "4.0.2" "2020-10-01"

Generate NISP SP800-53 Sarif file
generate-nispsp80053 "..\..\..\..\Source\sp800-53r5-control-catalog.csv" "..\..\..\..\..\NISP_SP800-53_v5.sarif" "5" "2020-12-10"

Generate NISP SP800-63B Sarif file
generate-nispsp80063b "..\..\..\..\Source\800-63-3-nist-pages\sp800-63b" "..\..\..\..\..\NISP_SP800-63B_v1.sarif" "1" "2020-03-02"

## Data Source

 |**Source Url**|**Target File Name**
-----|-----|-----
CWE|https://cwe.mitre.org/data/xml/cwec\_latest.xml.zip|CWE\_v4.4.sarif
Nisp|https://csrc.nist.gov/CSRC/media/Publications/sp/800-53/rev-5/final/documents/sp800-53r5-control-catalog.xlsx|NISP\_SP800-53\_v5.sarif
Owasp|https://github.com/OWASP/ASVS/raw/v4.0.2/4.0/docs\_en/OWASP%20Application%20Security%20Verification%20Standard%204.0.2-en.csv|OWASP\_ASVS\_v4.0.2.sarif

## License

Microsoft SARIF Taxonomies are licensed under the [MIT license](https://github.com/microsoft/sarif-visualstudio-extension/blob/main/LICENSE).