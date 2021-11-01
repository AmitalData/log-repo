using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class CustomerTenantAccessRequest
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string ForwarderId { get; set; }
        [ForeignKey("ForwarderId")]
        public virtual HybridPartner HybridPartnerId { get; set; }

        public DateTime RequestDateTime { get; set; }

        public string RequestStatus { get; set; }
        [ForeignKey("RequestStatus")]
        public virtual CustomerTenantAccessStatusType RequestStatusCode { get; set; }

        public bool IsCustoms { get; set; }
        public bool IsExport { get; set; }

    }
}
