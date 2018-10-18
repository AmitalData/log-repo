using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class DocumentOutCopyList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentId { get; set; }
        public string DocumentOutId { get; set; }
        public string DocumentTypeCopyId { get; set; }
    }
}