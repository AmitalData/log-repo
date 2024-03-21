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
   
    public class CB_CustomsItemDetailsHistory
    {
	 string dbms;

           [Column("ID")]
	    public int ID { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("Title")]
	    public string Title { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime? EndDate { get; set; }
        [ForeignKey("EntityStatusCode")]
        [Column("EntityStatusID")]
	    public string EntityStatusID { get; set; }
	      
        public virtual CustomsEntityStatus EntityStatusCode { get; set; }
        [Column("EnglishGoodsDescription")]
	    public string EnglishGoodsDescription { get; set; }
        [Column("GoodsDescription")]
	    public string GoodsDescription { get; set; }
        [Column("GoodsDescriptionRTF")]
	    public string GoodsDescriptionRTF { get; set; }
        [Column("EnglishGoodsDescriptionRTF")]
	    public string EnglishGoodsDescriptionRTF { get; set; }
        [Column("CustomsItemID")]
	    public int CustomsItemID { get; set; }
        [Column("ChangeRequestTypePriority")]
	    public int ChangeRequestTypePriority { get; set; }
     [Key]
        [Column("CB_ID")]
	    public string CB_ID { get; set; }
    }
}
	 