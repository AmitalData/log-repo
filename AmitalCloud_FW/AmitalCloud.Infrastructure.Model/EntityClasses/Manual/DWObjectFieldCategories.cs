using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DWObjectFieldCategories
    {
        [Key]
        public string Id { get; set; }
        public string DWObjectFieldCode { get; set; }
        public string DWCategoryCode { get; set; }
        public string DWObjectTableCode { get; set; }

        //[ForeignKey("DWObjectFieldCode")]
        //public virtual DWObjectField DWObjectField { get; set; }

        [ForeignKey("DWCategoryCode")]
        public virtual DWCategories DWCategory { get; set; }

    }
}
