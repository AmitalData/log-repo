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
   
    public class TMOfficeHour
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
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("OfficeHourUser")]
        [Column("UserId")]
	    public string UserId { get; set; }
	      
        public virtual User OfficeHourUser { get; set; }
        [Column("WorkDate")]
	    public DateTime WorkDate { get; set; }
        [Column("RecordedEntryTime")]
	    public DateTime? RecordedEntryTime { get; set; }
        [Column("RecordedExitTime")]
	    public DateTime? RecordedExitTime { get; set; }
        [Column("EntryTime")]
	    public DateTime? EntryTime { get; set; }
        [Column("ExitTime")]
	    public DateTime? ExitTime { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [Column("Inactive")]
	    public bool Inactive { get; set; }
    }
}
	 