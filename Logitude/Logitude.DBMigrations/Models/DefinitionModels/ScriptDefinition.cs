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
        public string SxmlFileName { get; set; }

        public string ScriptHistoryAction { get; set; }
        
        [XmlAttribute()]
        public string DBType { get; set; }

        [XmlAttribute()]
        public string Module { get; set; }

        [XmlAttribute()]
        public bool Pre { get; set; }

        [XmlAttribute()]
        public bool AOT { get; set; }

        [XmlAttribute()]
        public string TargetTableName { get; set; }

        [XmlAttribute()]
        public int BatchSize { get; set; }

        [XmlElement]
        public SqlScriptDefinition Sql { get; set; }

        [XmlElement]
        public OracleScriptDefinition Oracle { get; set; }

        [XmlAttribute()]
        public string Env { get; set; }
    }
}