using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.LXMLFixer.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Constraints", Namespace = "", IsNullable = false)]
    public class ConstraintsDefinition
    {
        [XmlAttribute]
        public bool PrimaryKey { get; set; }

        [XmlIgnore]
        public bool PrimaryKeySpecified { get { return PrimaryKey; } }


        [XmlAttribute()]
        public bool Nullable { get; set; }

    }
}