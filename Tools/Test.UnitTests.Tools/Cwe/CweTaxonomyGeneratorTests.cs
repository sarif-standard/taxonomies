// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using Taxonomy.Cwe;

using Xunit;

namespace Test.UnitTests.Tools.Cwe
{
    public class CweTaxonomyGeneratorTests
    {
        [Fact]
        public void SaveXmlToSarifTests()
        {
            (string version, string expectedReleaseDate, int expectedRuleCount)[] testCases = new[]
            {
                ("4.3", "2020-12-10", 1335),
                ("4.4", "2021-03-15", 1338),
                ("4.5", "2021-07-20", 1343),
                ("4.6", "2021-10-28", 1357),
                ("4.7", "2022-04-28", 1386),
                ("4.8", "2022-06-28", 1389)
            };

            foreach ((string version, string expectedReleaseDate, int expectedRuleCount) testCase in testCases)
            {
                string targetFilePath = $"CWE_v{testCase.version}.sarif";
                string sourceFilePath = Path.Combine("TestData", "Cwe", $"cwec_v{testCase.version}.xml");
                bool success = new CweTaxonomyGenerator().SaveXmlToSarif(sourceFilePath, targetFilePath, testCase.version, "comprehensive");
                success.Should().BeTrue();
                string sarifLogText = File.ReadAllText(targetFilePath);
                sarifLogText.Should().NotBeNullOrWhiteSpace();
                SarifLog? sarifLog = JsonConvert.DeserializeObject<SarifLog>(sarifLogText);
                sarifLog.Should().NotBeNull();
                sarifLog?.Runs.Count.Should().Be(1);
                sarifLog?.Runs[0].Taxonomies.Count.Should().Be(1);
                sarifLog?.Runs[0].Taxonomies[0].Version.Should().Be(testCase.version);
                sarifLog?.Runs[0].Taxonomies[0].ReleaseDateUtc.Should().Be(testCase.expectedReleaseDate);
                sarifLog?.Runs[0].Taxonomies[0].Name.Should().Be($"CWE V{testCase.version}");
                sarifLog?.Runs[0].Taxonomies[0].Taxa.Count.Should().Be(testCase.expectedRuleCount);
            }
        }
    }
}
