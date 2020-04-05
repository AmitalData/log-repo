using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("From", Namespace = "", IsNullable = false)]
    public class From
    {
        [XmlAttribute("Address")]
        public string Address { get; set; }
    }
}