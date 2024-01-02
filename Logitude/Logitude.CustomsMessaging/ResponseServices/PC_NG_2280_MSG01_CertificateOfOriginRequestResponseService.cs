using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Common.RequestParams;
using UnifreightIIG.Common.LogisticActionRequestMessageServiceReference;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using UnifreightIIG.Common.CertificateOfOriginRequestServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class PC_NG_2280_MSG01_CertificateOfOriginRequestResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback, CertificateOfOriginRequestRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customResponse, CertificateOfOriginRequestRequestParams requestParams) =>
            this.MyResponseData;        

        public override void Update(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customResponse, CertificateOfOriginRequestRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            bool hasException = customResponse.ResponseContentHeader.Exception != null;

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = hasException;
            this.MyResponseData.ApplicationID = requestParams.CertificateOfOriginId; 
            this.MyResponseData.UserMessage = hasException ? customResponse.ResponseContentHeader.Exception[0].ExeptionDescription : "המסר התקבל בהצלחה במכס";
        }
    }
}
