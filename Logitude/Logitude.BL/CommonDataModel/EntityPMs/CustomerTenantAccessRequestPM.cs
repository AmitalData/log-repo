using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
   public class CustomerTenantAccessRequestPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ForwarderId { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string RequestStatus { get; set; }
        public string StatusName { get; set; }
        public string ForwarderName { get; set; }
        public bool IsCustoms { get; set; }
        public bool IsExport { get; set; }
    }
}
