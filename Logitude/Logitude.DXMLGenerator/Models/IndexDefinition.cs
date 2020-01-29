using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DXMLGenerator.Models
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

        [XmlIgnore]
        public bool IncludeSpecified { get { return Include != null; } }
    }
}