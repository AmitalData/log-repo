using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ComputingPartnerTable
    {
        [Key]
        public string ObjectTableId { get; set; }
        [Key]
        public string ComputingPartnerId { get; set; }
        [Key]
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

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        [ForeignKey("ComputingPartnerId")]
        public virtual ComputingPartner ComputingPartner { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
    }
}
