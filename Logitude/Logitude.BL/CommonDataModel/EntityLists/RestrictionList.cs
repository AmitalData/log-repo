using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class RestrictionList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectFieldId { get; set; }
        public string Value { get; set; }
        public string ContactTenantId { get; set; }
    }
}