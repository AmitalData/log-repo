using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClientApplication
{
    public class APIResponseParameters
    {
        public int Tenant { get; set; }
        public string ApiTanentType { get; set; }
        public List<string> APIsNames { get; set; }
        public Dictionary<string, string> XMLRequestText { get; set; }

    }
}
