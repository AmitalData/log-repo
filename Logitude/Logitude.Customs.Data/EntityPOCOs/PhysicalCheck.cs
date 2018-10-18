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
   
    public class PhysicalCheck
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Declaration")]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
        [ForeignKey("StorageSite")]
        [Column("StorageSiteCode")]
	    public string StorageSiteCode { get; set; }
	      
        public virtual SiteLookup StorageSite { get; set; }
        [ForeignKey("CheckSite")]
        [Column("CheckSiteCode")]
	    public string CheckSiteCode { get; set; }
	      
        public virtual SiteLookup CheckSite { get; set; }
        [ForeignKey("CheckQueueType")]
        [Column("QueueTypeCode")]
	    public string QueueTypeCode { get; set; }
	      
        public virtual CheckQueueType CheckQueueType { get; set; }
        [ForeignKey("Operation")]
        [Column("OperationCode")]
	    public string OperationCode { get; set; }
	      
        public virtual PhysicalCheckOperation Operation { get; set; }
        [Column("CheckId")]
	    public string CheckId { get; set; }
        [Column("ContainerNubmer")]
	    public string ContainerNubmer { get; set; }
        [Column("OpenDate")]
	    public DateTime OpenDate { get; set; }
        [Column("LimitDate")]
	    public DateTime? LimitDate { get; set; }
        [Column("CargoIdentifierKey1")]
	    public string CargoIdentifierKey1 { get; set; }
        [Column("CargoIdentifierKey2")]
	    public string CargoIdentifierKey2 { get; set; }
        [Column("CargoIdentifierKey3")]
	    public string CargoIdentifierKey3 { get; set; }
        [Column("RowNumber")]
	    public string RowNumber { get; set; }
        [Column("CheckEssence")]
	    public string CheckEssence { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("StatusMessage")]
        [Column("StatusMessageCode")]
	    public string StatusMessageCode { get; set; }
	      
        public virtual PhysicalCheckStatusMessage StatusMessage { get; set; }
        [ForeignKey("CheckEntityType")]
        [Column("CargoTypeCode")]
	    public string CargoTypeCode { get; set; }
	      
        public virtual CheckEntityType CheckEntityType { get; set; }
        [ForeignKey("CheckRepresentativeType")]
        [Column("InitiatorTypeCode")]
	    public string InitiatorTypeCode { get; set; }
	      
        public virtual CheckRepresentativeType CheckRepresentativeType { get; set; }
        [Column("ImporterNumber")]
	    public string ImporterNumber { get; set; }
        [ForeignKey("CargoIdentifireType")]
        [Column("CargoIdentifierTypeCode")]
	    public string CargoIdentifierTypeCode { get; set; }
	      
        public virtual CargoIdentifireType CargoIdentifireType { get; set; }
        [Column("ConcurrencyGUID")]
	    public string ConcurrencyGUID { get; set; }
        [Column("IsComprehensiveCheck")]
	    public bool IsComprehensiveCheck { get; set; }
        [ForeignKey("CheckTypeLookup")]
        [Column("CheckTypeCode")]
	    public string CheckTypeCode { get; set; }
	      
        public virtual CheckTypeLookup CheckTypeLookup { get; set; }
        [ForeignKey("CustomerCard")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card CustomerCard { get; set; }
        [Column("NoEscortRequired")]
	    public bool NoEscortRequired { get; set; }
    }
}
	 