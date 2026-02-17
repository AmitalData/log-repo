using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Restriction
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectFieldId { get; set; }
        public string Value { get; set; }
        public string ContactTenantId { get; set; }

         [ForeignKey("ObjectTableId")]
        public ObjectTable ObjectTable { get; set; }
        [ForeignKey("ObjectFieldId")]
        public ObjectField ObjectField { get; set; }
        [ForeignKey("ContactTenantId")]
        public ContactTenant ContactTenant { get; set; }

    }
}