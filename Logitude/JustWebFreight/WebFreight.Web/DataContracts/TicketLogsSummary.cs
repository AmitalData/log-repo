using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class TicketLogsSummary
    {
        [Key]
        public int Id { get; set; }

        public TimeSpan ResponseSLAViolated { get; set; }

        public TimeSpan ResolveSLAViolated { get; set; }

        public int OpenPeriodMinutes { get; set; }

        public double ResponseSLAViolatedTotalMinutes { get; set; }
        public double ResolveSLAViolatedTotalMinutes { get; set; }


    }

}