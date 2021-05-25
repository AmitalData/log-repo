using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TermsofUse
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int VersionNumber { get; set; }
        public int Tenant { get; set; }
        public string VersionDocumentId { get; set; }

        //public List<TermsofUseSignature> TermsofUseSignatures { get; set; }
    }
}