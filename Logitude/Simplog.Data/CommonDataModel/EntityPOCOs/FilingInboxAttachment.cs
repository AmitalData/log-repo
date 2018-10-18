using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class FilingInboxAttachment
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string FileName { get; set; }
        public string DocumentId { get; set; }
        public string FilingInboxId { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        [ForeignKey("FilingInboxId")]
        public virtual FilingInbox FilingInbox { get; set; }
    }
}
