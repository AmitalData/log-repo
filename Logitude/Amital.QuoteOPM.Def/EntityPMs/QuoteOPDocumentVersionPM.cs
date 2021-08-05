using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.Def.EntityPMs
{
    public class QuoteOPDocumentVersionPM
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

        public string CreatedByUserName { get; set; }
        public string UpdateByUserName { get; set; }
        public string VersionTypeName { get; set; }

        public double? FileSize { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
