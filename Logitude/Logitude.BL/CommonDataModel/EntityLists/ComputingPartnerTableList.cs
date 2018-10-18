using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ComputingPartnerTableList
    {
        [Key]
        public string ObjectTableId { get; set; }
        [Key]
        public string ComputingPartnerId { get; set; }

        public int Tenant { get; set; }
        public string Name { get; set; }
        public bool HasPartnerList { get; set; }
        public bool MustUsePartnerList { get; set; }
        public bool TransalationRequired { get; set; }
        public bool TenantLevelTranslationBlocked { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }

        public string ObjectTableName { get; set; }
        public string ComputingPartnerName { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
    }
}