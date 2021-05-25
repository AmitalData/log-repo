using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TermsofUseSignatureList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime SignedDatetime { get; set; }
        public string ContactId { get; set; }
        public int TermsofUseId { get; set; }
        public int VersionNumber { get; set; }
        public string VersionDocumentId { get; set; }

    }
}