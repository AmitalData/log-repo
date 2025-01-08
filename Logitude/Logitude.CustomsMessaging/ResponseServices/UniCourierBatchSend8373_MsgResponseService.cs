
using Microsoft.Practices.Unity;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure.Helpers;

 
using Devart.Data.Oracle;
using Simplog.Data.InfrastructureModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
//using System.Data.OracleClient;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
 
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

            var objectTableId =ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            //var qs = new DeclarationCourierStatusQueryService(context);
            var repo = new DeclarationCourierStatusRepository(context);
            List<DeclarationCourierStatus> listPoco = new List<DeclarationCourierStatus>();
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {

                mess.AppendLine($"מפוצל כבר !!!");

                List<DeclarationCourierStatus> ServerSplitDeclarationsList
                    = repo.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);
                Create8373_InProgress(requestParams, mess, objectTableId, objectTableIdCourierMaster, ServerSplitDeclarationsList, customResponse.CourierMasterId);
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");
                if (customResponse.ClientFilterDeclarationsList != null && customResponse.ClientFilterDeclarationsList.Count > 0)
                {
                    mess.AppendLine($"סומנו בצד הלקוח ");

                    listPoco = repo.GetDeclarationsByIds(customResponse.ClientFilterDeclarationsList, requestParams.Tenant);
                }
                else
                {
                    mess.AppendLine($"GetBy");
                    listPoco = repo.GetBy(customResponse.tenant, customResponse.CourierMasterId).ToList();
                }
                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"אין הצהרות בטיסה  {requestParams.AppicationId} ");
                }
                else
                {
                    if (customResponse.CourierDeclarationStatusCode == "R")
                    {
                        listPoco = listPoco.Where(r => r.CourierDeclarationStatusCode == "X").ToList();
                    }
                    if (listPoco.Count == 0)
                    {
                        mess.AppendLine($"אין תיקים  {requestParams.AppicationId} ");
                    }
                    else
                    {
                        listPoco.Select(r=>r.DeclarationId).ToList().ChunkBy(100).ForEach(list100 =>
                        {
                            customResponse.ServerSplitDeclarationsList = list100;
                            customResponse.LoggingUserId = requestParams.LoggingUserId;

                            var CreateDCAInUCB8373_MsgMessagingService = new CRSUtil();
                            CreateDCAInUCB8373_MsgMessagingService
                            .CreateCRS_DCAIn<DCAInUCB8373WithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);
                        });
                    }
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }


        private static void Create8373_InProgress(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationCourierStatus> listPM, string courierMasterId)
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

        private static void Create8373(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, DeclarationCourierStatus itemPM, bool isEffectiveFlight)
        {
           

            var requestParams8373 = new MANIFESTRequestRequestParams()
            {
                Tenant = requestParams.Tenant,
                //IsFakeResponse = true,
                //RequestName = requestName,
                //ResponseName = responseName,
                LoggingEnabled = true,
                LoggingObjectTableId = objectTableId,
                LoggingEntityId = itemPM.DeclarationId,
                LoggingObjectTableId2 = requestParams.LoggingObjectTableId,
                LoggingEntityId2 = objectTableIdCourierMaster,
                InterfaceTypeCode = "8373",
                LoggingUserId = requestParams.LoggingUserId,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                DeclarationId = itemPM.DeclarationId,
                LoggingEntityReference = itemPM.DeclarationId,
                ParentId = requestParams.CustomsRequestsSheetId,
            };
            if (isEffectiveFlight)
            {
                requestParams8373.TenantPriority = 98;
            }

            SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams8373, false);
            LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
            mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB8373WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        

    }
}
