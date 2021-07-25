
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

            CommDecService CommDecService = new CommDecService();
                try {
                    string error = "";
                string decId = "";
                string courierMasterID = "";
                string moreParams = customResponse.MoreParams;
                    CommDecService.ProccessGenericRequestReal(customResponse.LOGICOMMDEC, requestParams.Tenant, requestParams.LoggingUserId , ref moreParams, out error, out customFileNo, out decId, out courierMasterID);

                if(error!="")
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

                CustomsRequestsSheetQueryService customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(context);
                List<CustomsRequestsSheetPM> customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(requestParams.Tenant, "UCUDO", "", "", null, null, courierMasterID, true);

                if(customsRequestsSheetPMList== null || customsRequestsSheetPMList.Count==0)
                {
                    customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(requestParams.Tenant, "UCUW2L", "", "", null, null, courierMasterID, true);
                    customsRequestsSheetPMList = customsRequestsSheetPMList.Where(x => x.Id != requestParams.PBId).ToList();
                    if (customsRequestsSheetPMList == null || customsRequestsSheetPMList.Count == 0 )
                    {

                        var messagingService = new DCAInUCUDO_UpdateOpenDeclarationsMessagingService();
                        UpdateOpenDeclarationsRequestParams requestParams2 = new UpdateOpenDeclarationsRequestParams()
                        {

                            LoggingUserId = requestParams.LoggingUserId,
                            Tenant = requestParams.Tenant,
                            LoggingEntityId = courierMasterID,

                        };

                        string message = messagingService.CreateCRS(requestParams.Tenant, requestParams.LoggingUserId, requestParams2);
                    }
                }

                this.MyResponseData.ApplicationID = customFileNo;
                     this.MyResponseData.HasException = false;
                    this.MyResponseData.Succeeded = true;


                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.CustomFileNo = customFileNo;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                 this.MyRequestSheetParam.EntityId1 = decId;
             //   this.MyRequestSheetParam.RequestDescription = "הצהרה נפתחה בהצלחה :" + customFileNo + "_" + decId;


            }
            catch (Exception ex)
                {
                    this.MyResponseData.ApplicationID = customFileNo;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.UserMessage = ex.Message;
                throw new Exception(ex.Message);
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
            _documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            var documentsFilingIds = _documentsFilingQuery.GetByexternalentityreference("CFIFILEM", customFileNo, requestParams.Tenant)
            .Where(r => r.EntityId == null || r.EntityId.Trim() == string.Empty)
            .Select(r => r.Id).ToList();
            foreach (string documentsFilingId in documentsFilingIds)
            {
                UpdatePaymentDocument(documentsFilingId,DeclarationId, requestParams.Tenant, requestParams.LoggingUserId);
            }
            
            


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
                LogMessagingUtil.Instance.AppendLine("Connect  document " + documentsFilingPM.Code + " to DeclarationId:" + DeclarationId);
            }

        }
    }
}
