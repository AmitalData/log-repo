using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DXMLGenerator.Models
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
        public string Type { get; set; }

        [XmlAttribute()]
        public int Size { get; set; }

        [XmlIgnore]
        public bool SizeSpecified { get { return Size != 0; } }

        [XmlElement]
        public ConstraintsDefinition Constraints { get; set; }
    }
}