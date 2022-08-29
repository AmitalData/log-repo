using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class PortList
    {
        [Key]
        public string Id { get; set; }

        private string code;


        public string Code
        {
            get { return code; }
            set
            {
                code = value.ToUpper();
            }
        }


        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }

        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string CountryId { get; set; }

        public string EnglishName { get; set; }
        public string SearchFields { get; set; }

        public bool IsAir { get; set; }
        public bool IsOcean { get; set; }
        public bool IsInland { get; set; }
        public bool AddedManually { get; set; }
        public string StateId { get; set; }
        public bool CountryEC { get; set; }
        public string CombinedCode { get; set; }
        // by islam
        public bool InUse { get; set; }
        public string RecentlyAdded { get; set; }
       
        public string TransportModeId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }
    }
}