using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.EntityLists
{
     
    public partial class OpportunityStageList
    {
        [DataMember]
        public string FromStageName { get; set; }

        [DataMember]
        public string ToStageName { get; set; }

        [DataMember]
        public string OpportunityTopic { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public DateTime? LastModifiedDate { get; set; }

        [DataMember]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        public string Customer { get; set; }

        [DataMember]
        public string OpportunityType { get; set; }

        [DataMember]
        public string OwnerName { get; set; }

        [DataMember]
        public bool IsCancelled { get; set; }

        [DataMember]
        public string LeadSourceId { get; set; }
    }
}
