using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.LXMLFixer.Models
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

        [XmlAttribute()]
        public int Precision { get; set; }

        [XmlIgnore]
        public bool PrecisionSpecified { get { return Type == "decimal"; } }

        [XmlAttribute()]
        public int Scale { get; set; }

        [XmlIgnore]
        public bool ScaleSpecified { get { return Type == "decimal"; } }

        [XmlElement]
        public ConstraintsDefinition Constraints { get; set; }
    }
}