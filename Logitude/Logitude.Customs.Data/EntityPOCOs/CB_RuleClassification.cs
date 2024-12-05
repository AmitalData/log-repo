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
   
    public class CB_RuleClassification
    {
	 string dbms;

        [Key]
        [Column("CB_ID")]
	    public string CB_ID { get; set; }
        [Column("CustomsItemID")]
	    public int CustomsItemID { get; set; }
        [Column("ID")]
	    public int ID { get; set; }
        [Column("CustomsBookType")]
	    public string CustomsBookType { get; set; }
        [Column("Rules")]
	    public string Rules { get; set; }
        [Column("ParentID")]
	    public int? ParentID { get; set; }
        [Column("Index")]
	    public string Index { get; set; }
    }
}
	 