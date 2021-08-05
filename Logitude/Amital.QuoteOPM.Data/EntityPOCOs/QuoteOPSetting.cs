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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPSetting
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CopyExchangeRates")]
	    public bool CopyExchangeRates { get; set; }
        [Column("AutomaticallyCloseDays")]
	    public int AutomaticallyCloseDays { get; set; }
        [Column("CopyShipper")]
	    public bool CopyShipper { get; set; }
        [Column("CopyConsignee")]
	    public bool CopyConsignee { get; set; }
        [Column("CopyMainCarriage")]
	    public bool CopyMainCarriage { get; set; }
        [Column("CopyPickup")]
	    public bool CopyPickup { get; set; }
        [Column("CopyDelivery")]
	    public bool CopyDelivery { get; set; }
        [Column("CopyChargesTypes")]
	    public bool CopyChargesTypes { get; set; }
        [Column("CopyChargesCost")]
	    public bool CopyChargesCost { get; set; }
        [Column("CopyChargesSale")]
	    public bool CopyChargesSale { get; set; }
        [Column("EditMainCarriage")]
	    public bool EditMainCarriage { get; set; }
        [Column("CopyAgent")]
	    public bool CopyAgent { get; set; }
        [Column("CopyNotify")]
	    public bool CopyNotify { get; set; }
        [Column("IsSaleAsCostCurrency")]
	    public bool IsSaleAsCostCurrency { get; set; }
        [Column("IsMultiCurrency")]
	    public bool IsMultiCurrency { get; set; }
        [Column("QuoteExpirationDays")]
	    public int? QuoteExpirationDays { get; set; }
    }
}
	 