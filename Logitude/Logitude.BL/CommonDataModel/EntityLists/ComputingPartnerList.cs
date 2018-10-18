using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ComputingPartnerList
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public int LoggedTenantId { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }

    }
}