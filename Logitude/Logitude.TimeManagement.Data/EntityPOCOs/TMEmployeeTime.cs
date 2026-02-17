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

namespace Logitude.TimeManagement.Data.EntityPOCOs
{
   
    public class TMEmployeeTime
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
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [ForeignKey("EmployeeUser")]
        [Column("EmployeeUserId")]
	    public string EmployeeUserId { get; set; }
	      
        public virtual User EmployeeUser { get; set; }
        [Column("DateOfWork")]
	    public DateTime DateOfWork { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [Column("TimeInMinutes")]
	    public int TimeInMinutes { get; set; }
        [Column("WINumber")]
	    public string WINumber { get; set; }
        [Column("ProjectId")]
	    public string ProjectId { get; set; }
        [ForeignKey("Location")]
        [Column("LocationCode")]
	    public string LocationCode { get; set; }
	      
        public virtual TMLocation Location { get; set; }
        [Column("AnalyzeQueueId")]
	    public string AnalyzeQueueId { get; set; }
        [Column("SprintId")]
	    public string SprintId { get; set; }
        [Column("ProratedDuration")]
	    public double ProratedDuration { get; set; }
        [Column("FullDuration")]
	    public double FullDuration { get; set; }
        [Column("NeedsProrating")]
	    public bool NeedsProrating { get; set; }
    }
}
	 