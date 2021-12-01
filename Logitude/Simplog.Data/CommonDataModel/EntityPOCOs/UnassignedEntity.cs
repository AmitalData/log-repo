using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class UnassignedEntity
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string UnassignedCode { get; set; }

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable EntityObjectTable { get; set; }
    }
}
