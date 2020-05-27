using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Sql", Namespace = "", IsNullable = false)]
    public class SqlScriptDefinition
    {
        [XmlText]
        public string Script { get; set; }
        
        [XmlAttribute()]
        public int Version { get; set; }
    }
}