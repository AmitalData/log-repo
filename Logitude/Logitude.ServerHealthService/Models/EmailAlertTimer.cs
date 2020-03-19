using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("EmailAlertTimer", Namespace = "", IsNullable = false)]
    public class EmailAlertTimer
    {
        [XmlAttribute("IntervalInSeconds")]
        public long IntervalInSeconds { get; set; }
    }
}