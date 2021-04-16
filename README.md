# SARIF Taxonomies

This repo stores SARIF Taxonomies

## Tool Usage

Generate CWE Sarif file
generatecwe "..\..\..\..\Source\cwec_v4.4.xml" "..\..\..\..\..\CWE_v4.4.sarif"

Generate OWASP ASVS Sarif file
generateowasp "..\..\..\..\Source\OWASP Application Security Verification Standard 4.0.2-en.csv" "..\..\..\..\..\OWASP_ASVS_v4.0.2.sarif"

Generate NISP SP800-63B Sarif file
generatenispsp80053 "..\..\..\..\Source\sp800-53r5-control-catalog.csv" "..\..\..\..\..\NISP_SP800-53_v5.sarif"

Generate NISP SP800-63B Sarif file
generatenispsp80063b "..\..\..\..\Source\800-63-3-nist-pages\sp800-63b" "..\..\..\..\..\NISP_SP800-63B_v1.sarif"

## Data Source

 |**Source Url**|**Target File Name**
-----|-----|-----
CWE|https://cwe.mitre.org/data/xml/cwec\_latest.xml.zip|CWE\_v4.4.sarif
Nisp|https://csrc.nist.gov/CSRC/media/Publications/sp/800-53/rev-5/final/documents/sp800-53r5-control-catalog.xlsx|NISP\_SP800-53\_v5.sarif
Owasp|https://github.com/OWASP/ASVS/raw/v4.0.2/4.0/docs\_en/OWASP%20Application%20Security%20Verification%20Standard%204.0.2-en.csv|OWASP\_ASVS\_v4.0.2.sarif

## License

Microsoft SARIF Viewer is licensed under the [MIT license](https://github.com/microsoft/sarif-visualstudio-extension/blob/main/LICENSE).