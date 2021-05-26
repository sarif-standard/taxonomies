// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Tools.NistSP80053
{
    /// <summary>
    /// auto generated class
    /// </summary>
    public class NistSP80053JsonRecord
    {
        public Catalog catalog { get; set; }
    }

    public class Catalog
    {
        public string uuid { get; set; }
        public Metadata metadata { get; set; }
        public Group[] groups { get; set; }
        public BackMatter backmatter { get; set; }
    }

    public class Metadata
    {
        public string title { get; set; }
        public DateTime lastmodified { get; set; }
        public string version { get; set; }
        public string oscalversion { get; set; }
        public Prop[] props { get; set; }
        public Link[] links { get; set; }
        public Role[] roles { get; set; }
        public Party[] parties { get; set; }
        public ResponsibleParties responsibleparties { get; set; }
    }

    public class ResponsibleParties
    {
        public Creator creator { get; set; }
        public Contact contact { get; set; }
    }

    public class Creator
    {
        public string[] partyuuids { get; set; }
    }

    public class Contact
    {
        public string[] partyuuids { get; set; }
    }

    public class Prop
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

    public class Role
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    public class Party
    {
        public string uuid { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string[] emailaddresses { get; set; }
        public Address[] addresses { get; set; }
    }

    public class Address
    {
        public string[] addrlines { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalcode { get; set; }
    }

    public class BackMatter
    {
        public Resource[] resources { get; set; }
    }

    public class Resource
    {
        public string uuid { get; set; }
        public string title { get; set; }
        public Citation citation { get; set; }
        public Rlink[] rlinks { get; set; }
        public DocumentIds[] documentids { get; set; }
    }

    public class Citation
    {
        public string text { get; set; }
    }

    public class Rlink
    {
        public string href { get; set; }
        public string mediatype { get; set; }
    }

    public class DocumentIds
    {
        public string scheme { get; set; }
        public string identifier { get; set; }
    }

    public class Group
    {
        public string id { get; set; }
        public string _class { get; set; }
        public string title { get; set; }
        public Control[] controls { get; set; }
    }

    public class Control
    {
        public string id { get; set; }
        public string @class { get; set; }
        public string title { get; set; }
        public Param[] @params { get; set; }
        public Prop1[] props { get; set; }
        public Link1[] links { get; set; }
        public Part[] parts { get; set; }
        public Control1[] controls { get; set; }
    }

    public class Param
    {
        public string id { get; set; }
        public string label { get; set; }
        public Select select { get; set; }
        public string dependson { get; set; }
    }

    public class Select
    {
        public string[] choice { get; set; }
        public string howmany { get; set; }
    }

    public class Prop1
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Link1
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

    public class Part
    {
        public string id { get; set; }
        public string name { get; set; }
        public string prose { get; set; }
        public Part1[] parts { get; set; }
        public Link2[] links { get; set; }
        public Prop7[] props { get; set; }
    }

    public class Part1
    {
        public string id { get; set; }
        public string name { get; set; }
        public Prop2[] props { get; set; }
        public string prose { get; set; }
        public Part2[] parts { get; set; }
    }

    public class Prop2
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Part2
    {
        public string id { get; set; }
        public string name { get; set; }
        public Prop3[] props { get; set; }
        public string prose { get; set; }
        public Part3[] parts { get; set; }
    }

    public class Prop3
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Part3
    {
        public string id { get; set; }
        public string name { get; set; }
        public Prop4[] props { get; set; }
        public string prose { get; set; }
        public Part4[] parts { get; set; }
    }

    public class Prop4
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Part4
    {
        public string id { get; set; }
        public string name { get; set; }
        public Prop5[] props { get; set; }
        public string prose { get; set; }
        public Part5[] parts { get; set; }
    }

    public class Prop5
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Part5
    {
        public string id { get; set; }
        public string name { get; set; }
        public Prop6[] props { get; set; }
        public string prose { get; set; }
    }

    public class Prop6
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Link2
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

    public class Prop7
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Control1
    {
        public string id { get; set; }
        public string _class { get; set; }
        public string title { get; set; }
        public Prop8[] props { get; set; }
        public Part6[] parts { get; set; }
        public Param1[] _params { get; set; }
        public Link7[] links { get; set; }
    }

    public class Prop8
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Part6
    {
        public string id { get; set; }
        public string name { get; set; }
        public string prose { get; set; }
        public Prop9[] props { get; set; }
        public Part7[] parts { get; set; }
        public Link6[] links { get; set; }
    }

    public class Prop9
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Part7
    {
        public string name { get; set; }
        public string prose { get; set; }
        public string id { get; set; }
        public Prop10[] props { get; set; }
        public Part8[] parts { get; set; }
        public Link5[] links { get; set; }
    }

    public class Prop10
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Part8
    {
        public string id { get; set; }
        public string name { get; set; }
        public Prop11[] props { get; set; }
        public string prose { get; set; }
        public Link3[] links { get; set; }
        public Part9[] parts { get; set; }
    }

    public class Prop11
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Link3
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

    public class Part9
    {
        public string id { get; set; }
        public string name { get; set; }
        public Prop12[] props { get; set; }
        public string prose { get; set; }
        public Link4[] links { get; set; }
        public Part10[] parts { get; set; }
    }

    public class Prop12
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Link4
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

    public class Part10
    {
        public string id { get; set; }
        public string name { get; set; }
        public Prop13[] props { get; set; }
        public string prose { get; set; }
    }

    public class Prop13
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Link5
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

    public class Link6
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

    public class Param1
    {
        public string id { get; set; }
        public Select1 select { get; set; }
        public string label { get; set; }
        public string dependson { get; set; }
    }

    public class Select1
    {
        public string[] choice { get; set; }
        public string howmany { get; set; }
    }

    public class Link7
    {
        public string href { get; set; }
        public string rel { get; set; }
    }
}
