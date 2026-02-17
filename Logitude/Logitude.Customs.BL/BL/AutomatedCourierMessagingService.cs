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
        private readonly bool _featureSendPaymentOn902Close;
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
            _featureSendPaymentOn902Close = features.Features.Any(x => x.Code == "SendPaymentOn902Close");

        }
        public bool CheckAndSendMessageis(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            sent = false;

            if (!_featureSendManifest && !_featureSendDeclaration && !_featureSendPayment && !_featureSendPaymentOn902Close)
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

            if (declaration == null || declaration.IsAmendment == true || declaration.HatraDate != null || declaration.PaymentDate != null)
            {
                Log("DECL:N:D0");
                return sent;
            }

            if (TrySendManifest(declarationCourierStatusPM)) return sent;
            if (TrySendDeclaration(declarationCourierStatusPM, declaration, declarationPendingRepo)) return sent;
            if (TrySendPaymentOn900(declarationCourierStatusPM, declaration, courierMasterRepo, declarationPendingRepo)) return sent;
            if (TrySendPaymentOn902(declarationCourierStatusPM, declaration, courierMasterRepo, declarationPendingRepo)) return sent;

            return sent;

        }
      

        private bool TrySendManifest(DeclarationCourierStatusPM pm)
        {
            if (!_featureSendManifest) return false;
            if (pm.CourierManifestStatusCode != "R") return false;
            SendManifest(pm);              // sets 'sent' inside on success
            return sent;
        }

        private bool TrySendDeclaration(DeclarationCourierStatusPM pm, Declaration declaration, DeclarationPendingRepository declarationPendingRepo)
        {
            if (!_featureSendDeclaration) return false;
            if (pm.CourierManifestStatusCode != "V" || pm.CourierDeclarationStatusCode != "R" || pm.DocumentStatusCode != "V")
                return false;

            Log("DECL:E");

            bool hasActivePending = declarationPendingRepo.HasPendingWithStatus(declaration.Id, pm.Tenant, "A");
            Log($"DECL:P={(hasActivePending ? 1 : 0)}");
            if (hasActivePending) { Log("DECL:N:P"); return false; }

            var idHas = !string.IsNullOrEmpty(declaration.ImporterId);
            var codeHas = !string.IsNullOrEmpty(declaration.ImporterCode);
            bool importerOk = idHas || (!idHas && !codeHas);

            Log($"DECL:ImpOK={(importerOk ? 1 : 0)};ID={(idHas ? 1 : 0)};CODE={(codeHas ? 1 : 0)}");
            if (!importerOk) { Log("DECL:N:IMP"); return false; }


            Log("DECL:SEND");
            SendDeclaration(pm);           // sets 'sent' inside on success
            Log($"DECL:RES={(sent ? "OK" : "NO")}");
            return sent;
        }

        private bool TrySendPaymentOn900(DeclarationCourierStatusPM pm, Declaration declaration,
                                         CourierMasterRepository courierMasterRepo, DeclarationPendingRepository declarationPendingRepo)
        {
            if (!_featureSendPayment) return false;

            var cm = courierMasterRepo.GetCourierMasterByDeclarationId(pm.DeclarationId, pm.Tenant);
            if (cm == null || cm.CourierMasterPaymentStatusCd != "1") return false;
            if (pm.CourierDeclarationStatusCode != "V") return false;               

            var pendings = declarationPendingRepo.GetDeclarationPendingsByDeclarationId(declaration.Id, pm.Tenant);
            bool hasActivePending = pendings.Any(x => x.Status == "A");
            bool pending900Solved = pendings.Any(x => x.CourierPendingReasonCode == "900" && x.Status == "S");

            if (hasActivePending || !pending900Solved) return false;

            SendPayment(pm);             
            return sent;
        }

        private bool TrySendPaymentOn902(DeclarationCourierStatusPM pm, Declaration declaration,
                                         CourierMasterRepository courierMasterRepo, DeclarationPendingRepository declarationPendingRepo)
        {
            if (!_featureSendPaymentOn902Close) return false;

            Log("902:E");

            var cm = courierMasterRepo.GetCourierMasterByDeclarationId(pm.DeclarationId, pm.Tenant);
            if (cm == null) { Log("902:N:CM0"); return false; }
            if (cm.CourierMasterPaymentStatusCd != "1") { Log("902:N:PAY"); return false; }
            if (pm.CourierDeclarationStatusCode != "V") { Log("902:N:DECL"); return false; }

            var pendings = declarationPendingRepo.GetDeclarationPendingsByDeclarationId(declaration.Id, pm.Tenant);
            bool hasActivePending = pendings.Any(x => x.Status == "A");
            bool pending902Solved = pendings.Any(x => x.CourierPendingReasonCode == "902" && x.Status == "S");

            Log($"902:P={(hasActivePending ? 1 : 0)};S={(pending902Solved ? 1 : 0)}");

            if (hasActivePending) { Log("902:N:PA"); return false; }
            if (!pending902Solved) { Log("902:N:S0"); return false; }

            Log("902:SEND");
            SendPayment(pm);              
            Log($"902:RES={(sent ? "OK" : "NO")}");
            return sent;
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
        private void SendDeclaration(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            try
            {
                var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(declarationCourierStatusPM.Tenant);
                var requestInProgressList = customsRequestsSheetQS.GetRequestInProgress(declarationCourierStatusPM.Tenant, "2750", declarationObjectTableId, declarationCourierStatusPM.DeclarationId, null, null, null, false, null);
                if (requestInProgressList != null && requestInProgressList.Any())
                {
                    Log($"RIPL>0");
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
                    SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2750, false, futureSendTime);
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
        private static string SetBankIdInUnifreightListOnServerOnly(string internalBankId)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("InternalBankId", internalBankId);
            var UnifreightListOnServerOnly = UnifreightListsUtil.Serialize(dic);
            return UnifreightListOnServerOnly;
        }
        private static void Log(string m) => LogMessagingUtil.Instance.AppendLine($"Automated|{m}");
    }
}
