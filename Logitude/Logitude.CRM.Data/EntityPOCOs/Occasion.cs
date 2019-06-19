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
   
    public class Occasion
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
        [Column("StartDateTime")]
	    public DateTime StartDateTime { get; set; }
        [Column("EndDateTime")]
	    public DateTime EndDateTime { get; set; }
        [Column("Goal")]
	    public string Goal { get; set; }
        [Column("Location")]
	    public string Location { get; set; }
        [ForeignKey("Owner")]
        [Column("OwnerId")]
	    public string OwnerId { get; set; }
	      
        public virtual User Owner { get; set; }
        [ForeignKey("Industry")]
        [Column("IndustryId")]
	    public string IndustryId { get; set; }
	      
        public virtual Industry Industry { get; set; }
        [ForeignKey("OccasionType")]
        [Column("OccasionTypeId")]
	    public string OccasionTypeId { get; set; }
	      
        public virtual OccasionType OccasionType { get; set; }
        [ForeignKey("OccasionStatus")]
        [Column("OccasionStatusId")]
	    public string OccasionStatusId { get; set; }
	      
        public virtual OccasionStatus OccasionStatus { get; set; }
    }
}
	 