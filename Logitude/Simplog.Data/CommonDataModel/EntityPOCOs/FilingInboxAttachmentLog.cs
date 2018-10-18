using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class FilingInboxAttachmentLog
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentsFilingId { get; set; }
        public string FilingInboxAttachmentId { get; set; }

        [ForeignKey("DocumentsFilingId")]
        public virtual DocumentsFiling DocumentsFiling { get; set; }

        [ForeignKey("FilingInboxAttachmentId")]
        public virtual FilingInboxAttachment FilingInboxAttachment { get; set; }
    }
}
