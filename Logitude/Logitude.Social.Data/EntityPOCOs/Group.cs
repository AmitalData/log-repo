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
   
    public class Group
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [Column("IsPrivate")]
	    public bool IsPrivate { get; set; }
        [ForeignKey("User")]
        [Column("OwnerId")]
	    public string OwnerId { get; set; }
	      
        public virtual User User { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
    }
}
	 