using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DocumentOutCopy
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentId { get; set; }
        public string DocumentOutId { get; set; }
        public string DocumentTypeCopyId { get; set; }
        public string LastPrintedByUserId { get; set; }
        public DateTime? LastPrintDate { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
        [ForeignKey("DocumentOutId")]
        public virtual DocumentOut DocumentOut { get; set; }
        [ForeignKey("DocumentTypeCopyId")]
        public virtual DocumentTypeCopy DocumentTypeCopy { get; set; }
        [ForeignKey("LastPrintedByUserId")]
        public virtual User LastPrintedByUser { get; set; }
    }
}
