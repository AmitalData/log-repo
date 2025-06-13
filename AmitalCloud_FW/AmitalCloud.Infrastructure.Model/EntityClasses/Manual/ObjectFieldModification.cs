using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ObjectFieldModification
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public bool IsRequired { get; set; }
        public string ObjectFieldId { get; set; }
        public string ObjectFieldCode { get; set; }
        public DateTime? UpdateDateGMT { get; set; }

        [ForeignKey("ObjectFieldId")]
        public virtual ObjectField ObjectField { get; set; }
    }
}