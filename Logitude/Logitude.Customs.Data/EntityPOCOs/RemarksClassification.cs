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
   
    public class RemarksClassification
    {
	 string dbms;

           [Column("Id")]
	    public int Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("CB_ID")]
	    public string CB_ID { get; set; }
        [Column("CustomsItemsID")]
	    public int CustomsItemsID { get; set; }
        [Column("RemarkDescription")]
	    public string RemarkDescription { get; set; }
    }
}
	 