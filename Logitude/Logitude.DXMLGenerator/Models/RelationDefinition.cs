using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DXMLGenerator.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Relation", Namespace = "", IsNullable = false)]
    public class RelationDefinition
    {
        [XmlAttribute()]
        public string ParentTableName { get; set; }

        [XmlAttribute()]
        public string ParentColumnName { get; set; }

        [XmlAttribute()]
        public string ReferencedTableName { get; set; }

        [XmlAttribute()]
        public string ReferencedColumnName { get; set; }

        [XmlAttribute()]
        public string ForeignKeyConstraintName { get; set; }
    }
}
