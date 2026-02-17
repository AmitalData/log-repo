using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Ransom;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInVAL_NG_8227_MSG_520_RequiredDocumentMassagingService : MessagingServiceBase<
        RequiredDocumentRequestParams,
        RequiredDocumentResponseData,
        DCAInCustomRequest,
        VAL_NG_8227_MSG_520_RequiredDocumentMessage,
        DCAInRequiredDocumentCustomRequestService,
        VAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService, DCAInRequestHeader>
    {

        protected override RequiredDocumentRequestParams CreateDefaultRequestParamsFromCustomsResponse(VAL_NG_8227_MSG_520_RequiredDocumentMessage customsResponse)
        {
            var myGenericRequestParams = new RequiredDocumentRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument"),
                LoggingEntityId = customsResponse.RequiredDocumentDetails.documentID.ToString()
            };
            return myGenericRequestParams;

        }

        protected override VAL_NG_8227_MSG_520_RequiredDocumentMessage CallWS(DCAInCustomRequest customRequest, RequiredDocumentRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "8227"; }
        }

    }
}
