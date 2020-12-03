using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.FakeMessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using UnifreightIIG.Common.MessageLib.DeclarationCancel;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg : MessagingServiceBase<
        GenericRequestParams, INF_MSG_GenericResponseData,
        DCAInCustomRequest, //DF_NG_2754_MSG10004_ImportDeclarationResponse
        DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg,
        DCAInCustomRequestService, DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsgResponseService,
        DCAInRequestHeader>
    {


         protected override DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {

            var fake = new Fake_DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg(requestParamsData);
            return fake.GetFakeCustomsResponse(requestParamsData);

            
        }
        protected override DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "5118"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg customsResponse)
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
