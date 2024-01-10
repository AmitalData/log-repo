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
   
    public class CB_CustomsItem
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [Column("FullClassification")]
	    public string FullClassification { get; set; }
        [ForeignKey("Parent_CustomsItemID")]
        [Column("Parent_CustomsItemID")]
	    public string Parent_CustomsItemID { get; set; }
	      
        public virtual CB_CustomsItem Parent_CustomsItemID { get; set; }
        [Column("ComputedCheckDigit")]
	    public string ComputedCheckDigit { get; set; }
        [ForeignKey("CustomsBookTypeID")]
        [Column("CustomsBookTypeID")]
	    public string CustomsBookTypeID { get; set; }
	      
        public virtual CustomsBookType CustomsBookTypeID { get; set; }
        [ForeignKey("CustomsItemCategoryID")]
        [Column("CustomsItemCategoryID")]
	    public string CustomsItemCategoryID { get; set; }
	      
        public virtual CustomsItemCategory CustomsItemCategoryID { get; set; }
        [ForeignKey("CustomsItemHierarchicLocationID")]
     
	    public string CustomsItemHierarchicLocationID { get; set; }
	      
        public virtual CustomsItemHierarchicLocation CustomsItemHierarchicLocationID { get; set; }
    }
}
	 