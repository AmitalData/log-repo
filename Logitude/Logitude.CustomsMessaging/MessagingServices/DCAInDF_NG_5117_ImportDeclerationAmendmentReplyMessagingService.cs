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
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using UnifreightIIG.Common.MessageLib.ID;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDF_NG_5117_ImportDeclerationAmendmentReplyMessagingService : MessagingServiceBase<
        GenericRequestParams, INF_MSG_GenericResponseData,
        DCAInCustomRequest, //DF_NG_2754_MSG10004_ImportDeclarationResponse
        DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg,
        DCAInCustomRequestService, DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
        DCAInRequestHeader>
    {
        protected override DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "5117"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };

            return myGenericRequestParams;
        }
    }
}
