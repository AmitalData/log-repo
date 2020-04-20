using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Drive", Namespace = "", IsNullable = false)]
    public class Drive
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("MinimumFreeSpacePercent")]
        public double MinimumFreeSpacePercent { get; set; }

        [XmlAttribute("MinimumFreeSpaceGB")]
        public double MinimumFreeSpaceGB { get; set; }
    }
}