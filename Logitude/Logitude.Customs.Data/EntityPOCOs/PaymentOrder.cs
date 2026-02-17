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
   
    public class PaymentOrder
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("PaymentNumber")]
	    public string PaymentNumber { get; set; }
        [Column("TotalSumToPay")]
	    public decimal? TotalSumToPay { get; set; }
        [Column("LastPayDate")]
	    public DateTime? LastPayDate { get; set; }
        [Column("Reason")]
	    public string Reason { get; set; }
        [ForeignKey("CustomerCard")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card CustomerCard { get; set; }
        [ForeignKey("CustomerActivityType")]
        [Column("CustomerActivityTypeCode")]
	    public string CustomerActivityTypeCode { get; set; }
	      
        public virtual CustomerActivityType CustomerActivityType { get; set; }
        [ForeignKey("PaymentOrderType")]
        [Column("PaymentOrderTypeCode")]
	    public string PaymentOrderTypeCode { get; set; }
	      
        public virtual PaymentOrderType PaymentOrderType { get; set; }
        [ForeignKey("PaymentProcess")]
        [Column("PaymentProcessCode")]
	    public string PaymentProcessCode { get; set; }
	      
        public virtual PaymentProcess PaymentProcess { get; set; }
        [ForeignKey("PaymentStatus")]
        [Column("PaymentStatusCode")]
	    public string PaymentStatusCode { get; set; }
	      
        public virtual PaymentOrderStatus PaymentStatus { get; set; }
        [ForeignKey("CustomsHouseType")]
        [Column("CustomsHouseCode")]
	    public string CustomsHouseCode { get; set; }
	      
        public virtual CustomsHouseType CustomsHouseType { get; set; }
        [Column("ActualPayDate")]
	    public DateTime? ActualPayDate { get; set; }
        [Column("InternalNotes")]
	    public string InternalNotes { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("EntityTypeLookup")]
        [Column("CustomsEntityTypeCode")]
	    public string CustomsEntityTypeCode { get; set; }
	      
        public virtual EntityTypeLookup EntityTypeLookup { get; set; }
        [Column("FirstEntityID")]
	    public string FirstEntityID { get; set; }
        [Column("SecondEntityID")]
	    public string SecondEntityID { get; set; }
        [Column("ThirdEntityID")]
	    public string ThirdEntityID { get; set; }
        [ForeignKey("Client")]
        [Column("ImporterId")]
	    public string ImporterId { get; set; }
	      
        public virtual Client Client { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
        [Column("ConcurrencyGUID")]
	    public string ConcurrencyGUID { get; set; }
        [Column("AccountingCustomFile")]
	    public string AccountingCustomFile { get; set; }
        [Column("CustomFiles")]
	    public string CustomFiles { get; set; }
        [Column("PaymentOrderSelectedLabel")]
	    public string PaymentOrderSelectedLabel { get; set; }
        [Column("PaymentOrderLeftAmount")]
	    public decimal? PaymentOrderLeftAmount { get; set; }
    }
}
	 