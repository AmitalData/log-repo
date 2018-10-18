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
    public class DCAInVAL_NG_8228_RequiredDocumentVerificationDecisionMassagingService : MessagingServiceBase<
        RequiredDocumentRequestParams,
        RequiredDocumentResponseData,
        DCAInCustomRequest,
        VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage,
        DCAInRequiredDocumentCustomRequestService,
        VAL_NG_8228_RequiredDocumentVerificationDecisionResponseService, DCAInRequestHeader>
    {
        protected override VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage CallWS(DCAInCustomRequest customRequest, RequiredDocumentRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "8228"; }
        }

        protected override RequiredDocumentRequestParams CreateDefaultRequestParamsFromCustomsResponse(VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage customsResponse)
        {
            var tableName = "Customs.CustomsDocument";

            var myGenericRequestParams = new RequiredDocumentRequestParams()
             {
                 LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
             };
             return myGenericRequestParams;
        }
    }
}
