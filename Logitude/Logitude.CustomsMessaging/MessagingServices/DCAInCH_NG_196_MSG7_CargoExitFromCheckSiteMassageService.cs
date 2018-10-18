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
using UnifreightIIG.Common.MessageLib.PhysicalCheck;

namespace Logitude.CustomsMessaging.MessagingServices
{
    //CH_NG_196_MSG7_CargoExitFromCheckSiteResponseService
    public class DCAInCH_NG_196_MSG7_CargoExitFromCheckSiteMassageService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        CH_NG_196_MSG7_CargoExitFromCheckSite,
        DCAInCustomRequestService,
        CH_NG_196_MSG7_CargoExitFromCheckSiteResponseService, DCAInRequestHeader>
    {
        protected override CH_NG_196_MSG7_CargoExitFromCheckSite CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "196"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(CH_NG_196_MSG7_CargoExitFromCheckSite customsResponse)
        {
            var tableName = "Customs.PhysicalCheck";
            //ResolveTenant() ==CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = customsResponse.generalDetails.checkId.ToString()
            };
            return myGenericRequestParams;
        }

        /*protected override RequestSheetParam GetSheetDetailsFromRequestParam(GenericRequestParams requestParams)
        {
            var myRequestSheetParam = new Logitude.CustomsMessaging.Common.RequestParams.RequestSheetParam();
            myRequestSheetParam.RequestDescription = "שחרור מטען מאתר בדיקה " + requestParams.AppicationId;
            myRequestSheetParam.EntityId1 = requestParams.AppicationId;
            myRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.PhysicalCheck");

            return myRequestSheetParam;
        }*/
    }
}
