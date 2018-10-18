using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class QuoteFilters
    {
        public string CustomerId { get; set; }
        public bool IsCancelled { get; set; }
    }
}