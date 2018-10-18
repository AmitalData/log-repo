using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class FVRServiceArgs
    {
        [Key]
        public string AirlineId { get; set; }
        public string ShipmentId { get; set; }
        public string BookingId { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public decimal? Volume { get; set; }
        public decimal? GrossWeight { get; set; }
        public string VolumeUnitCode { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string Recipient { get; set; }
    }
}