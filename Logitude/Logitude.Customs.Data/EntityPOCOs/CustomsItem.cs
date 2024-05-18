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
   
    public class CustomsItem
    {
	 string dbms;

           [Column("CustomsBookTypeID")]
	    public int CustomsBookTypeID { get; set; }
        [Column("FullClassification")]
	    public string FullClassification { get; set; }
        [Column("CustomsItemCategoryID")]
	    public int CustomsItemCategoryID { get; set; }
        [Column("CustomsItemHierarchicLocatioID")]
	    public int? CustomsItemHierarchicLocatioID { get; set; }
        [Column("ComputedCheckDigit")]
	    public string ComputedCheckDigit { get; set; }
     [Key]
        [Column("ID")]
	    public string ID { get; set; }
    }
}
	 