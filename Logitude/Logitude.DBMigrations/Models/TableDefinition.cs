using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Table", Namespace = "", IsNullable = false)]
    public class TableDefinition
    {
        [XmlAttribute()]
        public string Name { get; set; }

        [XmlAttribute()]
        public string OldName { get; set; }

        [XmlElement("Column")]
        public List<ColumnDefinition> Columns { get; set; }
    }
}