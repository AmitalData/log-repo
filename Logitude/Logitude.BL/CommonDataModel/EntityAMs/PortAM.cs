using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityAMs
{
    public class PortAM
    {
        public int Tenant { get; set; }

        private string code;
        public string Code
        {
            get { return code; }
            set
            {
                code = value.ToUpper();
            }
        }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Notes { get; set; }
        public bool InActive { get; set; }
        public bool IsAir { get; set; }
        public bool IsOcean { get; set; }
        public bool IsInland { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string TimeZoneCode { get; set; }
        public string PortGroupCode { get; set; }
    }
}
