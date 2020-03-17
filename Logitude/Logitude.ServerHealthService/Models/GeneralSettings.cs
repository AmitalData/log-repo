using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("General", Namespace = "", IsNullable = false)]
    public class GeneralSettings
    {
        [XmlElement("DrivesCheckTimer")]
        public DrivesCheckTimer DrivesCheckTimer { get; set; }

        [XmlElement("EmailAlertTimer")]
        public EmailAlertTimer EmailAlertTimer { get; set; }
    }
}