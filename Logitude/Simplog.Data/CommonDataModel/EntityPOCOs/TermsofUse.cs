using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TermsofUse
    {
        [Key]
        public int Version { get; set; }
        public DateTime Date { get; set; }

        //public List<TermsofUseSignature> TermsofUseSignatures { get; set; }
    }
}