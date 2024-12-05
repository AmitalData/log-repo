using System;
using System.Collections.Generic;


namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class GetClassifGuidanceDetailsResponseData : ResponseDataBase
    {
        public string classificationGuidanceNumber { get; set; }
        public string title { get; set; }
        public string classificationGuidanceTypeName { get; set; }
        public string fullClassificationItem { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public DateTime publicationDate { get; set; }
        public string classificationGuidanceTextRTF { get; set; }
        public List<ClassifGuidanceAttached> classifGuidanceAttached { get; set; } = new List<ClassifGuidanceAttached>();

    }

    public class ClassifGuidanceAttached
    {
        public string fullClassification { get; set; }
        public int attachedCustomsItemID { get; set; }

    }




}
