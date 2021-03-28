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
   
    public class ExportDeliveryDocumentMessage
    {
	 string dbms;

        [Key]
        [Column("Code")]
	    public string Code { get; set; }
        [Column("EnglishName")]
	    public string EnglishName { get; set; }
        [Column("LocalName")]
	    public string LocalName { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("Inactive")]
	    public bool Inactive { get; set; }
    }
}
	 