using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TermsofUseSignature
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime SignedDatetime { get; set; }
        public string ContactId { get; set; }
        public int TermsofUseId { get; set; }

        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        [ForeignKey("TermsofUseId")]
        public virtual TermsofUse TermsofUse { get; set; }
    }
}