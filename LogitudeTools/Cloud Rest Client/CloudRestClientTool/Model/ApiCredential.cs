using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRestClientTool
{
    public class ApiCredential
    {
        public bool CustomerCare { get; set; }
        public bool HasError { get; set; }
        public bool InValidKey { get; set; }
        public bool IpRestricted { get; set; }
        public int Tenant { get; set; }
        public string Token { get; set; }
    }
}
