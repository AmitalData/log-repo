using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{

    [DataContract(Namespace = "")]
    public class AutomationFollowUp
    {
        [DataMember]
        public string FollowUpEnglishName { get; set; }

        [DataMember]
        public string EventTypeId { get; set; }
        [DataMember]
        public string NoteValue { get; set; }
        [DataMember]
        public string OwnerValue { get; set; }
        [DataMember]
        public string OwnerFieldType { get; set; }

        [DataMember]
        public string DateEscalaActTimeIndicatorCode { get; set; }



        [DataMember]
        public int DateEscalationTime { get; set; }


        [DataMember]
        public string DateValue { get; set; }
        [DataMember]
        public string ObjectTableName { get; set; }
        [DataMember]
        public string LegType { get; set; }

        private List<FollowUpDocumentTypeList> documentTypeLists;
        [DataMember]
        public List<FollowUpDocumentTypeList> DocumentTypeLists
        {
            get
            {
                return this.documentTypeLists;
            }
            set
            {
                this.documentTypeLists = value;
            }
        }




    }

    [DataContract(Namespace = "")]
    public class FollowUpDocumentTypeList
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Area { get; set; }
    }


}

