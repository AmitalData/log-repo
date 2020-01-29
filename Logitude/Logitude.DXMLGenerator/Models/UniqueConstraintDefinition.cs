using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DXMLGenerator.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("UniqueConstraint", Namespace = "", IsNullable = false)]
    public class UniqueConstraintDefinition
    {
        [XmlAttribute()]
        public string Columns { get; set; }
    }
}