using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
    [DataContract(Namespace = "")]
    public class AutomationQueuedTask
    {
        [DataMember]
        public string DateFieldValue { get; set; }

        [DataMember]
        public string OffsetTypeValue { get; set; }

        [DataMember]
        public string TaskTimeUnitValue { get; set; }

        [DataMember]
        public string QueueId { get; set; }

        [DataMember]
        public string TeamId { get; set; }

        [DataMember]
        public int? TaskOffset { get; set; }

        [DataMember]
        public string TaskOwnerId { get; set; }

        [DataMember]
        public string TaskCustomerId { get; set; }

        [DataMember]
        public string TaskSubject { get; set; }

        [DataMember]
        public string TaskPriorityId { get; set; }

        [DataMember]
        public string TaskDescription { get; set; }
    }
}
