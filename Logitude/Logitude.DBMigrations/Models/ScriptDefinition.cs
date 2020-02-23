using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Script", Namespace = "", IsNullable = false)]
    public class ScriptDefinition
    {
        [XmlAttribute()]
        public string DBType { get; set; }

        [XmlElement]
        public SqlScriptDefinition Sql { get; set; }

        [XmlElement]
        public OracleScriptDefinition Oracle { get; set; }
    }
}