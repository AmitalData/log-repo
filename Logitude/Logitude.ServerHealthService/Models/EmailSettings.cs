using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Email", Namespace = "", IsNullable = false)]
    public class EmailSettings
    {
        [XmlElement("SmtpClient")]
        public SmtpClient SmtpClient { get; set; }

        [XmlElement("From")]
        public From From { get; set; }

        [XmlElement("To")]
        public To To { get; set; }
    }
}