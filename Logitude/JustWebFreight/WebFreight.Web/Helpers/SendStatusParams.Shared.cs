using System;
using System.Collections.Generic;
using System.Linq;


namespace WebFreight.Web.Helpers
{
    public class SendStatusParams
    {

        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string AirlineTTY { get; set; }
        public string StatusCodes { get; set; }
        public DateTime? EventDate { get; set; }
        public int NumberOfPieces { get; set; }
        public decimal Weight { get; set; }
        public string StatusType { get; set; }
        public string FromPortCode { get; set; }
        public string ToPortCode { get; set; }
        public int DepartureTime { get; set; }
        public int ArrivalTime { get; set; }
        public string TimeOfDepartureInfo { get; set; }
        public string TimeOfArrivalInfo { get; set; }
        public string DepartureDayChangeIndicator { get; set; }
        public string ArrivalDayChangeIndicator { get; set; }
        public string OSIFirstLine { get; set; }
        public string OSISecondLine { get; set; }
        public bool IsOSI { get; set; }
        public string Master { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public int OriginalNumberOfPackages { get; set; }
    }
}