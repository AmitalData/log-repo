using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
 public   class HybridTenantStatePM
    {

        [Key]
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public int FailedQueue { get; set; }
        [DataMember]
        public int WaitingQueue { get; set; }
        [DataMember]
        public DateTime LastUpdateDateTime { get; set; }
        [DataMember]
        public DateTime? LastQueueDateTime { get; set; }
    }
}
