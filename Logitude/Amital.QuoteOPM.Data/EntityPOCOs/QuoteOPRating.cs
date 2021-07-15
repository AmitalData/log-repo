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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPRating
    {
	 string dbms;

        [Key]
        [Column("Code")]
	    public string Code { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("IndexOrder")]
	    public int IndexOrder { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
    }
}
	 