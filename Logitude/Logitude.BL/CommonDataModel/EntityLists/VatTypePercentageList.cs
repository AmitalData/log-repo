using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class VatTypePercentageList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string VatTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public double? Percentage { get; set; }

        
    }
}
