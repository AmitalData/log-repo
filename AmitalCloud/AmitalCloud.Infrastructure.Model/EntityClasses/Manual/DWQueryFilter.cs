using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DWQueryFilter
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DWQueryId { get; set; }
        public string DWObjectFieldId { get; set; }
        public bool IsPredefined { get; set; }
        public string PredefinedValue { get; set; }
        public string PredefinedValue2 { get; set; }
        public string Operator { get; set; }
        public int IndexOrder { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        [ForeignKey("DWQueryId")]
        public virtual DWQuery DWQuery { get; set; }


        [ForeignKey("DWObjectFieldId")]
        public virtual DWObjectField DWObjectField { get; set; }
    }


}
