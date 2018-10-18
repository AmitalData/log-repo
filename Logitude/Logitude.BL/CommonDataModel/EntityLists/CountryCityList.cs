using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CountryCityList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Notes { get; set; }
        public bool InActive { get; set; }
        public bool AddedManually { get; set; }
        public string SearchFields { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryEnglishName { get; set; }
        public string StateId { get; set; }
        public string StateCode { get; set; }
        public string StateEnglishName { get; set; }
    }
}