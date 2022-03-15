using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class PaymentTermList
    {
        [Key]
        public string Id { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string ComputedLocalName { get; set; }
        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public bool AddedManually { get; set; }
        public int Days { get; set; }
        public bool DisplayInLOV { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }
        public string SearchFields { get; set; }
        public bool IsManuallySet { get; set; }
        public string ExternalId { get; set; }
        public bool EndOfMonth { get; set; }
        public int NumberOfMonths { get; set; }
        public string FromDateTypeCode { get; set; }
        public string CalculatedLocalName { get; set; }
        public string CalculatedEnglishName { get; set; }
        public string Code { get; set; }
       
    }
}