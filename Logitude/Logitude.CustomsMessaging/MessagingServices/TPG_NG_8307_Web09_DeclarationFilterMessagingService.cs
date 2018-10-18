//Yuval Chalup 07.09.2015 TASK-15037
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.DeclarationFilterParamServiceReference;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TPG_NG_8307_Web09_DeclarationFilterMessagingService : MessagingServiceBase<
        DeclarationFilterRequestParams,
        DeclarationFilterResponseData,
        TPG_NG_8307_Web09_DeclarationFilterParam,
        TPG_NG_8249_Web10_TPGDeclarationDetail,
        TPG_NG_8307_Web09_DeclarationFilterRequestService,
        TPG_NG_8249_Web10_TPGDeclarationDetailResponseService, RequestHeader>
    {
        protected override TPG_NG_8249_Web10_TPGDeclarationDetail CallWS(TPG_NG_8307_Web09_DeclarationFilterParam customRequest, DeclarationFilterRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TPG_NG_8249_Web10_TPGDeclarationDetail();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IDeclarationFilterParamOPeration>()
                    .DeclarationFilterParamOPeration(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        public override string MainInterfaceCode
        {
            get { return "8307"; }
        }

    }
}

