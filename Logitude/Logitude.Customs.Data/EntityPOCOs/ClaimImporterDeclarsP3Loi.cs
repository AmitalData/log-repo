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
   
    public class ClaimImporterDeclarsP3Loi
    {
	 string dbms;

        [Key]
        [ForeignKey("ClaimImporterDeclarsPage3")]
        [Column("ClaimId" ,Order = 1)]
	    public string ClaimId { get; set; }
	      
        public virtual ClaimImporterDeclarsPage3 ClaimImporterDeclarsPage3 { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("ClaimImporterDeclarsPage3")]
        [Column("CounterKey" ,Order = 2)]
	    public int CounterKey { get; set; }
     [Key]
        [Column("LineNo" ,Order = 3)]
	    public int LineNo { get; set; }
        [Column("DeclarationNumber")]
	    public string DeclarationNumber { get; set; }
    }
}
	 