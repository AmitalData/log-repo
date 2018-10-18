using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ReportGroupDetails
    {
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public int OrderNumber { get; set; }
    }
}