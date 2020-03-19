using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class ScreenModification
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ScreenId { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public string ScreenCode { get; set; }

        [ForeignKey("ScreenId")]
        public virtual Screen Screen { get; set; }

    }
}