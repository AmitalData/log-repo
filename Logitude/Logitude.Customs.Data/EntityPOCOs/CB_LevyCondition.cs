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
   
    public class CB_LevyCondition
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public string ID { get; set; }
        [Column("LevyConditionNumber")]
	    public int? LevyConditionNumber { get; set; }
        [Column("LevyGoodsDescription")]
	    public string LevyGoodsDescription { get; set; }
        [ForeignKey("CustomsItem")]
        [Column("CustomsItemID")]
	    public string CustomsItemID { get; set; }
	      
        public virtual CB_CustomsItem CustomsItem { get; set; }
        [ForeignKey("Vendor")]
        [Column("VendorID")]
	    public string VendorID { get; set; }
	      
        public virtual CB_Vendor Vendor { get; set; }
        [ForeignKey("CountryGroupCode")]
        [Column("CountryGroupID")]
	    public string CountryGroupID { get; set; }
	      
        public virtual CountryGroup CountryGroupCode { get; set; }
        [Column("IsCountriesGroup")]
	    public bool IsCountriesGroup { get; set; }
        [Column("CountryID")]
	    public string CountryID { get; set; }
        [ForeignKey("TradeLevy")]
        [Column("TradeLevyID")]
	    public string TradeLevyID { get; set; }
	      
        public virtual CB_TradeLevy TradeLevy { get; set; }
        [Column("StartDate")]
	    public DateTime StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime? EndDate { get; set; }
    }
}
	 