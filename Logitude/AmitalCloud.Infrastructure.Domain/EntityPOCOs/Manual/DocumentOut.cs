using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DocumentOut
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        //public string Note { get; set; }
        public bool Issued { get; set; }
        public byte[] EditableFields { get; set; }
        public string DocumentTemplateId { get; set; }
        public string EmailTemplateId { get; set; }
        public string XamlDocumentId { get; set; }
        public bool NeedsRebuild { get; set; }
        public bool IsBlobExist { get; set; }
        public DateTime? IssuedDate { get; set; }
        //public string EntityId { get; set; }
        //public string ObjectTableId { get; set; }
        public string IssuedByUserId { get; set; }
        //public string DocumentTypeId { get; set; }
       

        
        //public string ChildEntityId { get; set; }
        //public string ChildEntityReference { get; set; }
       
        //public bool IsCopy { get; set; }
        
        //public bool IsDuplex { get; set; }

        [ForeignKey("XamlDocumentId")]
        public Document XamlDocument { get; set; }
        //[ForeignKey("ObjectTableId")]
        //public virtual ObjectTable ObjectTable { get; set; }

        [ForeignKey("IssuedByUserId")]
        public User IssuedByUser { get; set; }

        //[ForeignKey("DocumentTypeId")]
        //public virtual DocumentType DocumentType { get; set; }
        
        public virtual DocumentsFiling DocumentsFiling { get; set; }



    }
}