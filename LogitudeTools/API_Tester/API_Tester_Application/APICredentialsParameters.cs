using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClientApplication
{
    public class APICredentialsParameters
    {
        public string PrimaryKey { get; set; }
        public string SecondaryKey { get; set; }
        public int Tenant { get; set; }
    }
}
