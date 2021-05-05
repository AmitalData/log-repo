using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ExternalAPIResponseParameters
    {
        public int Tenant { get; set; }
        public string ApiTanentType { get; set; }
        public List<string> APIsNames { get; set; } 
        public Dictionary<string, string> XMLRequestText { get; set; }
    }
}