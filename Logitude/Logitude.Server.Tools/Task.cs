using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Server.Tools
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
        public string SystemId { get; set; }



    }

    public class QueueTask
    {
        [XmlAttribute("action")]
        public string Action { get; set; }

        public List<Parameter> Parameters { get; set; }

    }


    public class Parameter
    {
        [XmlAttribute("order")]
        public int Order { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}




//                <tasks>
//<task action="customer.readyforactivation">
//         <parameter order="1">
//     <customerPM/>
// </parameter>
//</task>

//<task>
//<AddressPM/>
//</task >
//</tasks>
