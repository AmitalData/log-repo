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
    [XmlRoot("Service", Namespace = "", IsNullable = false)]
    public class BatchService
    {
        [XmlAttribute()]
        public string Code { get; set; }
        [XmlAttribute()]
        public int MaxWorkingTimeInMinutes { get; set; }
    }
}
