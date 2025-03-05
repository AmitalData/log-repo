using AmitalCloud.Infrastructure.Domain.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ScreenField : BaseEntity
    {
        [Key]
        public string Id { get; set; }

        //[Required]
        public int Tenant { get; set; }

        public int Column { get; set; }
        public int Row { get; set; }

        public string ScreenId { get; set; }
        public string ScreenCode { get; set; }

        // public string FieldName { get; set; }
        public string ObjectFieldId { get; set; }
        public string ObjectFieldCode { get; set; }
        public int? SectionNumber { get; set; }



        //[Include]
        //[Association("ScreenScreenField", "ScreenId", "Id",IsForeignKey=true)]
        [ForeignKey("ScreenId")]
        public virtual Screen Screen { get; set; }

        //[Include]
        //[Association("ObjectFieldScreenField", "ObjectFieldId", "Id",IsForeignKey=true)]

        [ForeignKey("ObjectFieldId")]
        public virtual ObjectField ObjectField { get; set; }

    }
}