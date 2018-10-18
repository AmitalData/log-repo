using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Vendor
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }

        public virtual Card Card { get; set; }
    }
}