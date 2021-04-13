using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
    [DataContract(Namespace = "")]
    public class AutomationCreateTask
    {
        [DataMember]
        public string AssigneeId { get; set; }

        [DataMember]
        public string OwnerValue { get; set; }

        [DataMember]
        public string OwnerFieldType { get; set; }


        [DataMember]
        public string TaskType { get; set; }

        [DataMember]
        public string EndDateValue { get; set; }

        [DataMember]
        public string EndDateTypeValue { get; set; }

    }
}
