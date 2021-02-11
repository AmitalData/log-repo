using System.Runtime.Serialization;

namespace Simplog.Data.Helpers
{
   
        [DataContract(Namespace = "")]
        public class AutomationSendDocument
        {
            [DataMember]
            public string SendVia { get; set; }
            [DataMember]
            public FTPAutomationDetails FTPDetails { get; set; }
        }
}
