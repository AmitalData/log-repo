using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DeploymentAgentService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("CurrentPackage", Namespace = "", IsNullable = false)]
    public class CurrentPackage
    {
        [XmlAttribute("Version")]
        public string Version { get; set; }

        [XmlAttribute("Url")]
        public string Url { get; set; }
    }
}