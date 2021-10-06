using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSend8250_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCB8250WithResponseContentHeader, GenericRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB8250WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCB8250WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            
            //var repo = new DeclarationCourierStatusQueryService(context);
            var repo = new DeclarationCourierStatusRepository(context);
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {

                mess.AppendLine($"מפוצל כבר !!!");

                List<DeclarationCourierStatus> ServerSplitDeclarationsList
                    = repo.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);
                CreateCRS8250(requestParams, mess, myDeclarationQueryService, objectTableId, objectTableIdCourierMaster, ServerSplitDeclarationsList, customResponse.TesterSendOption);
            }
            else
            {
                List<DeclarationCourierStatus> listPoco = repo.GetByMasterIDDeclarationCourierStatus(requestParams.Tenant, requestParams.AppicationId);

                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");


                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Declaration) send  for master {requestParams.AppicationId} ");
                }

                listPoco.Select(r => r.DeclarationId).ToList().ChunkBy(100)
   .ForEach(list100 =>
   {
       customResponse.ServerSplitDeclarationsList = list100;
       customResponse.LoggingUserId = requestParams.LoggingUserId;
        //CreateDCAInUCB1170_MsgMessagingService(customResponse, requestParams);
        var CreateDCAInUCB1170_MsgMessagingService = new CRSUtil();
       CreateDCAInUCB1170_MsgMessagingService
       .CreateCRS_DCAIn<DCAInUCB8250WithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));

   });
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        private static void CreateCRS8250(GenericRequestParams requestParams, StringBuilder mess, DeclarationQueryService myDeclarationQueryService, string objectTableId, string objectTableIdCourierMaster, List<DeclarationCourierStatus> listPoco, string testerSendOption)
        {
            var s =string.Join(",", listPoco.Select(x => x.DeclarationId));
            
                try
                {
                    //DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(itemPoco.DeclarationId, false, false);
                    //if (declarationPM != null && !string.IsNullOrEmpty(declarationPM.DeclarationNumber))
                    //{

                        using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                        {
                            var requestParams8250 = new DeclarationStatusRequestParams()
                            {
                                Tenant = requestParams.Tenant,
                                LoggingEnabled = true,
                                LoggingObjectTableId = objectTableIdCourierMaster,
                                LoggingEntityId =  requestParams.LoggingEntityId,
                                LoggingObjectTableId2 = objectTableId,
                                LoggingEntityId2 = s,
                                InterfaceTypeCode = "8250",
                                //LoggingEntityReference = declarationPM.DeclarationNumber,
                                LoggingUserId = requestParams.LoggingUserId,
                                RequestVIA = SendRequestVIA.WebServiceBatch,
                                //DeclarationNumber = declarationPM.DeclarationNumber,
                                DeclarationRadio = true,
                                TesterSendOption= testerSendOption,
                                
                            };

                            SBQMessageService.CreateSheetSBQMessage<DeclarationStatusRequestParams>(requestParams8250, false);
                            LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({s})");
                            mess.AppendLine($" CreateSheetSBQMessage({s})");

                            scopeNewCRS.Complete();
                        }
                    //}

                }
                catch (System.Exception ee1)
                {
                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({s}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({s}) : {ee1.Message}");
                }
            
        }
    }
}
