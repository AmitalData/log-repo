using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class VatTypeList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string ComputedLocalName { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
         
        public string Description { get; set; }
        public string LocalDescription { get; set; }
        public string SearchFields { get; set; }
        public double? RecognizedPercentage { get; set; }
        //public double? Percentage { get; set; }
        public string PayablesExternalId { get; set; }
        public string ReceivablesExternalId { get; set; }
        public string ExternalTAXItemId { get; set; }
        public bool IsMultiPercentage { get; set; }
    }
}