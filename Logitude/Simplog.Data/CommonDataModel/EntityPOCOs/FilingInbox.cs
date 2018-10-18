using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class FilingInbox
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string BodyDocumentId { get; set; }
        public string SearchFields { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("BodyDocumentId")]
        public virtual Document BodyDocument { get; set; }
    }
}
