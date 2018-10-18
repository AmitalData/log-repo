using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class VesselList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Notes { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string IMOCode { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}