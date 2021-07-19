using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TermsofUseList
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int VersionNumber { get; set; }
        public int Tenant { get; set; }

        public string VersionDocumentId { get; set; }
        public string PrivateLabelId { get; set; }




    }
}
