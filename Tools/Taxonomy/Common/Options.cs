// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;

namespace Taxonomy
{
    internal interface IOptions
    {
        [Option("type", HelpText = "type of the standard to process.", Required = true)]
        string Type { get; set; }

        [Option("source-file-path", HelpText = "Source file path to process.", Required = true)]
        string SourceFilePath { get; set; }

        [Option("target-file-path", HelpText = "Target file path to save.", Required = true)]
        string TargetFilePath { get; set; }

        [Option("version", HelpText = "Version string in target file.", Required = true)]
        string Version { get; set; }

        [Option("release-date", HelpText = "ReleaseDateUtc string in target file. Format: YYYY-MM-DD", Required = true)]
        string ReleaseDateUtc { get; set; }
    }

    [Verb("generate-cwe", false, HelpText = "Generate CWE Sarif file")]
    internal class CweOptions : IOptions
    {
        public string Type { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string Version { get; set; }
        public string ReleaseDateUtc { get; set; }
    }

    [Verb("generate-owasp", false, HelpText = "Generate OWASP Sarif file")]
    internal class OwaspOptions : IOptions
    {
        public string Type { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string Version { get; set; }
        public string ReleaseDateUtc { get; set; }
    }

    [Verb("generate-nist", false, HelpText = "Generate NIST Sarif file")]
    internal class NistOptions : IOptions
    {
        public string Type { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string Version { get; set; }
        public string ReleaseDateUtc { get; set; }
    }

    [Verb("add-owasprelationship-to-cwe", false, HelpText = "Add OWASP relationship To CWE Sarif file")]
    internal class AddOwaspRelationshipToCweOptions
    {
        [Option("source-cwe-file-path", HelpText = "Source CWE file path to process.", Required = true)]
        public string CweFilePath { get; set; }

        [Option("source-owasp-file-path", HelpText = "Source OWASP file path to process.", Required = true)]
        public string OwaspFilePath { get; set; }

        [Option("target-cwe-file-path", HelpText = "Target CWE file path to save.", Required = true)]
        public string TargetFilePath { get; set; }
    }

    [Verb("generate-wasc", false, HelpText = "Generate WASC Taxonomies Sarif file")]
    internal class WascOptions : IOptions
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string Version { get; set; }
        public string ReleaseDateUtc { get; set; }
    }
}
