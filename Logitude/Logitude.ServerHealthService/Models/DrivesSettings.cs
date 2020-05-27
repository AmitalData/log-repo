using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Drives", Namespace = "", IsNullable = false)]
    public class DrivesSettings
    {
        [XmlElement("Drive")]
        public List<Drive> Drives { get; set; }
    }
}