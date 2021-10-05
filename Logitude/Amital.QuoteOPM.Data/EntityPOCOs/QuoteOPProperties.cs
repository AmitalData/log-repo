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
   
    public class QuoteOPProperties
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("QuoteID")]
	    public string QuoteID { get; set; }
        [Column("Order")]
	    public int Order { get; set; }
        [Column("FromPortId")]
	    public string FromPortId { get; set; }
        [Column("ToPortId")]
	    public string ToPortId { get; set; }
        [Column("IncotermId")]
	    public string IncotermId { get; set; }
        [Column("SpecialServiceID")]
	    public string SpecialServiceID { get; set; }
        [Column("MainCarriageCarrierId")]
	    public string MainCarriageCarrierId { get; set; }
    }
}
	 