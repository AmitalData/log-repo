using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class MorningMessageResponseData : ResponseDataBase
    {
        public  List<MorningMessageResult> MorningMessageList { get; set; }
    }

    public class MorningMessageResult
    {
        public string MessageID { get; set; }
        public string Category { get; set; }
        public string CategoryName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string MessageDate { get; set; }
    }
}
