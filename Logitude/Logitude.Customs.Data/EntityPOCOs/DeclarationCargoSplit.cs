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
   
    public class DeclarationCargoSplit
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("RequestDate")]
	    public DateTime RequestDate { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("ActionCode")]
        [Column("ActionTypeCode")]
	    public string ActionTypeCode { get; set; }
	      
        public virtual ActionCode ActionCode { get; set; }
        [ForeignKey("SplitOrMergeReason")]
        [Column("RequestReason")]
	    public string RequestReason { get; set; }
	      
        public virtual SplitOrMergeReason SplitOrMergeReason { get; set; }
        [Column("RequestNumber")]
	    public string RequestNumber { get; set; }
        [Column("RequestRemarks")]
	    public string RequestRemarks { get; set; }
        [ForeignKey("CargoIdentifireType")]
        [Column("CargoTypeCode")]
	    public string CargoTypeCode { get; set; }
	      
        public virtual CargoIdentifireType CargoIdentifireType { get; set; }
        [Column("ManifestNumber")]
	    public string ManifestNumber { get; set; }
        [Column("SecondCargoID")]
	    public string SecondCargoID { get; set; }
        [Column("ThirdCargoID")]
	    public string ThirdCargoID { get; set; }
        [ForeignKey("Declaration")]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
        [ForeignKey("CargoSplitRequestStatus")]
        [Column("ResponseStatusCode")]
	    public string ResponseStatusCode { get; set; }
	      
        public virtual CargoSplitRequestStatus CargoSplitRequestStatus { get; set; }
    }
}
	 