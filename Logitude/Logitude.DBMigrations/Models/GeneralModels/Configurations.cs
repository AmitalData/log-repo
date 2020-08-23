using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Configurations", Namespace = "", IsNullable = false)]
    public class Configurations
    {
        [XmlElement("Config")]
        public List<Config> Configs { get; set; }
    }
}