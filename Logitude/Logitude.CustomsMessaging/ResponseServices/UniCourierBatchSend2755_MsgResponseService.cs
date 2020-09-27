using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.CustomsMessaging.Utils;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSend2755_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCB2755WithResponseContentHeader, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(DCAInUCB2755WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            //var qs = new DeclarationCourierStatusQueryService(context);
            //List<DeclarationCourierStatusPM> listPM = new List<DeclarationCourierStatusPM>();
            var repo = new DeclarationCourierStatusRepository(context);
            List<DeclarationCourierStatus> listPoco = new List<DeclarationCourierStatus>();
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {

                mess.AppendLine($"מפוצל כבר !!!");


                var q =
    (from p in context.DeclarationPendings
     where p.Status == "A"
     select p
    );


                var qDeclarationPaymentPendingHold = (
                    from p in q
                    where customResponse.ServerSplitDeclarationsList.Contains(p.DeclarationID)
                
                select new MyDTO
                    {
                        DeclarationID =p.DeclarationID,
                        ErrorPlace = q.Any(r => r.DeclarationID == p.DeclarationID &&
                        p.CourierPendingReason.ErrorPlace == "1")
                    }
                         );
                var listDeclarationPaymentPendingHold = qDeclarationPaymentPendingHold.ToList();

                List<DeclarationCourierStatus> ServerSplitDeclarationsList
                    = repo.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);
                CreateCRS2755WithoutPending_UpdatePayment2InProgress(customResponse, requestParams, mess, objectTableId, objectTableIdCourierMaster, ServerSplitDeclarationsList, listDeclarationPaymentPendingHold);
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
                    mess.AppendLine("GetByMasterIDCourierPaymentStatusCode(R, L)");
                    listPoco = repo.GetByMasterIDCourierPaymentStatusCode(requestParams.Tenant, requestParams.AppicationId, "R", "L");
                }


                listPoco = listPoco.Where(r => r.FastIndividualProcessCode == "F").ToList();

                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations (LOW Val.) 'R'eady to (DEc.Payment) send  for master {requestParams.AppicationId} ");
                }
                else
                {
                    listPoco.Select(r => r.DeclarationId).ToList().ChunkBy(100)
    .ForEach(list100 =>
    {
        customResponse.ServerSplitDeclarationsList = list100;
        customResponse.LoggingUserId = requestParams.LoggingUserId;
        //CreateDCAInUCB1170_MsgMessagingService(customResponse, requestParams);
        var CreateDCAInUCB2755_MsgMessagingService = new CRSUtil();
        CreateDCAInUCB2755_MsgMessagingService
        .CreateCRS_DCAIn<DCAInUCB2755WithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));

    });
                }
            }
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            //this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            ///LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        private static void CreateCRS2755WithoutPending_UpdatePayment2InProgress(DCAInUCB2755WithResponseContentHeader customResponse, GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationCourierStatus> listPoco, List<MyDTO> listDeclarationPaymentPendingHold)
        {
            string UnifreightListOnServerOnly_BankeId = SetBankIdInUnifreightListOnServerOnly(customResponse);
            

            var realUpdatedList = new List<string>();
            foreach (var itemPoco in listPoco)
            {


                // If  ErrorPlace.CourierPendingReasons == 1 Display error Message 
                //"קיים Pending עם עצירה בתשלום הצהרה"


                //if (!String.IsNullOrWhiteSpace(itemPM.CourierPendingReasonCode))
                //if (!String.IsNullOrWhiteSpace(itemPM.CourierPendingReasonErrorPlace))

                var holdUrHorses=listDeclarationPaymentPendingHold.FirstOrDefault(r => r.DeclarationID == itemPoco.DeclarationId);
                if (holdUrHorses!=null && holdUrHorses.ErrorPlace)
                {
                    mess.AppendLine($" קיים Pending " +
                        $"עם עצירה בתשלום הצהרה ({itemPoco.DeclarationId})");
                    continue;
                }
                
                
                try
                {
                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                    {
                        var requestParams2755 = new GenericRequestParams()
                        {
                            Tenant = requestParams.Tenant,
                            //IsFakeResponse = true,
                            //RequestName = requestName,
                            //ResponseName = responseName,
                            LoggingEnabled = true,
                            LoggingObjectTableId = objectTableId,
                            LoggingEntityId = itemPoco.DeclarationId,
                            LoggingObjectTableId2 = requestParams.LoggingObjectTableId,
                            LoggingEntityId2 = objectTableIdCourierMaster,
                            AppicationId = itemPoco.DeclarationId,
                            InterfaceTypeCode = "2755",
                            //LoggingEntityReference = declarationNumber,
                            LoggingUserId = requestParams.LoggingUserId,
                            RequestVIA = SendRequestVIA.WebServiceBatch,
                            UnifreightListOnServerOnly = UnifreightListOnServerOnly_BankeId,

                        };

                        SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2755, false);
                        LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPoco.DeclarationId})");
                        mess.AppendLine($" CreateSheetSBQMessage({itemPoco.DeclarationId})");

                        string updateSql = $"Update DeclarationCourierStatuses set COURIERPAYMENTSTATUSCODE='I' where DECLARATIONID = '{itemPoco.DeclarationId}' ";
                        CustomContext.CommandExecuteNonQuery(requestParams.Tenant, updateSql);

                        scopeNewCRS.Complete();
                    }
                    

                    realUpdatedList.Add(itemPoco.DeclarationId);

                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPoco.DeclarationId}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPoco.DeclarationId}) : {ee1.Message}");
                }
            }

    //        realUpdatedList.ChunkBy(100)
    //.ForEach(list100 =>
    //{
    //    string inList = String.Join(",", list100.Select(declarationId => $"'{declarationId}'").ToArray());
    //    string updateSql = $"Update DeclarationCourierStatuses set COURIERPAYMENTSTATUSCODE='I' where DECLARATIONID in ({inList}) ";

    //    CustomContext.CommandExecuteNonQuery(requestParams.Tenant, updateSql);
    //});
        }

        private static List<CourierPendingReason> GetAllCourierPendingReason(GenericRequestParams requestParams)
        {
            var repoCourierPendingReasonRepository = new CourierPendingReasonRepository(requestParams.Tenant);
            var allCourierPendingReason = repoCourierPendingReasonRepository.GetAll(requestParams.Tenant).ToList();
            return allCourierPendingReason;
        }

        private static string SetBankIdInUnifreightListOnServerOnly(DCAInUCB2755WithResponseContentHeader customResponse)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("InternalBankId", customResponse.InternalBankId);
            var UnifreightListOnServerOnly = UnifreightListsUtil.Serialize(dic);
            return UnifreightListOnServerOnly;
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB2755WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }


    }
    class MyDTO
    {
        public string DeclarationID { get; internal set; }
        public bool ErrorPlace { get; internal set; }
    }
}
