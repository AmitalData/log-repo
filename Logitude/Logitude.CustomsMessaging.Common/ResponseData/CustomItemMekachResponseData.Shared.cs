using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Xml.Serialization;


namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CustomItemMekachResponseData : ResponseDataBase
    {
        public List<CustomItemMekachData> CustomItemMekachDataList { get; set; }
    }

    public class CustomItemMekachData
    {
        public int mekachNumber { get; set; }
        [XmlIgnore]
        public string attachedMekahFile { get; set; }
        public DateTime validityDate { get; set; }
        public string changeDescription { get; set; }
    }
}

