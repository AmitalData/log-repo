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
using UnifreightIIG.Common.MessageLib.DeclarationDeal;
using UnifreightIIG.Common.MessageLib.Deficit;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDE_NG_280_MSG11_DebtNotificationMessageMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        DE_NG_280_MSG11_DebtNotificationMessage,
        DCAInCustomRequestService,
        DE_NG_280_MSG11_DebtNotificationMessageResponseService, DCAInRequestHeader>
    {
        public override string MainInterfaceCode
         {
             get { return "280"; }
         }


        protected override DcaReceivedController GetDcaReceivedController(DE_NG_280_MSG11_DebtNotificationMessage customsResponse, GenericRequestParams RequestParams)
        {
            if (customsResponse == null)
            {
                return null;
            }
            if (customsResponse.DebtNotificationMessag == null)
            {
                return null;
            }
            
            return new DcaReceivedController() { DcaAnalyzeAggregateKey =
                 MainInterfaceCode +":" +customsResponse.DebtNotificationMessag.debtNotificationID.ToString() };
        }
        protected override DE_NG_280_MSG11_DebtNotificationMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
         {
             

             throw new NotImplementedException();
         }

         protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DE_NG_280_MSG11_DebtNotificationMessage customsResponse)
         {
             var tableName = "Customs.Deficit";

             var myGenericRequestParams = new GenericRequestParams()
             {
                 LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                 //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
             };
             return myGenericRequestParams;
         }

        /*protected override RequestSheetParam GetSheetDetailsFromRequestParam(GenericRequestParams requestParams)
        {
            var myRequestSheetParam = new Logitude.CustomsMessaging.Common.RequestParams.RequestSheetParam();
            myRequestSheetParam.RequestDescription = "הודעת חיוב " + requestParams.AppicationId;
            myRequestSheetParam.EntityId1 = requestParams.AppicationId;
            myRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.Deficit");
            return myRequestSheetParam;
        }*/
    }
}
