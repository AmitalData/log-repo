

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
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.MessageLib.Vendor;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    class DCAInVE_3700_ImporterPeriodicDeclarationReplyMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        VE_MSG032_ImporterPeriodicDeclarationReplyMessage,
        DCAInCustomRequestService,
        VE_3700_ImporterPeriodicDeclarationReplyResponseService, DCAInRequestHeader>
    {
        protected override VE_MSG032_ImporterPeriodicDeclarationReplyMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check
            var response = new VE_MSG032_ImporterPeriodicDeclarationReplyMessage();
            /*// var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<>()
                    .VendorRepository(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this.mP,
                    out response);
            }*/
            return response;
        }

        public override string MainInterfaceCode
        {
            get { return "3700"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(VE_MSG032_ImporterPeriodicDeclarationReplyMessage customsResponse)
        {
            var tableName = "Customs.ImporterDesposition";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
    }
}

