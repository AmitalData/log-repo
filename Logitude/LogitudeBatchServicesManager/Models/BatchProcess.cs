using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogitudeBatchServicesManager.Models
{
    [Serializable()]
    //[DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("Process", Namespace = "", IsNullable = false)]
    public class BatchProcess
    {
        [XmlAttribute()]
        public string Name { get; set; }
        [XmlAttribute()]
        public bool AllServices { get; set; }
        [XmlAttribute()]
        public string Ignore { get; set; }
        [XmlAttribute()]
        public int MaxMemoryMB { get; set; }
        [XmlAttribute()]
        public string RestartTime { get; set; }
        [XmlAttribute()]
        public int MaxWorkingTimeInMinutes { get; set; }

        [XmlArray("Services")]
        [XmlArrayItem("Service")]
        public List<BatchService> Services { get; set; }
    }
}
