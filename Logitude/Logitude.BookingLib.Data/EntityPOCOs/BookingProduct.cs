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
 
namespace Logitude.BookingLib.Data.EntityPOCOs
{
   
    public class BookingProduct
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Code")]
	    public string Code { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("Airline")]
        [Column("AirlineId")]
	    public string AirlineId { get; set; }
	      
        public virtual Card Airline { get; set; }
        [Column("InActive")]
	    public bool InActive { get; set; }
    }
}
	 