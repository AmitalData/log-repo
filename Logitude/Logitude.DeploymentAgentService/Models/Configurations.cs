using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.DeploymentAgentService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Configurations", Namespace = "", IsNullable = false)]
    public class Configurations
    {
        [XmlElement("CurrentPackage")]
        public CurrentPackage CurrentPackage { get; set; }
    }
}