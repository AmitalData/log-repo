using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CountryList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }
        public bool EC { get; set; }
        public bool HasStates { get; set; }
        public bool IsStateRequired { get; set; }
        public string GlobalZoneName { get; set; }
        public string SearchFields { get; set; }
        public bool HasCitiesList { get; set; }
        public bool IsNorthAmerica { get; set; }
        public bool IsGreaterChina { get; set; }
    }
}