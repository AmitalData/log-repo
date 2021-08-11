using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Models.PackageTypesPreparation
{
    public class PackageType
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EnglishName { get; set; }
        public bool IsOcean { get; set; }
        public bool IsAir { get; set; }
        public bool IsInland { get; set; }
        public bool IsContainer { get; set; }
        public string PrintAs { get; set; }
    }
}
