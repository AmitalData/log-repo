using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CommunicationAttachment
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CommunicationLogId { get; set; }
        public string DocumentId { get; set; }
        [ForeignKey("CommunicationLogId")]
        public virtual CommunicationLog CommunicationLog { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
    }
}
