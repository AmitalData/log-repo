using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class OpportunityFilters
    {
        public string CustomerId { get; set; }
        public bool IsClosed { get; set; }
        public string SearchField { get; set; }
        public bool IsMyOpportunities { get; set; }
        public string OwnerId { get; set; }
    }
}