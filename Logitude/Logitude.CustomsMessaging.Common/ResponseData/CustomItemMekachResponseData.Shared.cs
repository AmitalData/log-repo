using System;
using System.Collections.Generic;
using System.Net.Mail;


namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CustomItemMekachResponseData : ResponseDataBase
    {
        public List<CustomItemMekachData> CustomItemMekachDataList { get; set; }
    }

    public class CustomItemMekachData
    {
        public int mekachNumber { get; set; }
        public Attachment attachedMekahFile { get; set; }
        public DateTime validityDate { get; set; }
        public string changeDescription { get; set; }
    }
}

