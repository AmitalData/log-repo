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
   
    public class CB_CustomsBookAddition
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public string ID { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [Column("TypeID")]
	    public string TypeID { get; set; }
        [Column("Title")]
	    public string Title { get; set; }
        [ForeignKey("CustomsBookTypeCode")]
        [Column("CustomsBookTypeID")]
	    public string CustomsBookTypeID { get; set; }
	      
        public virtual CustomsBookType CustomsBookTypeCode { get; set; }
        [Column("AdditionCode")]
	    public int? AdditionCode { get; set; }
    }
}
	 