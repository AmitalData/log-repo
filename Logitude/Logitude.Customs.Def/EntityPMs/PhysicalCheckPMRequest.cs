using System;

namespace Logitude.Customs.Def.EntityPMs
{

    public partial class PhysicalCheckPMRequest
    {
        //public .Common.AgentPaymentRequestServiceReference.RequestContentHeader requestContentHeader { get; set; }
        public NoticeToClient noticeToClient { get; set; }
      //  public  checkEntity { get; set; }
        public string checkInstruction { get; set; }
        public string splitCargoIdentifier { get; set; }
    }
    /*
    public class CheckEntity
    {
        public CargoIdentifier cargoIdentifier { get; set; }

    }
    public class CargoIdentifier
    {
       public int cargoIdentifierType { get; set; }
       public string   cargoIdentifierKey1 { get; set; }
       public string cargoIdentifierKey2 { get; set; }
       public string  cargoIdentifierKey3 { get; set; }
    }
    */
    public class NoticeToClient
    {
        public int operationCode { get; set; }
        public int statusMessage { get; set; }
        public int checkId { get; set; }
        public int entityType { get; set; }
        public bool initiatorTypeSpecified { get; set; }
        public int customsAgent { get; set; }
        public bool customsAgentSpecified { get; set; }
        public int importerNumber { get; set; }
        public bool importerNumberSpecified { get; set; }
        public string storageSiteNumber { get; set; }
        public string checkSiteNumber { get; set; }
        public DateTime openDate { get; set; }
        public bool limitDateSpecified  { get; set; }
        public int CheckType { get; set; }
        public bool QueueTypeSpecified { get; set; }
        public string declarationID { get; set; }
        public bool BackToPortIndication { get; set; }
        public bool BackToPortIndicationSpecified { get; set; }
        public bool IsComprehensiveCheckSpecified { get; set; }
        public bool IsSecurityCheckOnImportBaldarCargoSpecified { get; set; }
        public string VehicleChassisNumber { get; set; }



    }
    /*
   public class RequestContentHeader
    {
        public DateTime TransmitionDateTime { get; set; }
        public int SenderId { get; set; }
        public string RecieverID { get; set; }
        public string Convertor { get; set; }

    }*/
}
