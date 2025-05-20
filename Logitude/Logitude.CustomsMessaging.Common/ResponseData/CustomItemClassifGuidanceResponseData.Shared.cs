using System;
using System.Collections.Generic;


namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CustomItemClassifGuidanceResponseData : ResponseDataBase
    {
        public List<CustomItemClassifGuidanceResult> CustomItemClassifGuidanceList { get; set; } = new List<CustomItemClassifGuidanceResult>();

    }

    public class CustomItemClassifGuidanceResult
    {
        public string classificationGuidanceNumber { get; set; }
        public string title { get; set; }
        public string classificationGuidanceTypeName { get; set; }
        public string fullClassification { get; set; }
        public DateTime? publicationDate { get; set; }
        public int? customsItemId { get; set; }
    }

    
}
