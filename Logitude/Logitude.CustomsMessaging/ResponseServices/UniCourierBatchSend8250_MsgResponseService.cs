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
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;

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
            FeatureQuery featureQuery = new FeatureQuery();

            var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);

            var feature = features.Features.FirstOrDefault(x => x.Code == "StatusDeclarationOldVersion");
            if (feature != null && !customResponse.IsWorkSheetFromExcel)
            {
                UpdateRealOld(customResponse, requestParams);

            }
            else
            {
                UpdateReal(customResponse, requestParams);

            }

        }

        public  void UpdateReal(DCAInUCB8250WithResponseContentHeader customResponse, GenericRequestParams requestParams)
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

                /*List<DeclarationCourierStatus> ServerSplitDeclarationsList
                    = repo.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);*/

                CreateCRS8250(requestParams, mess, myDeclarationQueryService, objectTableId, objectTableIdCourierMaster, customResponse);
            }
            else
            {
                //List<DeclarationCourierStatus> listPoco = repo.GetByMasterIDDeclarationCourierStatus(requestParams.Tenant, requestParams.AppicationId);
                string MAWB = "";
                List<string> listPoco;
                if (customResponse.IsWorkSheetFromExcel) {
                    listPoco = repo.GetFromExcelDeclarationList(requestParams.Tenant, customResponse.WorkSheeetLogUser);
                }
                else
                {
                    listPoco = repo.GetByMasterIDDeclarationList(requestParams.Tenant, requestParams.AppicationId, out MAWB);
                }

                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");


                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Declaration) send  for master {requestParams.AppicationId} ");
                }

                listPoco.ChunkBy(100)
   .ForEach(list100 =>
   {
       customResponse.ServerSplitDeclarationsList = list100;
       customResponse.MyMoreParams = MAWB;
       customResponse.LoggingUserId = requestParams.LoggingUserId;
        //CreateDCAInUCB1170_MsgMessagingService(customResponse, requestParams);
        var CreateDCAInUCB1170_MsgMessagingService = new CRSUtil();
       CreateDCAInUCB1170_MsgMessagingService
       .CreateCRS_DCAIn<DCAInUCB8250WithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);

   });
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        private static void CreateCRS8250(GenericRequestParams requestParams, StringBuilder mess, DeclarationQueryService myDeclarationQueryService, string objectTableId, string objectTableIdCourierMaster, DCAInUCB8250WithResponseContentHeader customResponse)
        {
            var declarations =string.Join(",", customResponse.ServerSplitDeclarationsList);

            try
            {
                var declarationPMs = myDeclarationQueryService.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);
                string[] list = { "12", "7", "8" };
                var newServerSplitDeclarationsList = declarationPMs.Where(d => !list.Contains(d.DeclarationStatusTypeCode)).Select(d => d.Id).ToList();
                LogMessagingUtil.Instance.AppendLine($"ignore status: 12, 7, 8");

                if (newServerSplitDeclarationsList != null && newServerSplitDeclarationsList.Count > 0)
                {
                    declarations = string.Join(",", newServerSplitDeclarationsList);

                    //if (declarationPM != null && !string.IsNullOrEmpty(declarationPM.DeclarationNumber) && !list.Contains(declarationPM.DeclarationStatusTypeCode))
                    //{

                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                    {
                        var requestParams8250 = new DeclarationStatusRequestParams()
                        {
                            Tenant = requestParams.Tenant,
                            LoggingEnabled = true,
                            LoggingObjectTableId = objectTableIdCourierMaster,
                            LoggingEntityId = requestParams.LoggingEntityId,
                            LoggingObjectTableId2 = objectTableId,
                            LoggingEntityId2 = "_" + newServerSplitDeclarationsList.FirstOrDefault(),
                            DeclarationList = declarations,
                            InterfaceTypeCode = "8250",
                            //LoggingEntityReference = declarationPM.DeclarationNumber,
                            LoggingUserId = requestParams.LoggingUserId,
                            RequestVIA = SendRequestVIA.WebServiceBatch,
                            //DeclarationNumber = declarationPM.DeclarationNumber,
                            DeclarationRadio = true,
                            TesterSendOption = customResponse.TesterSendOption,
                            CourierMaster = customResponse.MyMoreParams,
                            ParentId = requestParams.CustomsRequestsSheetId,
                        };

                        SBQMessageService.CreateSheetSBQMessage<DeclarationStatusRequestParams>(requestParams8250, false);
                        LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({declarations})");
                        mess.AppendLine($" CreateSheetSBQMessage({declarations})");

                        scopeNewCRS.Complete();
                    }
                    //}
                }
            }
            catch (System.Exception ee1)
            {
                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({declarations}) : {ee1.Message}");
                mess.AppendLine($"Exception!!!CreateSheetSBQMessage({declarations}) : {ee1.Message}");
            }
            
        }

        public   void UpdateRealOld(DCAInUCB8250WithResponseContentHeader customResponse, GenericRequestParams requestParams)
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
                CreateCRS8250Old(requestParams, mess, myDeclarationQueryService, objectTableId, objectTableIdCourierMaster, ServerSplitDeclarationsList, customResponse.TesterSendOption);
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
       .CreateCRS_DCAIn<DCAInUCB8250WithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);

   });
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        private static void CreateCRS8250Old(GenericRequestParams requestParams, StringBuilder mess, DeclarationQueryService myDeclarationQueryService, string objectTableId, string objectTableIdCourierMaster, List<DeclarationCourierStatus> listPoco, string testerSendOption)
        {
            foreach (DeclarationCourierStatus itemPoco in listPoco)
            {
                try
                {
                    DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(itemPoco.DeclarationId, false, false);
                    string[] list = { "12", "7", "8" };
                    if (declarationPM != null && !string.IsNullOrEmpty(declarationPM.DeclarationNumber) && !list.Contains(declarationPM.DeclarationStatusTypeCode))
                    {

                        using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                        {
                            var requestParams8250 = new DeclarationStatusRequestParams()
                            {
                                Tenant = requestParams.Tenant,
                                LoggingEnabled = true,
                                LoggingObjectTableId = objectTableId,
                                LoggingEntityId = itemPoco.DeclarationId,
                                LoggingObjectTableId2 = objectTableIdCourierMaster,
                                LoggingEntityId2 = requestParams.LoggingEntityId,
                                InterfaceTypeCode = "8250",
                                LoggingEntityReference = declarationPM.DeclarationNumber,
                                LoggingUserId = requestParams.LoggingUserId,
                                RequestVIA = SendRequestVIA.WebServiceBatch,
                                DeclarationNumber = declarationPM.DeclarationNumber,
                                DeclarationRadio = true,
                                TesterSendOption = testerSendOption,

                            };

                            SBQMessageService.CreateSheetSBQMessage<DeclarationStatusRequestParams>(requestParams8250, false);
                            LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPoco.DeclarationId})");
                            mess.AppendLine($" CreateSheetSBQMessage({itemPoco.DeclarationId})");

                            scopeNewCRS.Complete();
                        }
                    }

                }
                catch (System.Exception ee1)
                {
                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPoco.DeclarationId}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPoco.DeclarationId}) : {ee1.Message}");
                }
            }
        }
    }
}
