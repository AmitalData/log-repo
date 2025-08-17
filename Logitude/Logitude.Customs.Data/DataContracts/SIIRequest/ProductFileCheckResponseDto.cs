using System.Collections.Generic;


namespace Logitude.Customs.Data.DataContracts.SIIRequest
{
    public class ProductFileCheckResponseDto
    {
        public List<ProductFileItem> productFiles { get; set; }
        public int responseCode { get; set; }
    }

    public class ProductFileItem
    {
        public string id { get; set; }
        public string description { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public Dictionary<string, string> models { get; set; }
    }

}
