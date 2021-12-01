using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
   
        [DataContract(Namespace = "")]
        public class AutomationSendInterface
        {

            [DataMember]
            public string InterfaceName { get; set; }

            [DataMember]
            public string SendVia { get; set; }


            [DataMember]
            public string Format { get; set; }

            [DataMember]
            public string ComputingPartnerId { get; set; }

            [DataMember]
            public FTPAutomationDetails FTPDetails { get; set; }

            [DataMember]
            public WebHookAutomationDetails WebHookDetails { get; set; }

            [DataMember]
            public AdvancedAutomationSendInterfaceDetails AdvancedAutomationSendInterfaceDetails { get; set; }
    }


        [DataContract(Namespace = "")]
        public class FTPAutomationDetails
        {
            [DataMember]
            public string Host { get; set; }
            [DataMember]
            public string Folder { get; set; }
            [DataMember]
            public string UserName { get; set; }
            [DataMember]
            public string Password { get; set; }

        }

        [DataContract(Namespace = "")]
        public class WebHookAutomationDetails
        {
            [DataMember]
            public string URL { get; set; }

        }

        [DataContract(Namespace = "")]
        public class AdvancedAutomationSendInterfaceDetails
        {
            [DataMember]
            public bool IncludeEvents { get; set; }
        }
}
