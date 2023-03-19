using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityAMs
{
    public class PortAM
    {
        public string Id { get; set; }
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
        public string CountryCode { get; set; }
        // public CountryPM countryPM;
        public string StateCode { get; set; }
        // public StatePM statePM
        public string PortTimeZoneCode { get; set; }
        // public PortTimeZonePM portTimeZonePM
    }
}
