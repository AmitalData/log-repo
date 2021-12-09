using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Simplog.Data.Helpers
{
    [DataContract(Namespace = "")]
    public class AutomationEvent
    {
        [DataMember]
        public string EventTypeId { get; set; }
        [DataMember]
        public string NoteValue { get; set; }
        [DataMember]
        public string ObjectTableName { get; set; }  
        [DataMember]
        public string ObjectTableId { get; set; }
    }
}
