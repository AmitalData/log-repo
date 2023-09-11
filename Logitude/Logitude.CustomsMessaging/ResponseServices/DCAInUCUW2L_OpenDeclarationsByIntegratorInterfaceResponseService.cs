
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
using Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments;
using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools.Utils;
using System.Configuration;
using Logitude.CustomsMessaging.U2L.CommDec;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Server.Tools.Models;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.CustomsMessaging.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
///using Logitude.Customs.BL.Messaging.U2L.CommDec;
using Logitude.Server.Tools.Contracts;
using System.Diagnostics;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCUW2LResponseContentHeader, DCAInUCUW2LRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(DCAInUCUW2LResponseContentHeader customResponse, DCAInUCUW2LRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCUW2LResponseContentHeader customResponse, DCAInUCUW2LRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();


            bool lockit = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Singleton.CRS:UCUW2L"));
            // string key = ProcessLockTableUtil.Instance.GetKey4DocumentsFilingId(customResponse.CustomFileNo, requestParams.Tenant);

            // using (var processLockTableDisposable = ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, lockit, key, "CRS:UCUW2L"))
            // {
            string customFileNo = "";

            //Do_CommDecService CommDecService = new Do_CommDecService();
            CommDecService CommDecService = new CommDecService();
            try
            {
                string error = "";
                string decId = "";
                string courierMasterID = "";
                string moreParams = customResponse.MoreParams;
                CommDecService.ProccessGenericRequestReal(customResponse.LOGICOMMDEC, requestParams.Tenant, requestParams.LoggingUserId, requestParams.PBId, ref moreParams, out error, out customFileNo, out decId, out courierMasterID);
                
                if (error != "")
                {
                    
                    this.MyResponseData.ApplicationID = customFileNo;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.UserMessage = error;
                    throw new BusinessErrorException(error);

                }
                if (!String.IsNullOrWhiteSpace(decId))
                {

                    var myConnectDocumentsfilingService = new ConnectDocumentsfilingService();
                    myConnectDocumentsfilingService.Connect(decId, customFileNo, requestParams);

                }

                var context = CustomContext.GetContext(requestParams.Tenant);


                var updateOpenDeclarationInCourierMasterService = new UpdateOpenDeclarationInCourierMasterService();
                updateOpenDeclarationInCourierMasterService.UpdateOpenDeclarationInCourierMaster(requestParams.Tenant, courierMasterID, requestParams.LoggingUserId);

                this.MyResponseData.ApplicationID = customFileNo;
                this.MyResponseData.HasException = false;
                this.MyResponseData.Succeeded = true;
                if(!string.IsNullOrEmpty(CommDecService.MyGenericResponseObj.ErrorDescription))
                    this.MyResponseData.UserMessage = CommDecService.MyGenericResponseObj.ErrorDescription;


                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.CustomFileNo = customFileNo;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = decId;
                this.MyRequestSheetParam.RequestDescription = CommDecService.MyGenericResponseObj.EnglishDescription;

                //   this.MyRequestSheetParam.RequestDescription = "הצהרה נפתחה בהצלחה :" + customFileNo + "_" + decId;


            }
            catch (Exception ex)
            {
                this.MyResponseData.ApplicationID = customFileNo;
                this.MyResponseData.HasException = true;
                this.MyResponseData.Succeeded = false;
                //this.MyResponseData.UserMessage = ex.Message;
                //throw new Exception(ex.Message);

                this.MyResponseData.UserMessage = ex.ToString();
                if(this.MyResponseData.UserMessage.Contains("effective flight"))
                {
                    throw new BusinessErrorException(ex.ToString());

                }
                throw;
            }
            // }
        }






    }
    public class ConnectDocumentsfilingService
    {
        private ICommonDataContext _DataContext;
        private DocumentsFilingQuery _documentsFilingQuery;

        public void Connect(string DeclarationId, string customFileNo, GenericRequestParams requestParams)
        {
            if (String.IsNullOrWhiteSpace(DeclarationId) || string.IsNullOrWhiteSpace(customFileNo))
            {
                LogMessagingUtil.Instance.AppendLine("ConnectDocumentsfilingService:" + DeclarationId + customFileNo);
            }

            _DataContext = CommonDataContext.GetContext(requestParams.Tenant);
            //UpdatePaymentDocument(declarationId, requestParams.Tenant, requestParams.LoggingUserId);
            var sw = Stopwatch.StartNew();
            //check
            LogMessagingUtil.Instance.AppendLine($"!!!!B4:GetByexternalentityreference");
            _documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            var documentsFilingIdsList = _documentsFilingQuery.GetByexternalentityreference("CFIFILEM", customFileNo, requestParams.Tenant).ToList();
            LogMessagingUtil.Instance.AppendLine($"!!!!after:GetByexternalentityreference {sw.ElapsedMilliseconds}");sw.Restart();
            var documentsFilingIds = documentsFilingIdsList
            .Where(r => r.EntityId == null || r.EntityId.Trim() == string.Empty)
            .Select(r => r.Id).ToList();
            if (documentsFilingIds.Count==0)
            {
                LogMessagingUtil.Instance.AppendLine($"ConnectDocumentsfilingService.Connect:GetByexternalentityreference:{customFileNo}.Where(r => r.EntityId == null || r.EntityId.Trim() == string.Empty) not found any !!");

                documentsFilingIds = GetByCourierRef(DeclarationId, requestParams.Tenant);

            }
            foreach (string documentsFilingId in documentsFilingIds)
            {
                UpdatePaymentDocument(documentsFilingId, DeclarationId, requestParams.Tenant, requestParams.LoggingUserId);
            }




        }

        private static List<string> GetByCourierRef(string DeclarationId, int Tenant)
        {
            List<string> documentsFilingIds = new List<string>();
            var sw = Stopwatch.StartNew();
            var decRepo = new DeclarationRepository(Tenant);
            string courierhawb = decRepo.GetCourierhawbFromId(DeclarationId, Tenant);
            if (!string.IsNullOrWhiteSpace(courierhawb))
            {
                var gDMREFRepository = new GDMREFRepository(Tenant);
                //gDMREFRepository
                const string CARFI = "CARFI";
                //courierhawb
                var q = gDMREFRepository.GetByRef(REFID: CARFI, REFERENCE: courierhawb).Select(r => r.COMID);
                documentsFilingIds = q.ToList();
                string remark = $"Took:{sw.ElapsedMilliseconds};gDMREFWhere(REFID == CARFI&REFERENCE == {courierhawb}).count={documentsFilingIds.Count}";
                LogMessagingUtil.Instance.AppendLine(remark);

            }

            return documentsFilingIds;
        }

        private void UpdatePaymentDocument(string documentsFilingId, string DeclarationId, int Tenant, string LoggedUserId)
        {

            var documentsFilingService = new UnifreightDocumentsFilingService(_DataContext, Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "" });
            //var documentTypeQuery = new DocumentTypeQuery(Tenant);


            //var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("POR", _PaymentOrderPM.Tenant);
            DocumentsFilingPM documentsFilingPM = _documentsFilingQuery.GetSinglePM(documentsFilingId, Tenant);
            if (documentsFilingPM != null && documentsFilingPM.EntityId != DeclarationId)
            {
                documentsFilingPM.EntityId = DeclarationId;
                documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                //documentsFilingPM.ChildEntityId = _PaymentOrderPM.Id;
                //documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                //documentsFilingPM.ExternalEntityReference = _PaymentOrderPM.AccountingCustomFile;
                documentsFilingPM.IsHybrid = true;//this is as substituteto hybrid !!!!
                documentsFilingService.Update(documentsFilingPM, null, LoggedUserId);
                LogMessagingUtil.Instance.AppendLine("UpdatePaymentDocument:Connect  document " + documentsFilingPM.Code + " to DeclarationId:" + DeclarationId);
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine($"UpdatePaymentDocument:GetSinglePM(documentsFilingId)-bad:{documentsFilingPM?.EntityId}");
            }
        }
    }


    public class UpdateOpenDeclarationInCourierMasterService : IUpdateOpenDeclarationInCourierMasterService
    {
        private void AppendLogLine(string mess)
        {
            LogMessagingUtil.Instance.AppendLine(mess);
        }
        public static string GetGeneralLockKey(string courierMasterId)
        {
            return $"UCUDO:{courierMasterId}";
        }
        public void UpdateOpenDeclarationInCourierMaster(/*DCAInUCUW2LRequestParams requestParams*/int Tenant, string courierMasterID, string LoggingUserId)
        {
            if (string.IsNullOrWhiteSpace(courierMasterID))
            {
                return;
            }
            var context = CustomContext.GetContext(Tenant);
            CustomsRequestsSheetQueryService customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(context);
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            List<CustomsRequestsSheetPM> customsRequestsSheetPMList = customsRequestsSheetQueryService
                //.GetRequestInProgress(requestParams.Tenant, "UCUDO", "", "", null, null, courierMasterID, true);
                .GetRequestInProgress(Tenant, "UCUDO", objectTableId, courierMasterID, null, null, null, true);

            if (customsRequestsSheetPMList == null || customsRequestsSheetPMList.Count == 0)
            {
                //customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(requestParams.Tenant, "UCUW2L", "", "", null, null, courierMasterID, true);
                //customsRequestsSheetPMList = customsRequestsSheetPMList.Where(x => x.Id != requestParams.PBId).ToList();
                //if (customsRequestsSheetPMList == null || customsRequestsSheetPMList.Count == 0 )
                {

                    AppendLogLine("open UCUDO  ??");

                    string GeneralKey = GetGeneralLockKey(courierMasterID);
                    var concurrentKiller = new ConcurrentKiller();

                    bool haveUCUDOInProgress = false;
                    try
                    {

                        using (var scope = TransactionFactory.GetNewTransaction())//open new Transaction  because if not the Transaction  wil invalidad
                        {
                            concurrentKiller.FreeLockIfCreated15MinOld(GeneralKey, Tenant);
                            concurrentKiller.LockOrCrashOnCommitDueUnique(GeneralKey, Tenant);
                            haveUCUDOInProgress = false;
                            AppendLogLine("UCUDO:concurrentKiller: Ok");
                            scope.Complete();
                        }
                    }
                    catch (Exception)
                    {
                        AppendLogLine("UCUDO:concurrentKiller:Have in the middle in the last 15 min- not open  UCUDO");
                        haveUCUDOInProgress = true;
                    }


                    var messagingService = new DCAInUCUDO_UpdateOpenDeclarationsMessagingService();
                    UpdateOpenDeclarationsRequestParams requestParams2 = new UpdateOpenDeclarationsRequestParams()
                    {

                        LoggingUserId = LoggingUserId,
                        Tenant = Tenant,
                        LoggingEntityId = courierMasterID,

                    };
                    if (!haveUCUDOInProgress)
                    {
                        string message = messagingService.CreateCRS(Tenant, LoggingUserId, requestParams2);
                        AppendLogLine($"UCUDO  opened {message}");

                    }
                }
            }
        }

    }
}
