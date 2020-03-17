using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("To", Namespace = "", IsNullable = false)]
    public class To
    {
        [XmlAttribute("Addresses")]
        public string Addresses { get; set; }
    }
}