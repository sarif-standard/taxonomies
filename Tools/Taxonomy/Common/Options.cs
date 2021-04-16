// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;

namespace Taxonomy
{
    interface IOptions
    {
        [Value(0, MetaName = "source file path", HelpText = "Source file path to process.", Required = true)]
        string SourceFilePath { get; set; }

        [Value(1, MetaName = "target file path", HelpText = "Target file path to save.", Required = true)]
        string TargetFilePath { get; set; }
    }

    [Verb("generatecwe", false, HelpText = "Generate CWE Sarif file")]
    class CweOptions : IOptions
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
    }

    [Verb("generateowasp", false, HelpText = "Generate OWASP Sarif file")]
    class OwaspOptions : IOptions
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
    }

    [Verb("generatenistsp80053", false, HelpText = "Generate NIST SP800-63B Sarif file")]
    class NistSP80053Options : IOptions
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
    }

    [Verb("generatenistsp80063b", false, HelpText = "Generate NIST SP800-63B Sarif file")]
    class GenerateNistSP80063BOptions
    {
        [Value(0, MetaName = "Source NIST SP800-63B folder path", HelpText = "Source NIST SP800-63B folder path with md files in it to process.", Required = true)]
        public string SourceFolderPath { get; set; }

        [Value(1, MetaName = "target file path", HelpText = "Target file path to save.", Required = true)]
        public string TargetFilePath { get; set; }
    }

    [Verb("addowasprelationshiptocwe", false, HelpText = "Add OWASP relationship To CWE Sarif file")]
    class AddOwaspRelationshipToCweOptions
    {
        [Value(0, MetaName = "Source CWE file path", HelpText = "Source CWE file path to process.", Required = true)]
        public string CweFilePath { get; set; }

        [Value(1, MetaName = "Source OWASP file path", HelpText = "Source OWASP file path to process.", Required = true)]
        public string OwaspFilePath { get; set; }

        [Value(2, MetaName = "Target CWE file path", HelpText = "Target CWE file path to save.", Required = true)]
        public string TargetFilePath { get; set; }
    }
}
