using Microsoft.Practices.Unity;
using Logitude.AmitalMessaging.Utils;
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
using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel;
using System.Data.Common;
using System.Data.SqlClient;
using Devart.Data.Oracle;

using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Data.SqlClient;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSend2750_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCB2750WithResponseContentHeader, GenericRequestParams>
    {
        public override void Update(DCAInUCB2750WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            //var repo = new DeclarationCourierStatusQueryService(context);
            var repo = new DeclarationCourierStatusRepository(context);
            //List<DeclarationCourierStatusPM> listPoco = new List<DeclarationCourierStatusPM>();
            List<DeclarationCourierStatus> listPoco = new List<DeclarationCourierStatus>();
            decimal minValPay = GetMinValPay(requestParams.Tenant);

            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {

                mess.AppendLine($"מפוצל כבר !!!");

                List<DeclarationCourierStatus> ServerSplitDeclarationsList
                    = repo.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);
                Create2750CRS(requestParams, mess, objectTableId, objectTableIdCourierMaster, ServerSplitDeclarationsList, customResponse.CourierMasterId);
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
                    mess.AppendLine($"GetByMasterIDCourierDeclarationStatusCode");
                    listPoco = GetByMasterIDCourierDeclarationStatusCode(customResponse, requestParams, repo, minValPay);
                }
                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Declaration) send  for master {requestParams.AppicationId} ");
                }
                else
                {

                    listPoco = listPoco.Where(r => (r.CourierPaymentStatusCode == "R" || string.IsNullOrWhiteSpace(r.CourierPaymentStatusCode))).ToList();
                    if (listPoco.Count == 0)
                    {
                        mess.AppendLine($"יש להוסיף בדיקה לשדר מצהר תקינים ושדר הצהרה תקינים שרק הצהרות שלא שולמו ישלחו  {requestParams.AppicationId} ");
                    }

                }
                if (listPoco.Count > 0)
                {



                    listPoco.Select(r => r.DeclarationId).ToList().ChunkBy(100)
   .ForEach(list100 =>
   {
       customResponse.ServerSplitDeclarationsList = list100;
       customResponse.LoggingUserId = requestParams.LoggingUserId;

       var createDCAInUCB2750_MsgMessagingService = new CRSUtil();
       createDCAInUCB2750_MsgMessagingService
       .CreateCRS_DCAIn<DCAInUCB2750WithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);

   });
                    //Create2750CRS(requestParams, mess, objectTableId, objectTableIdCourierMaster, listPoco);
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            //this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            ///LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }



        private static void Create2750CRS(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationCourierStatus> listPoco, string courierMasterId)
        {
            var listDeclarationIdCreateCRS = new List<string>();

            //if is EffectiveFlight : TenantPriority = 98
            var context = CustomContext.GetContext(requestParams.Tenant);
            CourierMasterQueryService myCourierMasterQueryService = new CourierMasterQueryService(context);
            bool isEffectiveFlight = myCourierMasterQueryService.GetSingle(courierMasterId, false, false)?.EffectiveFlight ?? false;


            foreach (var itemPM in listPoco)
            {
                try
                {
                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                    {

                        var requestParams2750 = new GenericRequestParams()
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
                            AppicationId = itemPM.DeclarationId,
                            InterfaceTypeCode = "2750",

                            //LoggingEntityReference = declarationNumber,
                            LoggingUserId = requestParams.LoggingUserId,
                            RequestVIA = SendRequestVIA.WebServiceBatch,
                            ParentId = requestParams.CustomsRequestsSheetId,
                        };
                        if(isEffectiveFlight)
                        {
                            requestParams2750.TenantPriority = 98;
                        }

                        SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2750, false);
                        LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                        mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");


                        RealSetDeclarationCourierDeclarationStatusCode(requestParams.Tenant, itemPM.DeclarationId);


                        //string updateSql = $"Update DeclarationCourierStatuses set COURIERDECLARATIONSTATUSCODE='I' where DECLARATIONID ='{itemPM.DeclarationId}' ";
                        //CustomContext.CommandExecuteNonQuery(requestParams.Tenant, updateSql);

                        scopeNewCRS.Complete();
                    }
                    listDeclarationIdCreateCRS.Add(itemPM.DeclarationId);

                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                }

                //UpdateCOURIERDECLARATIONSTATUSCODE_Inprogress(requestParams, listDeclarationIdCreateCRS);
            }
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


  

        private static void UpdateCOURIERDECLARATIONSTATUSCODE_Inprogress(GenericRequestParams requestParams, List<string> listDeclarationIdCreateCRS)
        {
            //            listDeclarationIdCreateCRS.ChunkBy(100)
            //.ForEach(list100 =>
            //{
            //string inList = String.Join(",", list100.Select(declarationId => $"'{declarationId}'").ToArray());
            //string updateSql = $"Update DeclarationCourierStatuses set COURIERDECLARATIONSTATUSCODE='I' where DECLARATIONID in ({inList}) ";

            //CustomContext.CommandExecuteNonQuery(requestParams.Tenant, updateSql);
            //});
        }

        private static List<DeclarationCourierStatus> GetByMasterIDCourierDeclarationStatusCode(DCAInUCB2750WithResponseContentHeader customResponse, GenericRequestParams requestParams, DeclarationCourierStatusRepository repo, decimal minValPay)
        {
            List<DeclarationCourierStatus> listPoco = new List<DeclarationCourierStatus>();
            if (customResponse.IsWorkSheetFromExcel)
            {
                if (customResponse.CourierDeclarationStatusCode == "X")
                {
                    listPoco = repo.GetFromExcelCourierDeclarationStatusCode(requestParams.Tenant, requestParams.LoggingUserId, "X",
                    customResponse.SelectedBOLValue,
                    customResponse.SelectedStatusValue,
                    customResponse.SelectedTotalInvoiceValue,
                    customResponse.SelectedFastIndividualProcessValue,
                    customResponse.SelectedCustomStatusValue,
                    customResponse.SelectedFinalReleaseValue, minValPay);
                }
                else
                {
                    listPoco = repo.GetFromExcelCourierDeclarationStatusCode(requestParams.Tenant, requestParams.LoggingUserId, "R",
                    customResponse.SelectedBOLValue,
                    customResponse.SelectedStatusValue,
                    customResponse.SelectedTotalInvoiceValue,
                    customResponse.SelectedFastIndividualProcessValue,
                    customResponse.SelectedCustomStatusValue,
                    customResponse.SelectedFinalReleaseValue, minValPay);
                    if (customResponse.CourierDeclarationStatusCode == "RV")
                    {
                        var listPM2 = repo.GetFromExcelCourierDeclarationStatusCode(requestParams.Tenant, requestParams.LoggingUserId, "V", customResponse.SelectedBOLValue,
                        customResponse.SelectedStatusValue,
                        customResponse.SelectedTotalInvoiceValue,
                        customResponse.SelectedFastIndividualProcessValue,
                        customResponse.SelectedCustomStatusValue,
                        customResponse.SelectedFinalReleaseValue, minValPay);
                        listPoco = listPoco.Concat(listPM2).ToList();
                    }
                }
            }
            else
            {
                if (customResponse.CourierDeclarationStatusCode == "X")
                {
                    listPoco = repo.GetByMasterIDCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId, "X",
                    customResponse.SelectedBOLValue,
                    customResponse.SelectedStatusValue,
                    customResponse.SelectedTotalInvoiceValue,
                    customResponse.SelectedFastIndividualProcessValue,
                    customResponse.SelectedCustomStatusValue,
                    customResponse.SelectedFinalReleaseValue, minValPay);
                }
                else
                {
                    listPoco = repo.GetByMasterIDCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId, "R",
                    customResponse.SelectedBOLValue,
                    customResponse.SelectedStatusValue,
                    customResponse.SelectedTotalInvoiceValue,
                    customResponse.SelectedFastIndividualProcessValue,
                    customResponse.SelectedCustomStatusValue,
                    customResponse.SelectedFinalReleaseValue, minValPay);
                    if (customResponse.CourierDeclarationStatusCode == "RV")
                    {
                        var listPM2 = repo.GetByMasterIDCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId, "V", customResponse.SelectedBOLValue,
                        customResponse.SelectedStatusValue,
                        customResponse.SelectedTotalInvoiceValue,
                        customResponse.SelectedFastIndividualProcessValue,
                        customResponse.SelectedCustomStatusValue,
                        customResponse.SelectedFinalReleaseValue, minValPay);
                        listPoco = listPoco.Concat(listPM2).ToList();
                    }
                }
            }
            return listPoco;
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB2750WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        private decimal GetMinValPay(int tenant)
        {
            const decimal fallback = 75m;
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
            var s = defaultValueQueryService.GetDefault("ISRAEL", "CGO_MINVAL_PAY", "NON", "NON", tenant);

            if (string.IsNullOrWhiteSpace(s)) return fallback;

            var normalized = s.Trim().Replace(",", ".");
            if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) && val > 0)
                return val;

            return fallback;
        }

    }
}
