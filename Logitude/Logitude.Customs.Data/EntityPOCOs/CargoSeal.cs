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
   
    public class CargoSeal
    {
	 string dbms;

        [Key]
        [ForeignKey("CargoSealIdentifier")]
        [Column("CargoSealIdentifierId" ,Order = 1)]
	    public string CargoSealIdentifierId { get; set; }
	      
        public virtual CargoSealIdentifier CargoSealIdentifier { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("SealNumber" ,Order = 2)]
	    public string SealNumber { get; set; }
        [Column("Remarks")]
	    public string Remarks { get; set; }
        [ForeignKey("SealCompletenes")]
        [Column("SealCompletenessStateCode")]
	    public string SealCompletenessStateCode { get; set; }
	      
        public virtual SealCompletenes SealCompletenes { get; set; }
        [ForeignKey("SealType")]
        [Column("SealTypeCode")]
	    public string SealTypeCode { get; set; }
	      
        public virtual SealType SealType { get; set; }
        [ForeignKey("SealUpdateReasonType")]
        [Column("UpdateReasonCode")]
	    public string UpdateReasonCode { get; set; }
	      
        public virtual SealUpdateReasonType SealUpdateReasonType { get; set; }
        [ForeignKey("AmendmentType")]
        [Column("UpdateTypeCode")]
	    public string UpdateTypeCode { get; set; }
	      
        public virtual AmendmentType AmendmentType { get; set; }
    }
}
	 