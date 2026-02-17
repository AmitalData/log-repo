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
   
    public class ClaimImporterDeclarsPage3A
    {
	 string dbms;

        [Key]
        [ForeignKey("Claim")]
        [Column("ClaimId" ,Order = 1)]
	    public string ClaimId { get; set; }
	      
        public virtual Claim Claim { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("LineNo" ,Order = 2)]
	    public int LineNo { get; set; }
        [ForeignKey("CommercialSale")]
        [Column("CommercialSaleTypeCode")]
	    public string CommercialSaleTypeCode { get; set; }
	      
        public virtual CommercialSale CommercialSale { get; set; }
    }
}
	 