                                                                    //Yuval Chalup 23.06.2015 TASK-13278

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CreditQueryRequestParams : RequestParamsBase
    {
        public string AgentID { get; set; }
        public string AgentExternalId { get; set; }
        public string ExtertnalID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}