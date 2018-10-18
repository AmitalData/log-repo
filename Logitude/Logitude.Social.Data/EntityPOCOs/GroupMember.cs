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
 
namespace Logitude.Social.Data.EntityPOCOs
{
   
    public class GroupMember
    {
	 string dbms;

        [Key]
        [ForeignKey("Group")]
        [Column("GroupId" ,Order = 1)]
	    public string GroupId { get; set; }
	      
        public virtual Group Group { get; set; }
     [Key]
        [ForeignKey("User")]
        [Column("UserId" ,Order = 2)]
	    public string UserId { get; set; }
	      
        public virtual User User { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("CancelledDate")]
	    public DateTime? CancelledDate { get; set; }
    }
}
	 