using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class BookingsDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public string SelectedCustomerName { get; set; }
        public string SelectedCustomerLabel { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<BookingRecord> BookingRecordList { get; set; }
    }

    public class BookingRecord
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; } // tenant number
        public string CustomerName { get; set; }//(Tenant Name)
        public string Prefix { get; set; }
        public string AWBNumber { get; set; }
        public decimal? Weight { get; set; }
        public string Status { get; set; }
        public string Product { get; set; }
        public string BookedByUser { get; set; }//(user that sent the FFR)
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? BookedDate { get; set; }//(last FFR sent date)
        public DateTime? CreateDate { get; set; }
        public string CreatedByUser { get; set; }
        public string BookingNumber { get; set; }
    }
}