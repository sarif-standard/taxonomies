// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Taxonomy
{
    public class CweRelationship
    {
        // NATURE:ChildOf:CWE ID:707:VIEW ID:1000:ORDINAL:Primary::
        public string Nature { get; set; }
        public string CweId { get; set; }

        public string Kinds
        {
            get
            {
                switch (this.Nature)
                {
                    case "ChildOf":
                        return "superset";
                    case "PeerOf":
                        return "relevant";
                    case "CanPrecede":
                        return "canFollow";
                    case "CanAlsoBe":
                        return "equal";
                    case "Requires":
                        return "willPrecede";
                    case "StartsWith":
                        return "willPrecede";
                    default:
                        return $"NotIdentified-{this.Nature}";
                }
            }
        }

        public CweRelationship(string relString)
        {
            string[] terms = relString.Split(":", StringSplitOptions.TrimEntries);
            if (terms[2] != "CWE ID")
            {
                throw new ArgumentException();
            }
            this.Nature = terms[1];
            this.CweId = $"CWE-{terms[3]}";
        }

        public override string ToString()
        {
            return $"{this.Nature}:{this.CweId}";
        }
    }
}
