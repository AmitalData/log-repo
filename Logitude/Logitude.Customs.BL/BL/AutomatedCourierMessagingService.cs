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
                    hasActivePending = declarationPendingRepo.HasPendingWithStatus(declaration.Id, declarationCourierStatusPM.Tenant,"A");
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
                    RealSetDeclarationCourierPaymentStatusCode(declarationCourierStatusPM.Tenant, declarationCourierStatusPM.DeclarationId);

                    scopeNewCRS.Complete();
                }
            }
            catch (System.Exception ee1)
            {
                LogMessagingUtil.Instance.AppendLine($"Exception SendPayment!!!CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId}) : {ee1.Message}");
            
            }

        }

        private void SendDeclaration(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            try
            {
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
                        //LoggingEntityReference = declarationNumber,
                        LoggingUserId = userId,
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                    };
                    SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2750, false);
                    LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId})");
                    RealSetDeclarationCourierDeclarationStatusCode(declarationCourierStatusPM.Tenant, declarationCourierStatusPM.DeclarationId);

                    scopeNewCRS.Complete();
                }

            }
            catch (System.Exception ee1)
            {
                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId}) : {ee1.Message}");
            }
        }

        private void SendManifest(DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            
            var requestParams1170 = new MANIFESTRequestRequestParams()
            {
                Tenant = declarationCourierStatusPM.Tenant,
                //IsFakeResponse = true,
                //RequestName = requestName,
                //ResponseName = responseName,
                LoggingEnabled = true,
                LoggingObjectTableId = declarationObjectTableId,
                LoggingEntityId = declarationCourierStatusPM.DeclarationId,
                LoggingObjectTableId2 = objectTableIdCourierMaster,
                LoggingEntityId2 = objectTableIdCourierMaster,
                //AppicationId = itemPM.DeclarationId,
                InterfaceTypeCode = "1170",

                //LoggingEntityReference = declarationNumber,
                LoggingUserId = userId,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                DeclarationId = declarationCourierStatusPM.DeclarationId,
                LoggingEntityReference = declarationCourierStatusPM.DeclarationId,
            };
            SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams1170, false);
            LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({declarationCourierStatusPM.DeclarationId})");
        }
        public static void RealSetDeclarationCourierDeclarationStatusCode(int tenant, string declarationId)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            string strConnString = TenantServerConfigration.GetDbConnection(tenant);


            if (dbms == "oracle")
            {

                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update DeclarationCourierStatuses set COURIERDECLARATIONSTATUSCODE='I'";
                    cmd = cmd + "  where DECLARATIONID=:p1 ";

                    OracleCommand oracleCommand = new OracleCommand(cmd, con);
                    oracleCommand.Parameters.Add(new OracleParameter("p1", declarationId));
                    con.Open();
                    oracleCommand.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = "Update Customs.DeclarationCourierStatuses set COURIERDECLARATIONSTATUSCODE='I'";
                    cmd = cmd + " where DECLARATIONID=" + "'" + declarationId + "'";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }
        public static void RealSetDeclarationCourierPaymentStatusCode(int tenant, string declarationId)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = TenantServerConfigration.GetDbConnection(tenant);

            if (dbms == "oracle")
            {

                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update DeclarationCourierStatuses set COURIERPAYMENTSTATUSCODE='I'";
                    cmd = cmd + "  where DECLARATIONID=:p1 ";

                    OracleCommand oracleCommand = new OracleCommand(cmd, con);
                    oracleCommand.Parameters.Add(new OracleParameter("p1", declarationId));
                    con.Open();
                    oracleCommand.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = "Update Customs.DeclarationCourierStatuses set COURIERPAYMENTSTATUSCODE='I'";
                    cmd = cmd + " where DECLARATIONID=" + "'" + declarationId + "'";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }


    }
}
