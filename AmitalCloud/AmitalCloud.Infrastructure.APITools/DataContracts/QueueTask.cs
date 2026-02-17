using System.Collections.Generic;
using System.Xml.Serialization;

namespace AmitalCloud.Infrastructure.APITools.DataContracts
{
    public class QueueTask
    {
        [XmlAttribute("action")]
        public string Action { get; set; }

        public List<Parameter> Parameters { get; set; }

    }

}
