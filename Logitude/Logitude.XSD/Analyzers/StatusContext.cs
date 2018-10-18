using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers
{
    public class StatusContext
    {
        public string StatusCode { get; set; }
        public string Location { get; set; }
        public DateTime? EventDate { get; set; }
        public decimal Weight { get; set; }
        public int NumberOfPieces { get; set; }
        public bool IsPartial { get; set; }
        public string FlightNumber { get; set; }
        public DateTime LogDate { get; set; }
        public string Details { get; set; }
        public string CodeOfArrival { get; set; }
        public string CodeOfDeparture { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string TimeOfArrivalInfo { get; set; }
        public string TimeOfDepartureInfo { get; set; }
    }
}
