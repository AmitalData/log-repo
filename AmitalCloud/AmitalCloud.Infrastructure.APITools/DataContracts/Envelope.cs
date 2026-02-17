using System.Collections.Generic;
using System.Xml.Serialization;

namespace AmitalCloud.Infrastructure.APITools.DataContracts
{
    public class Envelope
    {

        [XmlAttribute("CommunicationLogId")]
        public string CommunicationLogId { get; set; }

        [XmlAttribute("HasError")]
        public bool HasError { get; set; }
        [XmlAttribute("IsAuthenticationError")]
        public bool IsAuthenticationError { get; set; }

        public string ErrorMessage { get; set; }
        public string InnerErrorMessage { get; set; }

        public List<QueueTask> Tasks { get; set; }


    }

}
