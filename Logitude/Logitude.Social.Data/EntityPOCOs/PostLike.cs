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
   
    public class PostLike
    {
	 string dbms;

        [Key]
        [ForeignKey("Post")]
        [Column("PostId" ,Order = 1)]
	    public string PostId { get; set; }
	      
        public virtual Post Post { get; set; }
     [Key]
        [ForeignKey("User")]
        [Column("UserId" ,Order = 2)]
	    public string UserId { get; set; }
	      
        public virtual User User { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
    }
}
	 