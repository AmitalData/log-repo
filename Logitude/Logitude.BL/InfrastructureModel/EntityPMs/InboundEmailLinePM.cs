using Simplog.Server.Infrastructure;
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
    public class InboundEmailLinePM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }

        [DataMember]
        public string Sender { get; set; }

        [DataMember]
        public string Recepient { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public string FullBody { get; set; }

        [DataMember]
        public string HTMLFullBody { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public string CCs { get; set; }

        [DataMember]
        public string Bcc { get; set; }

        [DataMember]
        public string InternalUsers { get; set; }

        [DataMember]
        public string Direction { get; set; }

        [DataMember]
        public string InboundEmailId { get; set; }

        [DataMember]
        public string CommunicationLogId { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

        [DataMember]
        public string EntityLineId { get; set; }

        [Include]
        [Association("InboundEmailLineInboundEmail", "InboundEmailId", "Id", IsForeignKey = true)]
        public virtual InboundEmailPM InboundEmail { get; set; }

    }
}
