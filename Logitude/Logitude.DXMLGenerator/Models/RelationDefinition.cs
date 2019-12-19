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
        public string ForeignKeyColumn { get; set; }//ParentColumnName
        
        [XmlAttribute()]
        public string ReferencedTable { get; set; }//ReferencedTableName

        [XmlAttribute()]
        public string ReferencedColumn { get; set; }//ReferencedColumnName

        [XmlAttribute()]
        public string ForeignKeyConstraintName { get; set; }
    }
}