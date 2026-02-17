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

namespace Logitude.Infrastructure.Data.EntityPOCOs
{
   
    public class BIReportsExecutionLog
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("CommunicationStatusType")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual CommunicationStatusType CommunicationStatusType { get; set; }
        [Column("ExceptionMessage")]
	    public string ExceptionMessage { get; set; }
        [Column("DoneDate")]
	    public DateTime? DoneDate { get; set; }
        [Column("ReportFilterXML")]
	    public string ReportFilterXML { get; set; }
        [Column("BIReportId")]
	    public string BIReportId { get; set; }
    }
}
	 