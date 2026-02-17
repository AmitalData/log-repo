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

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class Deficit
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("DebtNotificationType")]
        [Column("NotificationTypeCode")]
	    public string NotificationTypeCode { get; set; }
	      
        public virtual DebtNotificationType DebtNotificationType { get; set; }
        [Column("DebtNotificationNumber")]
	    public string DebtNotificationNumber { get; set; }
        [Column("ProductionDate")]
	    public DateTime? ProductionDate { get; set; }
        [Column("DebtNotificationReason")]
	    public string DebtNotificationReason { get; set; }
        [Column("RealesGoodsDescription")]
	    public string RealesGoodsDescription { get; set; }
        [Column("ValidityDateTo")]
	    public DateTime? ValidityDateTo { get; set; }
        [Column("PaymentOrderNumber")]
	    public string PaymentOrderNumber { get; set; }
        [ForeignKey("Tapag")]
        [Column("TapagId")]
	    public string TapagId { get; set; }
	      
        public virtual Tapag Tapag { get; set; }
    }
}
	 