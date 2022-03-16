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
   
    public class LogisticActionRequest
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("RequestDate")]
	    public DateTime RequestDate { get; set; }
        [Column("ExportFileNo")]
	    public string ExportFileNo { get; set; }
        [Column("ExporterIdentifierType")]
	    public string ExporterIdentifierType { get; set; }
        [Column("ExporterNumber")]
	    public string ExporterNumber { get; set; }
        [Column("PassportCountry")]
	    public string PassportCountry { get; set; }
        [Column("PassportNumber")]
	    public string PassportNumber { get; set; }
        [ForeignKey("LogisticActionRequestType")]
        [Column("RequestType")]
	    public string RequestType { get; set; }
	      
        public virtual LogisticActionRequestType LogisticActionRequestType { get; set; }
        [Column("RequestReason")]
	    public string RequestReason { get; set; }
        [ForeignKey("SiteLookup")]
        [Column("DeliverySiteID")]
	    public string DeliverySiteID { get; set; }
	      
        public virtual SiteLookup SiteLookup { get; set; }
        [ForeignKey("CargoIdentifireType")]
        [Column("CargoIdentifierType")]
	    public string CargoIdentifierType { get; set; }
	      
        public virtual CargoIdentifireType CargoIdentifireType { get; set; }
        [Column("CargoIdentifierKey1")]
	    public string CargoIdentifierKey1 { get; set; }
        [Column("CargoIdentifierKey2")]
	    public string CargoIdentifierKey2 { get; set; }
        [Column("CargoIdentifierKey3")]
	    public string CargoIdentifierKey3 { get; set; }
        [ForeignKey("PackingType")]
        [Column("PackagingTypeCode")]
	    public string PackagingTypeCode { get; set; }
	      
        public virtual PackingType PackingType { get; set; }
        [Column("Quantity")]
	    public decimal Quantity { get; set; }
        [Column("RequestNumber")]
	    public string RequestNumber { get; set; }
        [ForeignKey("LogisticActionResponseReqS")]
        [Column("ResponseStatusCode")]
	    public string ResponseStatusCode { get; set; }
	      
        public virtual LogisticActionResponseReqS LogisticActionResponseReqS { get; set; }
        [Column("OperationalStatus")]
	    public string OperationalStatus { get; set; }
        [Column("Direction")]
	    public string Direction { get; set; }
        [Column("TransportmodeId")]
	    public string TransportmodeId { get; set; }
        [Column("DecisionRmarks")]
	    public string DecisionRmarks { get; set; }
        [Column("CustomsUserName")]
	    public string CustomsUserName { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
    }
}
	 