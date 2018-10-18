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
   
    public class DecConsAcceptance
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("Declaration")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
     [Key]
        [Column("ConsignmentNumber" ,Order = 2)]
	    public int ConsignmentNumber { get; set; }
     [Key]
        [Column("LineNumber" ,Order = 3)]
	    public int LineNumber { get; set; }
        [Column("LoadDate")]
	    public DateTime? LoadDate { get; set; }
        [ForeignKey("PackingType")]
        [Column("PackageTypeCode")]
	    public string PackageTypeCode { get; set; }
	      
        public virtual PackingType PackingType { get; set; }
        [Column("PackageQuantity")]
	    public int? PackageQuantity { get; set; }
        [Column("GrossMassMeasure")]
	    public decimal? GrossMassMeasure { get; set; }
    }
}
	 