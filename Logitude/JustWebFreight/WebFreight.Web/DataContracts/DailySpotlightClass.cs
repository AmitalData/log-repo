using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class DailySpotlightClass
    {
        [Key]
        public int Id { get; set; }
        
        public int Quotes_Today { get; set; }
        public int Quotes_Yesterday { get; set; }
        public int Quotes_LastWeek { get; set; }

        public int Shipments_Today { get; set; }
        public int Shipments_Yesterday { get; set; }
        public int Shipments_LastWeek { get; set; }

        public int ARInvoices_Today { get; set; }
        public int ARInvoices_Yesterday { get; set; }
        public int ARInvoices_LastWeek { get; set; }

        public int Activities_Today { get; set; }
        public int Activities_Yesterday { get; set; }
        public int Activities_LastWeek { get; set; }

        public int Opportunities_Today { get; set; }
        public int Opportunities_Yesterday { get; set; }
        public int Opportunities_LastWeek { get; set; }

        public int Customers_Today { get; set; }
        public int Customers_Yesterday { get; set; }
        public int Customers_LastWeek { get; set; }

        public int PotentialCustomers_Today { get; set; }
        public int PotentialCustomers_Yesterday { get; set; }
        public int PotentialCustomers_LastWeek { get; set; }

        //Airline DashBoard
        public int FWB_Today { get; set; }
        public int FWB_Yesterday { get; set; }
        public int FWB_LastWeek { get; set; }

        public int FHL_Today { get; set; }
        public int FHL_Yesterday { get; set; }
        public int FHL_LastWeek { get; set; }

        public int FFR_Today { get; set; }
        public int FFR_Yesterday { get; set; }
        public int FFR_LastWeek { get; set; }

        public int Participations_Today { get; set; }
        public int Participations_Yesterday { get; set; }
        public int Participations_LastWeek { get; set; }

        public int NewParticipants_Today { get; set; }
        public int NewParticipants_Yesterday { get; set; }
        public int NewParticipants_LastWeek { get; set; }
    }
}