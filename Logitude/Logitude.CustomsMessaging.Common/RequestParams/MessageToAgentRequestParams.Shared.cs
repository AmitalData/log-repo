using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{

    public class MessageToAgentRequestParams : RequestParamsBase
    {
        public string NotificationId { get; set; }
        public string DeclarationId { get; set; } //Yuval Chalup 18.06.2015 TASK-13858

    }
}
