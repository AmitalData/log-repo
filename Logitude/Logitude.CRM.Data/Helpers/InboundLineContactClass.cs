using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.Helpers
{
    public class InboundLineContactClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsInternal { get; set; }
        public bool RightToLeft { get; set; }
    }
}
