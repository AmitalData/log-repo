
                                                                    //Yuval Chalup 29.10.2015 TASK-16002

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CourierBOLQueryRequestParams : RequestParamsBase
    {
        public string CourierBOL { get; set; }
        public string CourierVAT { get; set; }
        public string DeclarationId { get; set; }
        public string CustomFileNo { get; set; }
    }
}