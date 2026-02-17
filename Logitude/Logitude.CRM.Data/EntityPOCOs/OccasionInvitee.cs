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
   
    public class OccasionInvitee
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("AddedDate")]
	    public DateTime AddedDate { get; set; }
        [ForeignKey("AddedByUser")]
        [Column("AddedByUserId")]
	    public string AddedByUserId { get; set; }
	      
        public virtual User AddedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [ForeignKey("Occasion")]
        [Column("OccasionId")]
	    public string OccasionId { get; set; }
	      
        public virtual Occasion Occasion { get; set; }
        [ForeignKey("Contact")]
        [Column("ContactId")]
	    public string ContactId { get; set; }
	      
        public virtual Contact Contact { get; set; }
        [Column("Invited")]
	    public bool Invited { get; set; }
        [Column("Participated")]
	    public bool Participated { get; set; }
    }
}
	 