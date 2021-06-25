// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Taxonomy.Cwe
{
    public static class Extensions
    {
        public static string ToSarifRelationship(this RelatedNatureEnumeration nature)
        {
            switch (nature)
            {
                case RelatedNatureEnumeration.ChildOf:
                    return "superset";
                case RelatedNatureEnumeration.PeerOf:
                    return "relevant";
                case RelatedNatureEnumeration.CanPrecede:
                    return "canFollow";
                case RelatedNatureEnumeration.CanAlsoBe:
                    return "equal";
                case RelatedNatureEnumeration.Requires:
                    return "willPrecede";
                case RelatedNatureEnumeration.StartsWith:
                    return "willPrecede";
                default:
                    throw new Exception("relationship not found");
            }
        }

        public static string ToDescription(this StructuredTextType node)
        {
            return node == null || node.Any == null || node.Any.Length == 0
                ? null
                : string.Join("", node.Any.Select(n => n.ToDescription()).ToList());
        }

        public static string ToDescription(this XmlNode node)
        {
            if (node.Value != null)
            {
                return node.Value;
            }

            XmlNodeList allNodes = node.SelectNodes("descendant::node()");
            var list = new List<string>();
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i].Value != null)
                {
                    if (!string.IsNullOrWhiteSpace(allNodes[i].Value.Replace(@"\r", "").Replace(@"\n", "").Replace(@"\t", "")))
                    {
                        list.Add(allNodes[i].Value);
                    }
                }
            }

            return string.Join("", list);
        }
    }
}
