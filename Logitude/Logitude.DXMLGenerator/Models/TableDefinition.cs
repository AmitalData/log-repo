using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DXMLGenerator.Models
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
        public string Schema { get; set; }

        [XmlAttribute()]
        public string DBType { get; set; }

        [XmlElement("Column")]
        public List<ColumnDefinition> Columns { get; set; }

        [XmlElement("Relation")]
        public List<RelationDefinition> Relations { get; set; }

        [XmlElement("Index")]
        public List<IndexDefinition> Indexes { get; set; }

        [XmlElement("UniqueConstraint")]
        public List<UniqueConstraintDefinition> UniqueConstraints { get; set; }
    }
}