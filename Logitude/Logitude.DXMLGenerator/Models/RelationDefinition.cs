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
        public string ForeignKeyColumn { get; set; }
        
        [XmlAttribute()]
        public string ReferencedTable { get; set; }

        [XmlAttribute()]
        public string ReferencedColumn { get; set; }

        [XmlAttribute()]
        public int ReferencedColumnOrder { get; set; }

        [XmlIgnore]
        public bool ReferencedColumnOrderSpecified { get { return ReferencedColumnOrder > 0; } }

        [XmlAttribute()]
        public string ReferencedTableSchema { get; set; }

        [XmlAttribute()]
        public string ForeignKeyConstraintName { get; set; }
    }
}