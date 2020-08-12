using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Config", Namespace = "", IsNullable = false)]
    public class Config
    {
        [XmlAttribute()]
        public string Name { get; set; }

        [XmlAttribute()]
        public string Value { get; set; }
    }
}