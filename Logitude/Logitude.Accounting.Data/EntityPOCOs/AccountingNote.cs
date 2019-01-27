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
   
    public class AccountingNote
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
        [ForeignKey("Card")]
        [Column("CardId")]
	    public string CardId { get; set; }
	      
        public virtual Card Card { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
    }
}
	 