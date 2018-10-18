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
 
namespace Logitude.Accounting.Data.EntityPOCOs
{
   
    public class BankCheque
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
        [ForeignKey("BankAccount")]
        [Column("BankAccountId")]
	    public string BankAccountId { get; set; }
	      
        public virtual BankAccount BankAccount { get; set; }
        [Column("FirstNumber")]
	    public string FirstNumber { get; set; }
        [Column("CurrentChequeNumber")]
	    public string CurrentChequeNumber { get; set; }
        [Column("LastNumber")]
	    public string LastNumber { get; set; }
        [Column("Inactive")]
	    public bool? Inactive { get; set; }
        [Column("IsEnded")]
	    public bool? IsEnded { get; set; }
    }
}
	 