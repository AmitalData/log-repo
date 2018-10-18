using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ClosingReasonDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsClosedLost { get; set; }
        public string SearchFields { get; set; }
        public string LocalName { get; set; }
        public bool AddedManually { get; set; }
        public int Tenant { get; set; }
    }
}