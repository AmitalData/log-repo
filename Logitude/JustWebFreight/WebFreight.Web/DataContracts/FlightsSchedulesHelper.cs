using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class FlightsSchedulesHelper
    {
        [Key]
        public int Id { get; set; }

        public int Tenant { get; set; }
        public string FromPortId { get; set; }
        public DateTime? FromDate { get; set; }
        public TimeSpan? FromTime { get; set; }

        public string ToPortId { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? ToTime { get; set; }

        public string Airline1Id { get; set; }
        public string Airline2Id { get; set; }
        public string Airline3Id { get; set; }
        public string Airline4Id { get; set; }

        public string Flight1Number { get; set; }
        public string Flight2Number { get; set; }
        public string Flight3Number { get; set; }
        public string Flight4Number { get; set; }
    }
}