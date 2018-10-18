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
   
    public class FlightsSchedulesResponse
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("FromPort")]
        [Column("FromPortId")]
	    public string FromPortId { get; set; }
	      
        public virtual Port FromPort { get; set; }
        [ForeignKey("ToPort")]
        [Column("ToPortId")]
	    public string ToPortId { get; set; }
	      
        public virtual Port ToPort { get; set; }
        [ForeignKey("Airline")]
        [Column("AirlineId")]
	    public string AirlineId { get; set; }
	      
        public virtual Card Airline { get; set; }
        [ForeignKey("FlightsSchedulesRequest")]
        [Column("RequestId")]
	    public string RequestId { get; set; }
	      
        public virtual FlightsSchedulesRequest FlightsSchedulesRequest { get; set; }
        [Column("ETD")]
	    public DateTime? ETD { get; set; }
        [Column("ETA")]
	    public DateTime? ETA { get; set; }
        [Column("FlightNumber")]
	    public string FlightNumber { get; set; }
        [Column("AirplaneType")]
	    public string AirplaneType { get; set; }
        [Column("NumberOfStops")]
	    public int NumberOfStops { get; set; }
        [Column("ResultNumber")]
	    public int ResultNumber { get; set; }
        [Column("LineNumber")]
	    public int LineNumber { get; set; }
        [Column("FromPortCode")]
	    public string FromPortCode { get; set; }
        [Column("FromPortName")]
	    public string FromPortName { get; set; }
        [Column("ToPortCode")]
	    public string ToPortCode { get; set; }
        [Column("ToPortName")]
	    public string ToPortName { get; set; }
        [Column("MissingPort")]
	    public bool MissingPort { get; set; }
    }
}
	 