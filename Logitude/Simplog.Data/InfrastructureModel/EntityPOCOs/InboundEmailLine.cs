using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class InboundEmailLine
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string Sender { get; set; }
        public string Recepient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FullBody { get; set; }
        public string HTMLFullBody { get; set; }

        public DateTime CreateDate { get; set; }

        public string CCs { get; set; }
        public string Bcc { get; set; }

        public string InternalUsers { get; set; }

        public string Direction { get; set; }

        public string EntityLineId  { get; set; }

        public string InboundEmailId { get; set; }
        [ForeignKey("InboundEmailId")]
        public virtual InboundEmail InboundEmail { get; set; }

        public string CommunicationLogId { get; set; }
        [ForeignKey("CommunicationLogId")]
        public virtual CommunicationLog CommunicationLog { get; set; }
    }
}
