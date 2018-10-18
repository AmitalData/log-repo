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
using UnifreightIIG.Common.ImportDeclarationCancellationReplyMsgServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDF_5018_ImportExportDeclarationCancellationMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        DF_NG_5018_MSG14004_ImportDeclarationCancellationReplyMsg,
        DCAInCustomRequestService,
        DF_5018_ImportExportDeclarationCancellationResponseService, DCAInRequestHeader>
    {

        protected override DF_NG_5018_MSG14004_ImportDeclarationCancellationReplyMsg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "5018"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_5018_MSG14004_ImportDeclarationCancellationReplyMsg customsResponse)
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
