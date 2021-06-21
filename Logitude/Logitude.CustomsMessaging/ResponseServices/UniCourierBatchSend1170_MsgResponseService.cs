
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

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSend1170_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCB1170WithResponseContentHeader, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(DCAInUCB1170WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {

            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId =
    //ObjectTableRepository.GetObjectTableByName("Customs.CustomsClosedTable");
    ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            //var qs = new DeclarationCourierStatusQueryService(context);
            var repo = new DeclarationCourierStatusRepository(context);
            List<DeclarationCourierStatus> listPoco = new List<DeclarationCourierStatus>();
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {

                mess.AppendLine($"מפוצל כבר !!!");

                List<DeclarationCourierStatus> ServerSplitDeclarationsList
                    = repo.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);
                CreateCRS1170UpdateCOURIERMANIFESTSTATUSCODE_Inprogress(requestParams, mess, objectTableId, objectTableIdCourierMaster, ServerSplitDeclarationsList);
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
                    mess.AppendLine($"GetByMasterIDCourierManifestStatusCode");
                    listPoco = GetByMasterIDCourierManifestStatusCode(customResponse, requestParams, repo);
                }
                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Manifest) send for master {requestParams.AppicationId} ");
                }
                else
                {
                    if (customResponse.CourierDeclarationStatusCode == "RV")
                    {
                        listPoco = listPoco.Where(r => (r.CourierPaymentStatusCode != "P")).ToList();
                    }
                    else
                    {
                        //listPoco = listPoco.Where(r => (r.CourierPaymentStatusCode == "R" || string.IsNullOrWhiteSpace(r.CourierPaymentStatusCode))).ToList();
                    }
                        
                    if (listPoco.Count == 0)
                    {
                        mess.AppendLine($"יש להוסיף בדיקה לשדר מצהר תקינים ושדר הצהרה תקינים שרק הצהרות שלא שולמו ישלחו  {requestParams.AppicationId} ");
                    }
                    else
                    {
                        //CreateCRS1170UpdateCOURIERMANIFESTSTATUSCODE_Inprogress(requestParams, mess, objectTableId, objectTableIdCourierMaster, listPM);

                        listPoco.Select(r=>r.DeclarationId).ToList().ChunkBy(100)
    .ForEach(list100 =>
    {
        customResponse.ServerSplitDeclarationsList = list100;
        customResponse.LoggingUserId = requestParams.LoggingUserId;

        //CreateDCAInUCB1170_MsgMessagingService(customResponse, requestParams);
        var CreateDCAInUCB1170_MsgMessagingService = new CRSUtil();
        CreateDCAInUCB1170_MsgMessagingService
        .CreateCRS_DCAIn<DCAInUCB1170WithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));

    });
                    }



                }
            }

            

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            //this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            ///LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

      

            private static List<DeclarationCourierStatus> GetByMasterIDCourierManifestStatusCode(DCAInUCB1170WithResponseContentHeader customResponse, GenericRequestParams requestParams, DeclarationCourierStatusRepository qs)
        {
            List<DeclarationCourierStatus> listPM = qs.GetByMasterIDCourierManifestStatusCode(requestParams.Tenant, requestParams.AppicationId, "R",
               customResponse.SelectedBOLValue,
               customResponse.SelectedStatusValue,
               customResponse.SelectedTotalInvoiceValue,
               customResponse.SelectedFastIndividualProcessValue,
               customResponse.SelectedCustomStatusValue);
            if (customResponse.CourierDeclarationStatusCode == "RV")
            {
                var listPM2 = qs.GetByMasterIDCourierManifestStatusCode(requestParams.Tenant, requestParams.AppicationId, "V", customResponse.SelectedBOLValue,
                customResponse.SelectedStatusValue,
                customResponse.SelectedTotalInvoiceValue,
                customResponse.SelectedFastIndividualProcessValue,
                customResponse.SelectedCustomStatusValue);
                listPM = listPM.Concat(listPM2).ToList();
            }

            return listPM;
        }

        private static void CreateCRS1170UpdateCOURIERMANIFESTSTATUSCODE_Inprogress(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationCourierStatus> listPM)
        {
            
            var listDeclarationIdCreateCRS = new List<string>();
            foreach (var itemPM in listPM)
            {
                try
                {
                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                    {
                        Create1170(requestParams, mess, objectTableId, objectTableIdCourierMaster, itemPM);
                        
                        string updateSql = $"Update DeclarationCourierStatuses set COURIERMANIFESTSTATUSCODE='I' where DECLARATIONID ='{itemPM.DeclarationId}' ";
                        CustomContext.CommandExecuteNonQuery(requestParams.Tenant, updateSql);

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
//            listDeclarationIdCreateCRS.ChunkBy(100)
//.ForEach(list100 =>
//{
//    string inList = String.Join(",", list100.Select(declarationId => $"'{declarationId}'").ToArray());
//    string updateSql = $"Update DeclarationCourierStatuses set COURIERMANIFESTSTATUSCODE='I' where DECLARATIONID in ({inList}) ";

//    CustomContext.CommandExecuteNonQuery(requestParams.Tenant, updateSql);
//});
        }

        private static void Create1170(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, DeclarationCourierStatus itemPM)
        {
            var requestParams1170 = new MANIFESTRequestRequestParams()
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
                //AppicationId = itemPM.DeclarationId,
                InterfaceTypeCode = "1170",

                //LoggingEntityReference = declarationNumber,
                LoggingUserId = requestParams.LoggingUserId,
                RequestVIA = SendRequestVIA.WebServiceBatch,


                DeclarationId = itemPM.DeclarationId,
                LoggingEntityReference = itemPM.DeclarationId,
                //ImportManifest =""

            };

            SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams1170, false);
            LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
            mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB1170WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        

    }
}
