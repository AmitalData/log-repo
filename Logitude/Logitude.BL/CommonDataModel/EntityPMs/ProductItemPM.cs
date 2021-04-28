using System.ComponentModel.DataAnnotations;


namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class ProductItemPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomerId { get; set; }
        public string SKU { get; set; }
        public string Remarks { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }

    }
}
