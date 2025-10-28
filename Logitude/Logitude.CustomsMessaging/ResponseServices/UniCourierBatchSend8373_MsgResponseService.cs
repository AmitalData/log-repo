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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityListQueryServices;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSend8373_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCB8373WithResponseContentHeader, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(DCAInUCB8373WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {

            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            DeclarationCourierStatusListQueryService declarationCourierStatusQuery = new DeclarationCourierStatusListQueryService(context);
            IQueryable<DeclarationCourierStatusList> declarationQuery = Enumerable.Empty<DeclarationCourierStatusList>().AsQueryable();
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {
                mess.AppendLine($"מפוצל כבר !!!");
                List<DeclarationCourierStatusList> ServerSplitDeclarationsList = declarationCourierStatusQuery.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant).ToList();
                Create8373_InProgress(requestParams, mess, objectTableId, objectTableIdCourierMaster, ServerSplitDeclarationsList, customResponse.CourierMasterId);
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");
                if (customResponse.ClientFilterDeclarationsList != null && customResponse.ClientFilterDeclarationsList.Count > 0)
                {
                    mess.AppendLine($"סומנו בצד הלקוח ");
                    declarationQuery = declarationCourierStatusQuery.GetDeclarationsByIds(customResponse.ClientFilterDeclarationsList, requestParams.Tenant);
                }
                else
                {
                    if (customResponse.IsWorkSheetFromExcel)
                    {
                        declarationQuery = declarationCourierStatusQuery.GetDeclarationStatusesByFromExcel(customResponse.tenant, customResponse.LoggingUserId);
                    }
                    else
                    {
                        mess.AppendLine($"GetByCourierMasterId");
                        declarationQuery = declarationCourierStatusQuery.GetDeclarationStatusesByCourierMaster(customResponse.tenant, customResponse.CourierMasterId);
                    }
                }
                if (!declarationQuery.Any())
                {
                    mess.AppendLine($"אין הצהרות לשליחה בטיסה  {requestParams.AppicationId} ");
                }
                else
                {
                    if (customResponse.CourierDeclarationStatusCode == "R")
                    {
                        declarationQuery = declarationQuery.Where(r => r.CourierDeclarationStatusCode == "X");
                    }
                    if (!declarationQuery.Any())
                    {
                        mess.AppendLine($"אין הצהרות לשליחה בסטטוס X  {requestParams.AppicationId} ");
                    }
                    else
                    {
                        declarationQuery.Select(r => r.DeclarationId).ToList().ChunkBy(100).ForEach(list100 =>
                        {
                            customResponse.ServerSplitDeclarationsList = list100;
                            customResponse.LoggingUserId = requestParams.LoggingUserId;
                            var createDCAInUCB8373MsgMessagingService = new CRSUtil();
                            createDCAInUCB8373MsgMessagingService.CreateCRS_DCAIn<DCAInUCB8373WithResponseContentHeader>(customResponse,(requestParams as RequestParamsBase),
                                out string list);
                        });
                    }
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }


        private static void Create8373_InProgress(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationCourierStatusList> listPM, string courierMasterId)
        {
            //if is EffectiveFlight : TenantPriority = 98
            var context = CustomContext.GetContext(requestParams.Tenant);
            CourierMasterQueryService myCourierMasterQueryService = new CourierMasterQueryService(context);
            bool isEffectiveFlight = myCourierMasterQueryService.GetSingle(courierMasterId, false, false)?.EffectiveFlight ?? false;


            var listDeclarationIdCreateCRS = new List<string>();
            foreach (var itemPM in listPM)
            {
                try
                {
                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                    {
                        Create8373(requestParams, mess, objectTableId, objectTableIdCourierMaster, itemPM, isEffectiveFlight);
                        scopeNewCRS.Complete();
                    }
                    listDeclarationIdCreateCRS.Add(itemPM.DeclarationId);


                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                }
            }
        }

        private static void Create8373(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, DeclarationCourierStatusList itemPM, bool isEffectiveFlight)
        {


            var requestParams8373 = new DeclarationRestoreRequestParams()
            {
                CustomsFile = itemPM.CustomFileNo,
                DeclarationNumber = itemPM.DeclarationNumber,
                DeclarationId = itemPM.DeclarationId,
                Tenant = itemPM.Tenant,
                LoggingEntityId = itemPM.DeclarationId,
                LoggingUserId = AuthenticationUtil.ResolveUserId(itemPM.Tenant),
                ResponseName = "9079",
                RequestVIA = SendRequestVIA.WebServiceBatch,
                InterfaceTypeCode = "8373",
                AppicationId = itemPM.DeclarationId,
                LoggingEnabled = true,
                LoggingEntityReference = "I",
                IsAngularClient = true,
                ParentId = requestParams.CustomsRequestsSheetId,
			};
            if (isEffectiveFlight)
            {
                requestParams8373.TenantPriority = 98;
            }

            SBQMessageService.CreateSheetSBQMessage<DeclarationRestoreRequestParams>(requestParams8373, false);
            LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
            mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB8373WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }


    }
}
