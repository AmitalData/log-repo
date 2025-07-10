using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ObjectTableHelperControl
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ControlPath { get; set; }
        public string Code { get; set; }
        public string ObjectTableId { get; set; }
        public string FeatureId { get; set; }
        public string FeatureUniqeCode { get; set; }

        //[ForeignKey("FeatureId")]
        public Feature Feature { get; set; }


        //[Include]
        //[Association("ObjectTableHelperControlObjectTable", "ObjectTableId", "Id", IsForeignKey = true)]

        [ForeignKey("ObjectTableId")]
        public ObjectTable ObjectTable { get; set; }
    }
}