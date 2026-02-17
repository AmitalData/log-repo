using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class ValidateShipmentMasterArgs
    {
        [Key]
        public string ShipmentId { get; set; }
        public string BookingId { get; set; }
        public string Master { get; set; }
        public string AirlinePrefix { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public bool IsCancelled { get; set; }
    }
}