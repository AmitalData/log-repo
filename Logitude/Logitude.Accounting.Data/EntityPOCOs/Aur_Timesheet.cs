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

namespace Logitude.Accounting.Data.EntityPOCOs
{
   
    public class Aur_Timesheet
    {
	 string dbms;

        [Key]
        [Column("Line")]
	    public int Line { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("UserReport")]
	    public string UserReport { get; set; }
        [Column("ExecutionDate")]
	    public DateTime ExecutionDate { get; set; }
        [Column("RelatedProject")]
	    public string RelatedProject { get; set; }
        [Column("ProjectNumber")]
	    public string ProjectNumber { get; set; }
        [Column("ApprovesRelatedWork")]
	    public string ApprovesRelatedWork { get; set; }
        [Column("CRMRelatedWork")]
	    public string CRMRelatedWork { get; set; }
        [Column("CRMContactperson")]
	    public string CRMContactperson { get; set; }
        [Column("ConfirmRequestCRM")]
	    public string ConfirmRequestCRM { get; set; }
        [Column("RelatedTask")]
	    public string RelatedTask { get; set; }
        [Column("WorkType")]
	    public string WorkType { get; set; }
        [Column("CompletedEffort")]
	    public decimal CompletedEffort { get; set; }
        [Column("BillableHours")]
	    public decimal? BillableHours { get; set; }
        [Column("EmployeeType")]
	    public string EmployeeType { get; set; }
        [Column("HourlyRate")]
	    public decimal HourlyRate { get; set; }
     [Key]
        [Column("PaymentId")]
	    public string PaymentId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 