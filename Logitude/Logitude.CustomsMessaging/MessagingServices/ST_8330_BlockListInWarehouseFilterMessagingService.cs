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
using UnifreightIIG.Common.BlockListInWarehouseServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class ST_8330_BlockListInWarehouseFilterMessagingService : MessagingServiceBase<
        BlockListInWarehouseRequestParams,
        BlockListInWarehouseResponseData,
        ST_8330_Web01_BlockListInWarehouseFilterParam,
        ST_8331_Web02_BlockListInWarehouseDetail,
        ST_8330_BlockListInWarehouseFilterRequestService,
        ST_8331_BlockListInWarehouseDetailResponseService, RequestHeader>
    {

        protected override ST_8331_Web02_BlockListInWarehouseDetail CallWS(ST_8330_Web01_BlockListInWarehouseFilterParam customRequest, BlockListInWarehouseRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new ST_8331_Web02_BlockListInWarehouseDetail();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IBlockListInWarehouseOperation>()
                    .BlockListInWarehouseQuery(
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
            get { return "8330"; }
        }

        protected override RequestSheetParam GetSheetDetailsFromRequestParam(BlockListInWarehouseRequestParams requestParams)
        {
            var myRequestSheetParam = new RequestSheetParam();
            myRequestSheetParam.RequestDescription = "שאילתת גושים במחסן";
            //myRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.Deficit"); to check? 

            return myRequestSheetParam;
        }
    }
}
