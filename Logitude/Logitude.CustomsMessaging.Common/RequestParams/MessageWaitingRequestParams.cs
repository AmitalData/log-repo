using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    
    public class MessageWaitingRequestParams : RequestParamsBase
    {
        //TODO TASK:8061
        //params from screen
        public string CorrelationID { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }



        //public string InterfaceManagementsCode { get; set; }
        //public string InterfaceManagementsCodeValue { get; set; }
        public string InterfaceManagementsCode { get; set; }
    }
}
