using System;
namespace Logitude.Customs.Data.DataContracts.SIIRequest
{

    public class SiiStatusEnvelope
    {
        public string sender { get; set; }
        public string messageType { get; set; }
        public string messageReference { get; set; }
        public DateTime dateTime { get; set; }
        public string customer { get; set; }
        public SiiStatusMessage message { get; set; }
    }

    public class SiiStatusMessage
    {
        public string requestNumber { get; set; }
        public string importerNumber { get; set; }
        public DateTime receptionDate { get; set; }
        public string customsApprovalNumber { get; set; }
        public int lineSerialNumber { get; set; }
        public string modelCode { get; set; }
        public string saleApprovalStatus { get; set; }
        public DateTime? saleApprovalStatusDate { get; set; }
        public string customsAgentRegisteredNumber { get; set; }
        public string distributionApprovalAttachmentPath { get; set; }
    }

}