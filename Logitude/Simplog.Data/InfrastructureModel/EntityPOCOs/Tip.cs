using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class Tip
    {
         [Key]
        public string Code { get; set; }


        public int Tenant { get; set; }
        public bool VisibilityDefaultValue { get; set; }

        public string ShortTextCode { get; set; }

        public string ObjectTableId { get; set; }

        [ForeignKey("ObjectTableId")]
        //[Association("ObjectTableTip", "ObjectTableId", "Id", IsForeignKey = true)]
        public virtual ObjectTable ObjectTable { get; set; }

        [ForeignKey("ShortTextCode")]
        public virtual TextCode TextCode { get; set; }
        //public List<TipsVisibility> TipsVisibilities { get; set; }
        //public List<ObjectTable> ObjectTables { get; set; }

    }
}