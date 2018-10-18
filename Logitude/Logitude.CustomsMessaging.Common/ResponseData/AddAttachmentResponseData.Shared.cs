using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class AddAttachmentResponseData : ResponseDataBase
    {
        public string ApplicationID { get; set; }
        public string DocumentNumber { get; set; }
        public string CustomDocument { get; set; }
        public string CustomRecievedDate { get; set; }
        public string Remarks { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorRemarks { get; set; }
    }
}
