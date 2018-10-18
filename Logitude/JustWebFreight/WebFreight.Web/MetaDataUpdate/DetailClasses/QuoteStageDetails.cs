using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class QuoteStageDetails
    {
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? MaxDays { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
    }
}