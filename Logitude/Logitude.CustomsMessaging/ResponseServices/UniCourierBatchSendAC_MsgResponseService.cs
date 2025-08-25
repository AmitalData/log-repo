
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
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSendAC_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCBACWithResponseContentHeader, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(DCAInUCBACWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var declarationCourierStatusUpdate = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId =ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            var repoDeclarationCourierStatus = new DeclarationCourierStatusRepository(context);
            var repo = new DeclarationCourierStatusQueryService(context);
            var repo2 = new CourierDeclarationQueryService(context);

            List<string> listPoco = new List<string>();
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {

                mess.AppendLine($"מפוצל כבר !!!");

                List<DeclarationCourierStatusPM> ServerSplitDeclarationsList
                    = repo.GetDeclarationCourierStatusByDeclarationIdList(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);

                CreateCRS_AC_UpdateCOURIERMANIFESTSTATUSCODE_Inprogress(requestParams, mess, objectTableId, objectTableIdCourierMaster, ServerSplitDeclarationsList);
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");

                listPoco = repo2.GetDeclarationIdsByCourierMasterID(customResponse.CourierMasterId, requestParams.Tenant);


                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"יש להוסיף בדיקה לשדר מצהר תקינים ושדר הצהרה תקינים שרק הצהרות שלא שולמו ישלחו  {requestParams.AppicationId} ");
                }
                else
                {
                    listPoco.ChunkBy(100)
                    .ForEach(list100 =>
                    {
                        customResponse.ServerSplitDeclarationsList = list100;
                        customResponse.LoggingUserId = requestParams.LoggingUserId;

                        var CreateDCAInUCBAC_MsgMessagingService = new CRSUtil();
                        CreateDCAInUCBAC_MsgMessagingService
                        .CreateCRS_DCAIn<DCAInUCBACWithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);

                    });
                }
            }
            

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }



       
        private static void CreateCRS_AC_UpdateCOURIERMANIFESTSTATUSCODE_Inprogress(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationCourierStatusPM> listPM)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);

            var declarationCourierStatusUpdate = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);


            var listDeclarationIdCreateCRS = new List<string>();
            foreach (var itemPM in listPM)
            {
                try
                {

                    FeatureQuery featureQuery = new FeatureQuery(requestParams.Tenant);

                    var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);

                    var isFeature = features.Features.FirstOrDefault(x => x.Code == "SendDeclaration");

                    itemPM.ClassificationApproved = true;   
                    itemPM.ChangeSetOp = ChangeSetOperation.Update;
                    declarationCourierStatusUpdate.Update(itemPM,true);
                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                    {
                        if (itemPM.CourierManifestStatusCode == "V" && isFeature != null)
                        {
                            Create2750(requestParams, mess, objectTableId, objectTableIdCourierMaster, itemPM);
                        }
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

        public static void RealSetDeclarationCourierManifestStatusCode(int tenant, string declarationId)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = TenantServerConfigration.GetDbConnection(tenant);
            if (dbms == "oracle")
            {

                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update DeclarationCourierStatuses set COURIERMANIFESTSTATUSCODE='I'";
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
                    string cmd = "Update Customs.DeclarationCourierStatuses set COURIERMANIFESTSTATUSCODE='I'";
                    cmd = cmd + " where DECLARATIONID=" + "'" + declarationId + "'";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }


        private static void Create2750(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, DeclarationCourierStatusPM itemPM)
        {
            try
            {
                var requestParams2750 = new GenericRequestParams()
                {
                    Tenant = requestParams.Tenant,
                    LoggingEnabled = true,
                    LoggingObjectTableId = objectTableId,
                    LoggingEntityId = itemPM.DeclarationId,
                    AppicationId = itemPM.DeclarationId,
                    InterfaceTypeCode = "2750",
                    LoggingUserId = requestParams.LoggingUserId,
                    RequestVIA = SendRequestVIA.WebServiceBatch,

                };

                SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2750, false, DateTime.Now.AddMinutes(2));
                LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");

            }
            catch (System.Exception ee1)
            {

                LogMessagingUtil.Instance.AppendLine($"Exception!!!OnSucceededSendDeclarationDelay1Min({itemPM.DeclarationId}) : {ee1.Message}");

            }

            
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBACWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        

    }
}
