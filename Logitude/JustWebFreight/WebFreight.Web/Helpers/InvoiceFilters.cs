using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class InvoiceFilters
    {
        public string PartnerId { get; set; }
        public string PartnerType { get; set; }
        public string FilterName { get; set; }
        public string SearchField { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}