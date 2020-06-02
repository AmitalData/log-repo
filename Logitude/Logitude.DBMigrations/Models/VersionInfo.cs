using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("VersionInfo", Namespace = "", IsNullable = false)]
    public class VersionInfo
    {
        [XmlAttribute()]
        public string Version { get; set; }
    }
}