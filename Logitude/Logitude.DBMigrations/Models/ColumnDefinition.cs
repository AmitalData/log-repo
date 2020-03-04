using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Column", Namespace = "", IsNullable = false)]
    public class ColumnDefinition
    {
        [XmlAttribute()]
        public string Name { get; set; }

        [XmlAttribute()]
        public string ShortName { get; set; }

        [XmlAttribute()]
        public string OldNames { get; set; }

        [XmlAttribute()]
        public string Type { get; set; }

        [XmlAttribute()]
        public int Size { get; set; }

        [XmlAttribute()]
        public int Precision { get; set; }

        [XmlAttribute()]
        public int Scale { get; set; }

        [XmlAttribute()]
        public bool Identity { get; set; }

        [XmlAttribute()]
        public string DefaultValue { get; set; }

        [XmlElement]
        public ConstraintsDefinition Constraints { get; set; }
    }
}