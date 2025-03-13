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
        public AutomatedCustomsMessagingService(int tenant)
        {
            FeatureQuery featureQuery = new FeatureQuery();
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(tenant), tenant);

            _featureSendManifest = features.Features.Any(x => x.Code == "SendManifest");
            _featureSendDeclaration = features.Features.Any(x => x.Code == "SendDeclaration");
            _featureSendPayment = features.Features.Any(x => x.Code == "SendPaymentOn900Close");

        }
        public void CheckAndSendMessageis(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            return;
            if (!_featureSendManifest && !_featureSendDeclaration && !_featureSendPayment)
            {
                return;
            }


            declarationObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");


            var courierMasterRepo = new CourierMasterRepository(declarationCourierStatusPM.Tenant);
            var declarationPendingRepo = new DeclarationPendingRepository(declarationCourierStatusPM.Tenant);
            var declarationRepo = new DeclarationRepository(declarationCourierStatusPM.Tenant);

            Declaration declaration = null;

            if (_featureSendManifest && declarationCourierStatusPM.CourierManifestStatusCode == "R")
            {
                SendManifest(declarationCourierStatusPM);
                return;
            }

            if (_featureSendDeclaration
                && declarationCourierStatusPM.CourierManifestStatusCode == "V"
                && declarationCourierStatusPM.CourierDeclarationStatusCode == "R"
                && declarationCourierStatusPM.DocumentStatusCode == "V")
            {

                declaration = declarationRepo.GetSingle(
                    declarationCourierStatusPM.DeclarationId,
                    declarationCourierStatusPM.Tenant);


                bool hasActivePending = false;
                if (declaration != null)
                {
                    hasActivePending = declarationPendingRepo.HasPendingWithStatus(declaration.Id, declarationCourierStatusPM.Tenant, "A");
                }

                if (declaration != null &&
                    !hasActivePending &&
                    string.IsNullOrEmpty(declaration.ImporterCode))
                {
                    SendDeclaration(declarationCourierStatusPM);
                    return;
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
                    return;
                }

                if (declaration == null)
                {
                    declaration = declarationRepo.GetSingle(
                        declarationCourierStatusPM.DeclarationId,
                        declarationCourierStatusPM.Tenant);
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
        }
        private void SendPayment(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            try
            {
                var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(declarationCourierStatusPM.Tenant);
                var requestInProgressList = customsRequestsSheetQS.GetRequestInProgress(declarationCourierStatusPM.Tenant, "2755", declarationObjectTableId, declarationCourierStatusPM.DeclarationId, null, null, null, true, null);
                if (requestInProgressList != null && requestInProgressList.Any())
                {
                    return;
                }
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
                        LoggingUserId = userId,
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                    };
                    SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2755, false);

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
                        AppicationId = declarationCourierStatusPM.DeclarationId,
                        InterfaceTypeCode = "2750",
                        LoggingUserId = userId,
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                    };
                    SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2750, false);
                    LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId})");

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

                var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(declarationCourierStatusPM.Tenant);
                var requestInProgressList = customsRequestsSheetQS.GetRequestInProgress(declarationCourierStatusPM.Tenant, "1170", declarationObjectTableId, declarationCourierStatusPM.DeclarationId, null, null, null, true, null);
                if (requestInProgressList != null && requestInProgressList.Any())
                {
                    return;
                }

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
                };
                SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams1170, false);
                LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId})");

            }
            catch (System.Exception ex)
            {
                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId}) : {ex.Message}");
            }
        }
    }
}
