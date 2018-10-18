using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class InboundEmailPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        [DataMember]
        public string EntityId { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public DateTime UpdateDate { get; set; }

        [DataMember]
        public string Uniquekey { get; set; }

        [DataMember]
        public string ObjectTableId { get; set; }

        [DataMember]
        public string CreatedByContactId { get; set; }

        private List<InboundEmailLinePM> inboundEmailLines;
        [Include]
        [Composition]
        [Association("InboundEmailLinePMInboundEmailPM", "Id", "InboundEmailId")]
        public virtual List<InboundEmailLinePM> InboundEmailLines
        {
            get
            {
                if (inboundEmailLines == null)
                {
                    inboundEmailLines = new List<InboundEmailLinePM>();
                }

                return this.inboundEmailLines;
            }
            set
            {
                if (value != null)
                {
                    inboundEmailLines = value;
                }
            }
        }

        [DataMember]
        public string ObjectTableName { get; set; }

        [DataMember]
        public bool IsRejected { get; set; }

        [DataMember]
        public string AnalyzeQueueId { get; set; }

    }
}
