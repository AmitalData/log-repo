using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Simplog.Data.Helpers
{

    [DataContract(Namespace = "")]
    public class AutomationOnUpdateDocument
    {
        [DataMember]
        public string SendVia { get; set; }
        [DataMember]
        public FTPAutomationDetails FTPDetails { get; set; }
        [DataMember]
        public string ComputingPartnerId { get; set; }
        [DataMember]
        public List<OnUpdateDocumentTypeAttachment> DocumentTypeLists { get; set; }
    }

    [DataContract(Namespace = "")]
    public class OnUpdateDocumentTypeAttachment
    {
        [DataMember]
        public string DocumentTypeId { get; set; }
        [DataMember]
        public string DocumentTypeName { get; set; }
        [DataMember]
        public string DocumentTypeCopyId { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}
