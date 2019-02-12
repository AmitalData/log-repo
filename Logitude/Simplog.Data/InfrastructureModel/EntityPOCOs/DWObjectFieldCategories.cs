using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class DWObjectFieldCategories
    {
        [Key]
        public string Id { get; set; }
        public string DWObjectFieldCode { get; set; }
        public string DWCategoryCode { get; set; }

        //[ForeignKey("DWObjectFieldCode")]
        //public virtual DWObjectField DWObjectField { get; set; }

        [ForeignKey("DWCategoryCode")]
        public virtual DWCategories DWCategory { get; set; }

    }
}
