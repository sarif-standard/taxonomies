// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CsvHelper.Configuration.Attributes;

namespace Taxonomy
{
    public class CweCsvRecord
    {
        [Name("CWE-ID")]
        public int CweId { get; set; }

        public string Name { get; set; }

        [Name("Weakness Abstraction")]
        public string WeaknessAbstraction { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        [Name("Extended Description")]
        public string ExtendedDescription { get; set; }

        [Name("Related Weaknesses")]
        public string RelatedWeaknesses { get; set; }

        [Name("Weakness Ordinalities")]
        public string WeaknessOrdinalities { get; set; }

        [Name("Applicable Platforms")]
        public string ApplicablePlatforms { get; set; }

        [Name("Background Details")]
        public string BackgroundDetails { get; set; }

        [Name("Alternate Terms")]
        public string AlternateTerms { get; set; }

        [Name("Modes Of Introduction")]
        public string ModesOfIntroduction { get; set; }

        [Name("Exploitation Factors")]
        public string ExploitationFactors { get; set; }

        [Name("Likelihood of Exploit")]
        public string LikelihoodofExploit { get; set; }

        [Name("Common Consequences")]
        public string CommonConsequences { get; set; }

        [Name("Detection Methods")]
        public string DetectionMethods { get; set; }

        [Name("Potential Mitigations")]
        public string PotentialMitigations { get; set; }

        [Name("Observed Examples")]
        public string ObservedExamples { get; set; }

        [Name("Functional Areas")]
        public string FunctionalAreas { get; set; }

        [Name("Affected Resources")]
        public string AffectedResources { get; set; }

        [Name("Taxonomy Mappings")]
        public string TaxonomyMappings { get; set; }

        [Name("Related Attack Patterns")]
        public string RelatedAttackPatterns { get; set; }

        public string Notes { get; set; }
    }
}
