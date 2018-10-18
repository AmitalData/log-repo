using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class CustomerFilters
    {
        public string SearchField { get; set; }
        public bool IsMyCustomers { get; set; }
        public int Take { get; set; }
    }
}