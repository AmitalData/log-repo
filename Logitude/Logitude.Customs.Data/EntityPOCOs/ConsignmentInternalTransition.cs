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
   
    public class ConsignmentInternalTransition
    {
	 string dbms;

        [Key]
        [ForeignKey("Consignment")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual Consignment Consignment { get; set; }
     [Key]
        [ForeignKey("Consignment")]
        [Column("ConsignmentNumber" ,Order = 2)]
	    public int? ConsignmentNumber { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("InternalBorderSiteType")]
        [Column("SiteCode")]
	    public string SiteCode { get; set; }
	      
        public virtual InternalBorderSiteType InternalBorderSiteType { get; set; }
     [Key]
        [Column("LineNumber" ,Order = 3)]
	    public int LineNumber { get; set; }
    }
}
	 