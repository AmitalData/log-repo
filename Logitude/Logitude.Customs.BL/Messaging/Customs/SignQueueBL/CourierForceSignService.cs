using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    internal class CourierForceSignService
    {
        internal void ApplyForceSign<TRequestParams>(ref TRequestParams requestParams) where TRequestParams : RequestParamsBase
        {
            switch (requestParams.InterfaceTypeCode)
            {
                //case "2715":
                //    requestParams.ForcePersonalSign = true;
                //    break;

                case "2750":
                    requestParams.ForcePersonalSign = true;//DEFAULT HSM 
                    break;
                default:
                    break;
            }
        }
    }
}
