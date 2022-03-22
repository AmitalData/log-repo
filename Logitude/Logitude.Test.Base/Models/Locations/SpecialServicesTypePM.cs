using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Base.Models.Locations
{
    public class SpecialServicesTypePM
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public int Tenant { get; set; }
        public bool IsHybrid { get; set; }
        public bool IsSecured { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
    }
}
