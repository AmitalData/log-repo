using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("View", Namespace = "", IsNullable = false)]
    public class ViewDefinition
    {
        [XmlAttribute()]
        public string DBType { get; set; }

        [XmlAttribute()]
        public string Name { get; set; }

        [XmlElement]
        public string SqlScript { get; set; }

        [XmlElement]
        public string OracleScript { get; set; }
    }
}