using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
   public class QuoteDocumentVersion
    {
        [Key]
       public string QuoteId { get; set; }

        [Key]
        public int VersionNumber { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }

        public string VersionType { get; set; }
        public string DocumentId { get; set; }
        public DateTime? SendDate { get; set; }
        public bool IsSent { get; set; }


        public string QuoteTemplateId { get; set; }


        [ForeignKey("QuoteTemplateId")]
        public virtual QuoteTemplate QuoteTemplate { get; set; }


        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Doc { get; set; }


        [ForeignKey("QuoteId")]
        public virtual Quote Quote { get; set; }
    }
}
