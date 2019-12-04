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
   
    public class BIReport
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
        [Column("Name")]
	    public string Name { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [ForeignKey("DWQuery")]
        [Column("DWQueryId")]
	    public string DWQueryId { get; set; }
	      
        public virtual DWQuery DWQuery { get; set; }
        [Column("Inactive")]
	    public bool Inactive { get; set; }
        [ForeignKey("BIReportsType")]
        [Column("TypeCode")]
	    public string TypeCode { get; set; }
	      
        public virtual BIReportsType BIReportsType { get; set; }
        [Column("AGGridOptionsXML")]
	    public string AGGridOptionsXML { get; set; }
        [ForeignKey("BIReportFolder")]
        [Column("BIReportFolderId")]
	    public string BIReportFolderId { get; set; }
	      
        public virtual BIReportFolder BIReportFolder { get; set; }
        [Column("LastRunDate")]
	    public DateTime LastRunDate { get; set; }
        [ForeignKey("LastRunByUser")]
        [Column("LastRunByUserId")]
	    public string LastRunByUserId { get; set; }
	      
        public virtual User LastRunByUser { get; set; }
    }
}
	 