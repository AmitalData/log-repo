using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DBMigrations.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Constraint", Namespace = "", IsNullable = false)]
    public class ConstraintDefinition
    {
        [XmlAttribute()]
        public bool PrimaryKey { get; set; }

        [XmlAttribute()]
        public bool Nullable { get; set; }
    }
}