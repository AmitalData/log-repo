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
   
    public class ActivityEmailRecipient
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Activity")]
        [Column("ActivityId")]
	    public string ActivityId { get; set; }
	      
        public virtual Activity Activity { get; set; }
        [ForeignKey("Contact")]
        [Column("ContactId")]
	    public string ContactId { get; set; }
	      
        public virtual Contact Contact { get; set; }
        [Column("Email")]
	    public string Email { get; set; }
        [Column("RecipientTypeCode")]
	    public string RecipientTypeCode { get; set; }
        [ForeignKey("SenderContact")]
        [Column("SenderContactId")]
	    public string SenderContactId { get; set; }
	      
        public virtual Contact SenderContact { get; set; }
    }
}
	 