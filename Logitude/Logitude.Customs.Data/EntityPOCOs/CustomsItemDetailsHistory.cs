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
   
    public class CustomsItemDetailsHistory
    {
	 string dbms;

           [ForeignKey("CustomsItem")]
        [Column("CustomsItemID" ,Order = 1)]
	    public string CustomsItemID { get; set; }
	      
        public virtual CustomsItem CustomsItem { get; set; }
     [Key]
        [Column("ID" ,Order = 2)]
	    public string ID { get; set; }
        [Column("Title" ,Order = 3)]
	    public string Title { get; set; }
        [Column("StartDate" ,Order = 4)]
	    public DateTime StartDate { get; set; }
        [Column("EndDate" ,Order = 5)]
	    public DateTime EndDate { get; set; }
        [Column("EntityStatusID")]
	    public int EntityStatusID { get; set; }
    }
}
	 