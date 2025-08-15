using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Devart.Data.Oracle;
using Simplog.Data.Helpers;
using System.Data.SqlClient;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using System.Data.Entity;


namespace Logitude.Customs.BL.BL
{
    public class AutomatedCustomsMessagingService
    {
        private readonly bool _featureSendManifest;
        private readonly bool _featureSendDeclaration;
        private readonly bool _featureSendPayment;
        private string declarationObjectTableId;
        private string objectTableIdCourierMaster;
        private readonly string userId;
        private DateTime futureSendTime = DateTime.Now.AddMinutes(5);
        public bool sent = false;

        public AutomatedCustomsMessagingService(int tenant)
        {
            FeatureQuery featureQuery = new FeatureQuery(tenant);
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(tenant), tenant);

            _featureSendManifest = features.Features.Any(x => x.Code == "AutomatedSendManifest");
            _featureSendDeclaration = features.Features.Any(x => x.Code == "AutomatedSendDeclaration");
            _featureSendPayment = features.Features.Any(x => x.Code == "SendPaymentOn900Close");


        }
        public bool CheckAndSendMessageis(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
             sent = false;

            if (!_featureSendManifest && !_featureSendDeclaration && !_featureSendPayment)
            {
                return sent;
            }


            declarationObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");


            var courierMasterRepo = new CourierMasterRepository(declarationCourierStatusPM.Tenant);
            var declarationPendingRepo = new DeclarationPendingRepository(declarationCourierStatusPM.Tenant);

            Declaration declaration;
            using (var ctx = (CustomContext)CustomContext.GetContext(declarationCourierStatusPM.Tenant))
            {
                declaration = ctx.Declarations      
                                 .AsNoTracking()
                                 .FirstOrDefault(d => d.Id == declarationCourierStatusPM.DeclarationId && d.Tenant == declarationCourierStatusPM.Tenant);
            }

            if(declaration == null || declaration.IsAmendment == true)
            {
                return sent;
            }

            if (_featureSendManifest && declarationCourierStatusPM.CourierManifestStatusCode == "R")
            {
                SendManifest(declarationCourierStatusPM);
                return sent;
            }

            if (_featureSendDeclaration
                && declarationCourierStatusPM.CourierManifestStatusCode == "V"
                && declarationCourierStatusPM.CourierDeclarationStatusCode == "R"
                && declarationCourierStatusPM.DocumentStatusCode == "V")
            {

                bool hasActivePending = false;
                if (declaration != null)
                {
                    hasActivePending = declarationPendingRepo.HasPendingWithStatus(declaration.Id, declarationCourierStatusPM.Tenant, "A");
                }
                bool importerOk =(string.IsNullOrEmpty(declaration.ImporterCode) && (string.IsNullOrEmpty(declaration.ImporterId))
                    || !string.IsNullOrEmpty(declaration.ImporterId));

                if (declaration != null && !hasActivePending && importerOk )
                {
                    SendDeclaration(declarationCourierStatusPM);
                    return sent;
                }
            }

            if (_featureSendPayment)
            {
                var courierMaster = courierMasterRepo.GetCourierMasterByDeclarationId(
                    declarationCourierStatusPM.DeclarationId,
                    declarationCourierStatusPM.Tenant
                );

                if (courierMaster == null || courierMaster.CourierMasterPaymentStatusCd != "1")
                {
                    return sent;
                }


                if (declarationCourierStatusPM.CourierDeclarationStatusCode == "V")
                {
                    var pendings = declarationPendingRepo
                        .GetDeclarationPendingsByDeclarationId(declaration.Id, declarationCourierStatusPM.Tenant);

                    bool hasActivePending = pendings.Any(x => x.Status == "A");
                    var pending900 = pendings.FirstOrDefault(x =>
                        x.CourierPendingReasonCode == "900" &&
                        x.Status == "S");

                    if (!hasActivePending && pending900 != null)
                    {
                        SendPayment(declarationCourierStatusPM);
                    }
                }
            }
            return sent;

        }
        private void SendPayment(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            try
            {
                var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(declarationCourierStatusPM.Tenant);
                var requestInProgressList = customsRequestsSheetQS.GetRequestInProgress(declarationCourierStatusPM.Tenant, "2755", declarationObjectTableId, declarationCourierStatusPM.DeclarationId, null, null, null, true, null);
                if (requestInProgressList?.Exists(x => x.InterfaceTypeCode == "2755") == true)
                {
                    return;
                }
                var bankIds = new CustomBankRepository(declarationCourierStatusPM.Tenant)
                .GetAll(declarationCourierStatusPM.Tenant)
                .Where(b => b.PayerTypeCode == "3" && !b.InActive)
                .Select(b => b.Id)
                .Take(2)          
                .ToList();

                if (bankIds.Count != 1)           
                    return;

                string unifreightList = SetBankIdInUnifreightListOnServerOnly(bankIds[0]);

                using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                {
                    var requestParams2755 = new GenericRequestParams()
                    {
                        Tenant = declarationCourierStatusPM.Tenant,
                        LoggingEnabled = true,
                        LoggingObjectTableId = declarationObjectTableId,
                        LoggingEntityId = declarationCourierStatusPM.DeclarationId,
                        AppicationId = declarationCourierStatusPM.DeclarationId,
                        InterfaceTypeCode = "2755",
                        FromAutomate = true,
                        LoggingUserId = userId,
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                        UnifreightListOnServerOnly = unifreightList,
                        FutureSendDateTime = futureSendTime,
                    };
                    SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2755, false, futureSendTime);
                    sent = true;
                    scopeNewCRS.Complete();
                }
            }
            catch (System.Exception ex)
            {
                LogMessagingUtil.Instance.AppendLine($"Exception SendPayment!!!CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId}) : {ex.Message}");

            }

        }

        private void SendDeclaration(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            try
            {
                var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(declarationCourierStatusPM.Tenant);
                var requestInProgressList = customsRequestsSheetQS.GetRequestInProgress(declarationCourierStatusPM.Tenant, "2750", declarationObjectTableId, declarationCourierStatusPM.DeclarationId, null, null, null, true, null);
                if (requestInProgressList != null && requestInProgressList.Any())
                {
                    return;
                }
                using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                {

                    var requestParams2750 = new GenericRequestParams()
                    {
                        Tenant = declarationCourierStatusPM.Tenant,
                        LoggingEnabled = true,
                        LoggingObjectTableId = declarationObjectTableId,
                        LoggingEntityId = declarationCourierStatusPM.DeclarationId,
                        FromAutomate = true,
                        AppicationId = declarationCourierStatusPM.DeclarationId,
                        InterfaceTypeCode = "2750",
                        LoggingUserId = userId,
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                        FutureSendDateTime = futureSendTime,
                    };
                    SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2750, false , futureSendTime );
                    LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId})");
                    sent = true;
                    scopeNewCRS.Complete();
                }

            }
            catch (System.Exception ex)
            {
                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId}) : {ex.Message}");
            }
        }

        private void SendManifest(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            try
            {
                LogMessagingUtil.Instance.AppendLine($" Automated SendManifest({declarationCourierStatusPM.DeclarationId})");
                var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(declarationCourierStatusPM.Tenant);
                var requestInProgressList = customsRequestsSheetQS.GetRequestInProgress(declarationCourierStatusPM.Tenant, "1170", declarationObjectTableId, declarationCourierStatusPM.DeclarationId, null, null, null, true, null);
                if (requestInProgressList != null && requestInProgressList.Any())
                {
                    return;
                }
                LogMessagingUtil.Instance.AppendLine($"Didnt return Automated SendManifest({declarationCourierStatusPM.DeclarationId})");
                var requestParams1170 = new MANIFESTRequestRequestParams()
                {
                    Tenant = declarationCourierStatusPM.Tenant,

                    LoggingEnabled = true,
                    LoggingObjectTableId = declarationObjectTableId,
                    LoggingEntityId = declarationCourierStatusPM.DeclarationId,
                    LoggingObjectTableId2 = objectTableIdCourierMaster,
                    LoggingEntityId2 = objectTableIdCourierMaster,

                    InterfaceTypeCode = "1170",

                    LoggingUserId = userId,
                    RequestVIA = SendRequestVIA.WebServiceBatch,
                    DeclarationId = declarationCourierStatusPM.DeclarationId,
                    LoggingEntityReference = declarationCourierStatusPM.DeclarationId,
                    FutureSendDateTime = futureSendTime,
                };
                SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams1170, false, futureSendTime);
                LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId})");
                sent = true;

            }
            catch (System.Exception ex)
            {
                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId}) : {ex.Message}");
            }
        }
        private static string SetBankIdInUnifreightListOnServerOnly(string internalBankId)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("InternalBankId", internalBankId);
            var UnifreightListOnServerOnly = UnifreightListsUtil.Serialize(dic);
            return UnifreightListOnServerOnly;
        }
    }
}
