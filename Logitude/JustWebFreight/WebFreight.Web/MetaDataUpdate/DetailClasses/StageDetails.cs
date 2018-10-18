using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class StageDetails
    {
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Probability { get; set; }
        public string SearchFields { get; set; }
        public bool IsSelectable { get; set; }
    }
}