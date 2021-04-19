// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;

namespace Taxonomy
{
    internal interface IOptions
    {
        [Value(0, MetaName = "source file path", HelpText = "Source file path to process.", Required = true)]
        string SourceFilePath { get; set; }

        [Value(1, MetaName = "target file path", HelpText = "Target file path to save.", Required = true)]
        string TargetFilePath { get; set; }

        [Value(2, MetaName = "version", HelpText = "Version string in target file.", Required = true)]
        string Version { get; set; }

        [Value(3, MetaName = "release date utc", HelpText = "ReleaseDateUtc string in target file.", Required = true)]
        string ReleaseDateUtc { get; set; }
    }

    [Verb("generate-cwe", false, HelpText = "Generate CWE Sarif file")]
    internal class CweOptions : IOptions
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string Version { get; set; }
        public string ReleaseDateUtc { get; set; }
    }

    [Verb("generate-owasp", false, HelpText = "Generate OWASP Sarif file")]
    internal class OwaspOptions : IOptions
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string Version { get; set; }
        public string ReleaseDateUtc { get; set; }
    }

    [Verb("generate-nistsp80053", false, HelpText = "Generate NIST SP800-63B Sarif file")]
    internal class NistSP80053Options : IOptions
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string Version { get; set; }
        public string ReleaseDateUtc { get; set; }
    }

    [Verb("generate-nistsp80063b", false, HelpText = "Generate NIST SP800-63B Sarif file")]
    internal class GenerateNistSP80063BOptions
    {
        [Value(0, MetaName = "Source NIST SP800-63B folder path", HelpText = "Source NIST SP800-63B folder path with md files in it to process.", Required = true)]
        public string SourceFolderPath { get; set; }

        [Value(1, MetaName = "target file path", HelpText = "Target file path to save.", Required = true)]
        public string TargetFilePath { get; set; }

        [Value(2, MetaName = "version", HelpText = "Version string in target file.", Required = true)]
        public string Version { get; set; }

        [Value(3, MetaName = "release date utc", HelpText = "ReleaseDateUtc string in target file.", Required = true)]
        public string ReleaseDateUtc { get; set; }
    }

    [Verb("add-owasprelationship-to-cwe", false, HelpText = "Add OWASP relationship To CWE Sarif file")]
    internal class AddOwaspRelationshipToCweOptions
    {
        [Value(0, MetaName = "Source CWE file path", HelpText = "Source CWE file path to process.", Required = true)]
        public string CweFilePath { get; set; }

        [Value(1, MetaName = "Source OWASP file path", HelpText = "Source OWASP file path to process.", Required = true)]
        public string OwaspFilePath { get; set; }

        [Value(2, MetaName = "Target CWE file path", HelpText = "Target CWE file path to save.", Required = true)]
        public string TargetFilePath { get; set; }
    }
}
