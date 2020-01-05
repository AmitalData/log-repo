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
    [XmlRoot("BatchManagerConfig", Namespace = "", IsNullable = false)]
    public class BatchManagerConfigurations
    {
        [XmlArray("Processes")]
        [XmlArrayItem("Process")] 
        public List<BatchProcess> Processes { get; set; }

    }
}
