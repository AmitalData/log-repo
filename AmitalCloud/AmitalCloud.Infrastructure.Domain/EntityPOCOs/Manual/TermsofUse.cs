using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class TermsofUse
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int VersionNumber { get; set; }
        public int Tenant { get; set; }
        public string VersionDocumentId { get; set; }
        public string PrivateLabelId { get; set; }
        public bool IsNew { get; set; }

        //public List<TermsofUseSignature> TermsofUseSignatures { get; set; }
    }
}
