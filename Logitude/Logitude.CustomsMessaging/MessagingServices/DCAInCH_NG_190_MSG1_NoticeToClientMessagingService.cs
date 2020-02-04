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
using UnifreightIIG.Common.MessageLib.PhysicalCheck190;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInCH_NG_190_MSG1_NoticeToClientMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        CH_NG_190_MSG1_NoticeToClient,
        DCAInCustomRequestService,
        CH_NG_190_MSG1_NoticeToClientResponseService, DCAInRequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "190"; }
        }
        protected override CH_NG_190_MSG1_NoticeToClient GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {

            return new CH_NG_190_MSG1_NoticeToClient()
            {
                RequestContentHeader = new RequestContentHeader()
                {
                    TransmitionDateTime = DateTime.Now
                },
                NoticeToClient = new CH_NG_190_MSG1_NoticeToClientNoticeToClient()
                {
                    operationCode = 1,
                    statusMessage = 2,
                    checkId = 2369229,
                    entityType = 5,
                    customsAgent = 1111,
                    importerNumber = 111,
                    storageSiteNumber = "ILMMN",
                    checkSiteNumber = "10470",
                    openDate = DateTime.Now,
                    CheckType = 1,
                    declarationID = requestParamsData.AppicationId,///change to number 


                },
                CheckEntity = new CH_NG_190_MSG1_NoticeToClientCheckEntity()
                {
                    cargoIdentifier = new cargoIdentifier()
                    {
                        cargoIdentifierKey1 = "22",
                        cargoIdentifierType = 1
                    }

                },
                SplitCargoIdentifier = new CH_NG_190_MSG1_NoticeToClientSplitCargoIdentifier[]{
                      new CH_NG_190_MSG1_NoticeToClientSplitCargoIdentifier()
                  {
                       cargoIdentifier= new cargoIdentifier()
                       {
                            cargoIdentifierType= 27 ,
                             cargoIdentifierKey1= "50497355"
                       }
                  }
                  }

            };
        }
        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(CH_NG_190_MSG1_NoticeToClient customsResponse)
        {
            var tableName="Customs.PhysicalCheck";
            //ResolveTenant() ==CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = customsResponse.NoticeToClient.checkId.ToString()
            };
            myGenericRequestParams.RequestName = @" זימון לבדיקה" + customsResponse.NoticeToClient.checkId.ToString();
            return myGenericRequestParams;

        }
        protected override CH_NG_190_MSG1_NoticeToClient CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
        protected override DcaReceivedController GetDcaReceivedController(CH_NG_190_MSG1_NoticeToClient customsResponse, GenericRequestParams RequestParams)
        {
            if (customsResponse== null)
            {
                return null;
            }
            if (customsResponse.NoticeToClient== null)
            {
                return null;
            }
            if (customsResponse.NoticeToClient.checkId==null)
            {
                return null;
            }
            return new DcaReceivedController() { DcaAnalyzeAggregateKey = customsResponse.NoticeToClient.checkId.ToString() };
        }

        /*protected override RequestSheetParam GetSheetDetailsFromRequestParam(GenericRequestParams requestParams)
        {
            var myRequestSheetParam = new Logitude.CustomsMessaging.Common.RequestParams.RequestSheetParam();
            myRequestSheetParam.RequestDescription = "זימון לבדיקה " + requestParams.AppicationId;
            myRequestSheetParam.EntityId1 = requestParams.AppicationId;
            myRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.PhysicalCheck");
            return myRequestSheetParam;
        }*/
    }
}
