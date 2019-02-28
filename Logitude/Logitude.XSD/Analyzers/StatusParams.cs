using Logitude.BookingLib.Data;
using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers
{
    public class StatusParams
    {
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string StatusCode { get; set; }
        public string Details { get; set; }
        public string FromPortId { get; set; }
        public string FromPortCode { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortId { get; set; }
        public string FlightNumber { get; set; }
        public int Pieces { get; set; }
        public bool Partial { get; set; }
        public decimal Weight { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime LogDate { get; set; }
        public string Location { get; set; }
        public string LocationPortId { get; set; }
        public string LocationPortCode { get; set; }
        public string AirlineName { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string TimeOfDepartureInfo { get; set; }
        public string TimeOfArrivalInfo { get; set; }
        public ICommonDataContext CommonContext { get; set; }
        public IShipmentsContext ShipmentContext { get; set; }
        public IBookingContext BookingContext { get; set; }
    }
}
