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
        [Theory]
        [InlineData("4.3", "2020-12-10", 1335)]
        [InlineData("4.4", "2021-03-15", 1338)]
        [InlineData("4.5", "2021-07-20", 1343)]
        [InlineData("4.6", "2021-10-28", 1357)]
        [InlineData("4.7", "2022-04-28", 1386)]
        [InlineData("4.8", "2022-06-28", 1389)]
        public void SaveXmlToSarifTests(string version, string expectedReleaseDate, int expectedRuleCount)
        {
            string targetFilePath = $"CWE_v{version}.sarif";
            string sourceFilePath = Path.Combine("TestData", "Cwe", $"cwec_v{version}.xml");
            bool success = new CweTaxonomyGenerator().SaveXmlToSarif(sourceFilePath, targetFilePath, version, "comprehensive");
            success.Should().BeTrue();
            string sarifLogText = File.ReadAllText(targetFilePath);
            sarifLogText.Should().NotBeNullOrWhiteSpace();
            SarifLog? sarifLog = JsonConvert.DeserializeObject<SarifLog>(sarifLogText);
            sarifLog.Should().NotBeNull();
            sarifLog?.Runs.Count.Should().Be(1);
            sarifLog?.Runs[0].Taxonomies.Count.Should().Be(1);
            sarifLog?.Runs[0].Taxonomies[0].Version.Should().Be(version);
            sarifLog?.Runs[0].Taxonomies[0].ReleaseDateUtc.Should().Be(expectedReleaseDate);
            sarifLog?.Runs[0].Taxonomies[0].Name.Should().Be($"CWE V{version}");
            sarifLog?.Runs[0].Taxonomies[0].Taxa.Count.Should().Be(expectedRuleCount);
        }
    }
}
