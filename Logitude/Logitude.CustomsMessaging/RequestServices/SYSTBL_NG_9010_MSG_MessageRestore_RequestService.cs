using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageRestoreServiceReference;
using UnifreightIIG.Common.MessageToAgentServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class SYSTBL_NG_9010_MSG_MessageRestore_RequestService
        : RequestServiceBase<SYSTBL_NG_9010_MSG_MessageRestoreRequest, MessageRestoreRequestParams>
    {
        public override SYSTBL_NG_9010_MSG_MessageRestoreRequest GetRequest(MessageRestoreRequestParams requestParams)
        {
            //this.MyRequestSheetParam = new RequestSheetParam() {  RequestDescription  ="Resotre Messages Request"};
            //this.MyRequestSheetParam.RequestDescription = "Resotre Messages Request Erroor test"; LogMessagingUtil.Instance.AppendLine("itzik test  !!"); throw new System.Exception("test");
            //CorrelationNumber = correlationNo,
            //        InterfaceManagementsCode = InterfaceManagementsCode,
            //        FromDate=fromDate,
            //        ToDate=toDate,
            var customReq = new SYSTBL_NG_9010_MSG_MessageRestoreRequest()
            {
                CorrelationID = requestParams.CorrelationID,
                //fromDate = requestParams.FromDate.GetValueOrDefault(),
                //fromDateSpecified = requestParams.FromDate.HasValue,
                //toDate = requestParams.ToDate.GetValueOrDefault(),
                //toDateSpecified = requestParams.ToDate.HasValue,
                serviceName = requestParams.InterfaceManagementsCode
            };

            DateTime dateTime;
            if (requestParams.FromDate != null)
            {
                if (DateTime.TryParse(requestParams.FromDate.ToString(), out dateTime))
                {
                    customReq.fromDate = dateTime;
                    customReq.fromDateSpecified = requestParams.FromDate.HasValue;
                }
            }
            if (requestParams.ToDate != null)
            {
                if (DateTime.TryParse(requestParams.ToDate.ToString(), out dateTime))
                {
                    customReq.toDate = dateTime;
                    customReq.toDateSpecified = requestParams.ToDate.HasValue;
                }
            }
            if (String.IsNullOrWhiteSpace(requestParams.CorrelationID))
            {
                customReq.CorrelationID = null;
            }
            return customReq;
        }
    }
}

