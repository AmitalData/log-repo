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
   
    public class CB_CustomsItemComputedData
    {
	 string dbms;

        [Key]
        [Column("CB_ID")]
	    public string CB_ID { get; set; }
        [Column("ID")]
	    public int ID { get; set; }
        [Column("CustomsItemID")]
	    public int CustomsItemID { get; set; }
        [Column("FullClassification")]
	    public string FullClassification { get; set; }
        [Column("IsLeaf")]
	    public bool IsLeaf { get; set; }
        [Column("CustomsItemDetailsHistoryID")]
	    public int CustomsItemDetailsHistoryID { get; set; }
        [Column("PropertiesDetailsHistoryID")]
	    public int PropertiesDetailsHistoryID { get; set; }
        [Column("PH_MeasurementUnitID")]
	    public int? PH_MeasurementUnitID { get; set; }
        [Column("IsHistoryExists")]
	    public bool IsHistoryExists { get; set; }
        [Column("IsRulesExists")]
	    public bool IsRulesExists { get; set; }
        [Column("StartDate")]
	    public DateTime StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime EndDate { get; set; }
        [Column("CI_Parent_CustomsItemIDNum")]
	    public int? CI_Parent_CustomsItemIDNum { get; set; }
        [Column("CI_BaseFullClassification")]
	    public string CI_BaseFullClassification { get; set; }
        [Column("CI_ComputedCheckDigit")]
	    public string CI_ComputedCheckDigit { get; set; }
        [ForeignKey("CustomsBookType")]
        [Column("CI_CustomsBookTypeIDNum")]
	    public string CI_CustomsBookTypeIDNum { get; set; }
	      
        public virtual CustomsBookType CustomsBookType { get; set; }
        [ForeignKey("CustomsItemCategory")]
        [Column("CI_CustomsItemCategoryIDNum")]
	    public string CI_CustomsItemCategoryIDNum { get; set; }
	      
        public virtual CustomsItemCategory CustomsItemCategory { get; set; }
        [ForeignKey("CustomsItemHierarchicLocation")]
        [Column("ItemHierarchicLocationID")]
	    public string ItemHierarchicLocationID { get; set; }
	      
        public virtual CustomsItemHierarchicLocation CustomsItemHierarchicLocation { get; set; }
        [Column("CIH_Title")]
	    public string CIH_Title { get; set; }
        [Column("CIH_GoodsDescription")]
	    public string CIH_GoodsDescription { get; set; }
        [Column("CustomsItemEntityStatusIDNum")]
	    public int CustomsItemEntityStatusIDNum { get; set; }
        [Column("PH_IsCarItem")]
	    public bool? PH_IsCarItem { get; set; }
        [Column("FullGoodsDescription")]
	    public string FullGoodsDescription { get; set; }
    }
}
	 