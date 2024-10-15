using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CheckFileCrediteReq
	{
        public string AppicationId { get; set; }
        public string ClassName { get; set; }
        public string LoggingUserId { get; set; }
        public string LoggingEntityReference { get; set; }
        public string LoggingObjectTableId { get; set; }
    }
}
