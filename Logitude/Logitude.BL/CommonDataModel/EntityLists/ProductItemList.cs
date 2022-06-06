
using System.ComponentModel.DataAnnotations;


namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ProductItemList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomerId { get; set; }
        public string SKU { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string SearchFields { get; set; }
        public string ASIN { get; set; }
        public string UPC { get; set; }
        public string OriginCountryId { get; set; }
        public string OriginCountryName { get; set; }
        public string ShipperId { get; set; }
        public string ShipperName { get; set; }
        public double? ProductValue { get; set; }
        public string ProductValueCurrencyId { get; set; }
        public int? Quantity { get; set; }
        public string ProductValueCurrencyCode { get; set; }
    }
}
