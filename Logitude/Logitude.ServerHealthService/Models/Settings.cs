using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Settings", Namespace = "", IsNullable = false)]
    public class Settings
    {
        [XmlElement("Drives")]
        public DrivesSettings DrivesSettings { get; set; }

        [XmlElement("Email")]
        public EmailSettings EmailSettings { get; set; }

        [XmlElement("General")]
        public GeneralSettings GeneralSettings { get; set; }
    }
}