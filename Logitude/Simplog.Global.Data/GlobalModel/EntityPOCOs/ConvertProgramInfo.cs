using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class ConvertProgramInfo
    {
        [Key]
        public string Id { get; set; }
        public string MethodName { get; set; }
        public bool IsApplied { get; set; }
        public string GlobalDBId { get; set; }
        [ForeignKey("GlobalDBId")]
        public GlobalDB GlobalDB { get; set; }
    }
}