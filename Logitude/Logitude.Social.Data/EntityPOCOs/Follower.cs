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
   
    public class Follower
    {
	 string dbms;

        [Key]
        [ForeignKey("FolloweeUser")]
        [Column("FolloweeUserId" ,Order = 1)]
	    public string FolloweeUserId { get; set; }
	      
        public virtual User FolloweeUser { get; set; }
     [Key]
        [ForeignKey("FollowerUser")]
        [Column("FollowerUserId" ,Order = 2)]
	    public string FollowerUserId { get; set; }
	      
        public virtual User FollowerUser { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("CancelledDate")]
	    public bool? CancelledDate { get; set; }
    }
}
	 