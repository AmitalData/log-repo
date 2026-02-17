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
   
    public class Correspondence
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CreatedByContact")]
        [Column("CreatedByContactId")]
	    public string CreatedByContactId { get; set; }
	      
        public virtual Contact CreatedByContact { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [Column("IsInternal")]
	    public bool IsInternal { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("ObjectTableId")]
	    public string ObjectTableId { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [Column("EntityId")]
	    public string EntityId { get; set; }
        [Column("ActivityTypeCode")]
	    public string ActivityTypeCode { get; set; }
        [Column("ActivityId")]
	    public string ActivityId { get; set; }
        [Column("ActivitySubject")]
	    public string ActivitySubject { get; set; }
        [Column("CCs")]
	    public string CCs { get; set; }
        [Column("Bcc")]
	    public string Bcc { get; set; }
        [Column("NotifyMe")]
	    public bool NotifyMe { get; set; }
        [Column("NotifyOwner")]
	    public bool NotifyOwner { get; set; }
        [Column("InternalUsers")]
	    public string InternalUsers { get; set; }
        [Column("Direction")]
	    public string Direction { get; set; }
        [Column("HTMLFullBody")]
	    public string HTMLFullBody { get; set; }
        [Column("RightToLeft")]
	    public bool RightToLeft { get; set; }
    }
}
	 