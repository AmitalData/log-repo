using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ReportDetails
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string Description { get; set; }
        public string SearchFields { get; set; }
        public string FilterControlName { get; set; }
        public string ReportGroupId { get; set; }
        public string FeatureId { get; set; }
        public string FilterHtmlComponentUrl { get; set; }
    }
}