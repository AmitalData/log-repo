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
   
    public class ConsignmentPackDanger
    {
	 string dbms;

        [Key]
        [ForeignKey("ConsignmentPackage")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual ConsignmentPackage ConsignmentPackage { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("ConsignmentPackage")]
        [Column("ConsignmentNumber" ,Order = 2)]
	    public int? ConsignmentNumber { get; set; }
     [Key]
        [ForeignKey("ConsignmentPackage")]
        [Column("LineNumber" ,Order = 3)]
	    public int? LineNumber { get; set; }
     [Key]
        [Column("DangerousLineNo" ,Order = 4)]
	    public int? DangerousLineNo { get; set; }
        [ForeignKey("HazardousSubstance")]
        [Column("UNCode")]
	    public string UNCode { get; set; }
	      
        public virtual HazardousSubstance HazardousSubstance { get; set; }
        [ForeignKey("DangerousGoodsPackingReq")]
        [Column("DangerousGoodsPackingReqCode")]
	    public string DangerousGoodsPackingReqCode { get; set; }
	      
        public virtual DangerousGoodsPackingReq DangerousGoodsPackingReq { get; set; }
        [Column("FlashpointTemperature")]
	    public string FlashpointTemperature { get; set; }
        [Column("StorageTemperature")]
	    public string StorageTemperature { get; set; }
    }
}
	 