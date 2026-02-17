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
using Logitude.Infrastructure.Data.EntityPOCOs;
namespace Logitude.CRM.Data.EntityPOCOs
{
   
    public class SLAEscalation
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("SLAHeader")]
        [Column("SLAHeaderId")]
	    public string SLAHeaderId { get; set; }
	      
        public virtual SLAHeader SLAHeader { get; set; }
        [Column("LineNumber")]
	    public int LineNumber { get; set; }
        [Column("EscalationFor")]
	    public string EscalationFor { get; set; }
        [ForeignKey("EscalationActionTime")]
        [Column("EscalationActionTimeIndicator")]
	    public string EscalationActionTimeIndicator { get; set; }
	      
        public virtual EscalationActionTimeIndicator EscalationActionTime { get; set; }
        [Column("EscalationTime")]
	    public int? EscalationTime { get; set; }
        [ForeignKey("TimeUnit")]
        [Column("EscalationTimeUnit")]
	    public string EscalationTimeUnit { get; set; }
	      
        public virtual TimeUnit TimeUnit { get; set; }
        [Column("EscalaitonTimeInMinutes")]
	    public int? EscalaitonTimeInMinutes { get; set; }
    }
}
	 