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
   
    public class ReferantException
    {
	 string dbms;

        [Key]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("ExceptionReason")]
        [Column("ExceptionReasonsCode")]
	    public string ExceptionReasonsCode { get; set; }
	      
        public virtual ExceptionReason ExceptionReason { get; set; }
        [Column("ExceptionRemarks")]
	    public string ExceptionRemarks { get; set; }
        [Column("Status")]
	    public string Status { get; set; }
    }
}
	 