using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class ExportStorge
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
        [Column("ExportFileNo")]
	    public string ExportFileNo { get; set; }
        [Column("OrderNo")]
	    public string OrderNo { get; set; }
        [Column("CustomFileNo")]
	    public string CustomFileNo { get; set; }
        [ForeignKey("FclLclCodeTable")]
        [Column("FclLcl")]
	    public string FclLcl { get; set; }
	      
        public virtual FclLclCode FclLclCodeTable { get; set; }
        [Column("Direction")]
	    public string Direction { get; set; }
        [ForeignKey("CustomsTransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual CustomsTransportMode CustomsTransportMode { get; set; }
        [Column("StorageNo")]
	    public int StorageNo { get; set; }
        [Column("VoyageNo")]
	    public double VoyageNo { get; set; }
        [Column("StorageDate")]
	    public DateTime StorageDate { get; set; }
        [ForeignKey("StorageStatus")]
        [Column("StorageStatus")]
	    public string StorageStatus { get; set; }
	      
        public virtual StorageStatus StorageStatus { get; set; }
        [Column("IsOpenStoarge")]
	    public bool IsOpenStoarge { get; set; }
        [Column("IsConnectedToDeclaration")]
	    public bool IsConnectedToDeclaration { get; set; }
        [ForeignKey("NDMessageActionCode")]
        [Column("OperationCode")]
	    public string OperationCode { get; set; }
	      
        public virtual NDMessageActionCode NDMessageActionCode { get; set; }
        [ForeignKey("ExportDeliveryDocumentMessage")]
        [Column("SenderCodeID")]
	    public string SenderCodeID { get; set; }
	      
        public virtual ExportDeliveryDocumentMessage ExportDeliveryDocumentMessage { get; set; }
        [Column("MessageFromForm")]
	    public int MessageFromForm { get; set; }
        [Column("ReplyPhoneNumeric")]
	    public double ReplyPhoneNumeric { get; set; }
        [Column("OperatorID")]
	    public double OperatorID { get; set; }
        [ForeignKey("ExportDeliveryDocumentMessage")]
        [Column("InformedParty")]
	    public string InformedParty { get; set; }
        [Column("DeclarationNumber")]
	    public string DeclarationNumber { get; set; }
        [Column("DeclarationsInContainer")]
	    public int DeclarationsInContainer { get; set; }
        [Column("ExportManifestNumber")]
	    public int ExportManifestNumber { get; set; }
        [ForeignKey("InternationalSite")]
        [Column("ReceivingSite")]
	    public string ReceivingSite { get; set; }
	      
        public virtual InternationalSite InternationalSite { get; set; }
        [ForeignKey("StuffingSiteType")]
        [Column("StuffingSiteType")]
	    public string StuffingSiteType { get; set; }
	      
        public virtual StuffingSiteType StuffingSiteType { get; set; }
        [ForeignKey("LoadingSiteType")]
        [Column("LoadingSite")]
	    public string LoadingSite { get; set; }
	      
        public virtual LoadingSiteType LoadingSiteType { get; set; }
    }
}
	 