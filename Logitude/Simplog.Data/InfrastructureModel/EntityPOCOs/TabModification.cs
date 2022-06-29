using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class TabModification
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TabId { get; set; }
        public string TabCode { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public ObjectTableTab Tab { get; set; }

    }
}