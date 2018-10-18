using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DocumentTypeCopy
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DocumentTypeId { get; set; }
        public int IndexOrder { get; set; }
        public bool IsSelectedByDefault { get; set; }
        public bool InActive { get; set; }
        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }
        //public List<DocumentOutCopy> DocumentOutCopies { get; set; }
    }
}