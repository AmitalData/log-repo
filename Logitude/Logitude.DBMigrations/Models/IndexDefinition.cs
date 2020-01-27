using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Index", Namespace = "", IsNullable = false)]
    public class IndexDefinition
    {
        [XmlAttribute()]
        public string Columns { get; set; }

        [XmlAttribute()]
        public string Include { get; set; }

        [XmlAttribute()]
        public string IndexName { get; set; }

        [XmlAttribute()]
        public int KeyOrder { get; set; }
    }
}