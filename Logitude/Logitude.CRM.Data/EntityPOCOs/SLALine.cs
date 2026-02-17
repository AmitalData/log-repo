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
   
    public class SLALine
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
        [ForeignKey("TicketSeverity")]
        [Column("SeverityId")]
	    public string SeverityId { get; set; }
	      
        public virtual TicketSeverity TicketSeverity { get; set; }
        [ForeignKey("BusinessHour")]
        [Column("BusinessHoursId")]
	    public string BusinessHoursId { get; set; }
	      
        public virtual BusinessHour BusinessHour { get; set; }
        [Column("FirstResponseTime")]
	    public int? FirstResponseTime { get; set; }
        [ForeignKey("TimeUnit")]
        [Column("FirstResponseTimeUnit")]
	    public string FirstResponseTimeUnit { get; set; }
	      
        public virtual TimeUnit TimeUnit { get; set; }
        [Column("FirstResponseTimeInMinute")]
	    public int? FirstResponseTimeInMinute { get; set; }
        [Column("ResolveWithinTime")]
	    public int? ResolveWithinTime { get; set; }
        [ForeignKey("ResolveTimeUnit")]
        [Column("ResolveWithinTimeUnit")]
	    public string ResolveWithinTimeUnit { get; set; }
	      
        public virtual TimeUnit ResolveTimeUnit { get; set; }
        [Column("ResolveWithinTimeInMinute")]
	    public int? ResolveWithinTimeInMinute { get; set; }
        [Column("FirstResponseEscalate")]
	    public bool FirstResponseEscalate { get; set; }
        [Column("ResolveWithinEscalate")]
	    public bool ResolveWithinEscalate { get; set; }
    }
}
	 