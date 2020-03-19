using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Logitude.ServerHealthService.Models
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("SmtpClient", Namespace = "", IsNullable = false)]
    public class SmtpClient
    {
        [XmlAttribute("Host")]
        public string Host { get; set; }

        [XmlAttribute("Port")]
        public int Port { get; set; }

        [XmlAttribute("Username")]
        public string Username { get; set; }

        [XmlAttribute("Password")]
        public string Password { get; set; }
    }
}