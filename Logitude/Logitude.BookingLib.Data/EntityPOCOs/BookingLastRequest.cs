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
   
    public class BookingLastRequest
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Booking")]
        [Column("BookingId")]
	    public string BookingId { get; set; }
	      
        public virtual Booking Booking { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [Column("ETD")]
	    public DateTime? ETD { get; set; }
        [Column("FlightNumber")]
	    public string FlightNumber { get; set; }
        [Column("Origin")]
	    public string Origin { get; set; }
        [Column("Destination")]
	    public string Destination { get; set; }
        [ForeignKey("Carrier")]
        [Column("CarrierId")]
	    public string CarrierId { get; set; }
	      
        public virtual Card Carrier { get; set; }
    }
}
	 