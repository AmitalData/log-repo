using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Xml.Serialization;


namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class AttachmentResponseData : ResponseDataBase
    {
        public AttachedMekahFileData AttachedMekahFileData { get; set; }

    }

    public class AttachedMekahFileData
    {
        public string fileName { get; set; }
        [XmlIgnore]
        public string content { get; set; }
    }
}

