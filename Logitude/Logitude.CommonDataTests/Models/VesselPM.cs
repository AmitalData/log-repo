using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CommonDataTests.Models
{
    public class VesselPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Notes { get; set; }
        public string ComputedLocalName { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public bool IsHybrid { get; set; }
        public string IMOCode { get; set; }
    }
}
