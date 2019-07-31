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
   
    public class Post
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("User")]
        [Column("CreatedById")]
	    public string CreatedById { get; set; }
	      
        public virtual User User { get; set; }
        [ForeignKey("Group")]
        [Column("GroupId")]
	    public string GroupId { get; set; }
	      
        public virtual Group Group { get; set; }
        [Column("BodyText")]
	    public string BodyText { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [ForeignKey("ParentPost")]
        [Column("ParentPostId")]
	    public string ParentPostId { get; set; }
	      
        public virtual Post ParentPost { get; set; }
        [Column("NumberOfLikes")]
	    public int NumberOfLikes { get; set; }
        [Column("IsPrivate")]
	    public bool IsPrivate { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("ObjectTableId")]
	    public string ObjectTableId { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [Column("EntityId")]
	    public string EntityId { get; set; }
        [Column("IsAutomatic")]
	    public bool IsAutomatic { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("EntityDescription")]
	    public string EntityDescription { get; set; }
        [Column("NumberOfComments")]
	    public int NumberOfComments { get; set; }
    }
}
	 