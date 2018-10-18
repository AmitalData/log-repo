using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CargoQueryMessageServiceReference;
using UnifreightIIG.Common.TheGateway;
using Logitude.CustomsMessaging.RequestParams;
using Logitude.CustomsMessaging.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Customs.BL.DummyData;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class MN_NG_8240_CargoQuery_MessageService
         : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        MN_NG_8240_CargoQuery_Message,
        MN_NG_8241_Cargo_Message,
        MN_NG_8240_CargoQuery_MessageRequestService,
        MN_NG_8241_Cargo_MessageResponseService>
    {
        

        protected override MN_NG_8241_Cargo_Message CallWS(MN_NG_8240_CargoQuery_Message customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new MN_NG_8241_Cargo_Message();

            var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();



            customRequest.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
            //myMP.MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.TestMode;



            using (var uifreightSdkGateway = new UnifreightSdkGateway(UnifreightIIGCommonUtil.GetTenantSetting().UnifreightIIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICargoQueryMessageRequestOperation>()
                    .CargoQueryMessageRequestOperation(
                    this.RequestCommunicationLogId,
                    UnifreightIIGCommonUtil.GetTenantSetting().ConsumerID,
                    customRequest,
                    ref mP,
                    out response);

            }


            return response;
        }

        public override string UnityId
        {
            get { throw new NotImplementedException(); }
        }
    }
}


