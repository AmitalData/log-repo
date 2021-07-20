using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ProductItem
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
        public string ASIN { get; set; }
        public string UPC { get; set; }
        public string OriginCountryId { get; set; }

        [ForeignKey("OriginCountryId")]
        public virtual Country OriginCountry { get; set; }
    }
}
