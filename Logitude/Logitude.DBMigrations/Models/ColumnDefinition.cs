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
        public string Type { get; set; }

        [XmlAttribute()]
        public string Size { get; set; }

        [XmlElement("Constraint")]
        public List<ConstraintDefinition> Constraints { get; set; }
    }
}