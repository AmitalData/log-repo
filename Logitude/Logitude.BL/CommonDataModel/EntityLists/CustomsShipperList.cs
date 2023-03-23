using Logitude.BL.InfrastructureModel.EntityLists;
using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
   public class CustomsShipperList : CustomFieldList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomsShipperCode { get; set; }
        public string ValidDepositionNumber { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public string SearchFields { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string ShipperVAT { get; set; }
    }
}