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
using Logitude.Customs.BL.Messaging;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Maman;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSendDelayForm_MsgResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, DCAInUCBDelayFormWithResponseContentHeader, GenericRequestParams>
    {
        public override void Update(DCAInUCBDelayFormWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            var listPoco = new List<DeclarationCourierStatus>();
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {
                mess.AppendLine($"מפוצל כבר !!!");
                SendDelayForm(mess, context, customResponse.ServerSplitDeclarationsList, requestParams.Tenant);
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");

                if (customResponse.ClientFilterDeclarationsList != null && customResponse.ClientFilterDeclarationsList.Count > 0)
                {
                    mess.AppendLine($"סומנו בצד הלקוח ");
                    var repo = new DeclarationCourierStatusRepository(context);
                    listPoco = repo.GetDeclarationsByIds(customResponse.ClientFilterDeclarationsList, requestParams.Tenant);
                }
                
                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Declaration) send  for master {requestParams.AppicationId} ");
                }

                listPoco.Select(r => r.DeclarationId).ToList().ChunkBy(100)
                     .ForEach(list100 =>
                     {
                         customResponse.ServerSplitDeclarationsList = list100;
                         customResponse.LoggingUserId = requestParams.LoggingUserId;
                         var CreateDCAInUCBSEDF_MsgMessagingService = new CRSUtil();
                         CreateDCAInUCBSEDF_MsgMessagingService.CreateCRS_DCAIn<DCAInUCBDelayFormWithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);
                     });
            }
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

    

        private static void SendDelayForm(StringBuilder mess, ICustomContext myContext, List<string> declarationIds, int tenant)
        {
            foreach (var declarationId in declarationIds)
            {
                mess.AppendLine("declarationId = "+ declarationId);
                DeclarationMamanSpecialActionRepository declarationMamanSpecialActionRepository = new DeclarationMamanSpecialActionRepository(tenant);
                var declarationMamanSpecialAction = declarationMamanSpecialActionRepository.GetDeclarationMamanSpecialActionByDeclarationIdAction(declarationId, tenant, "2");
                if (declarationMamanSpecialAction == null)
                {
                    mess.AppendLine("create new declarationMamanSpecialAction entity");
                    var declarationMamanSpecialActionPM = new DeclarationMamanSpecialActionPM();
                    declarationMamanSpecialActionPM.Tenant = tenant;
                    declarationMamanSpecialActionPM.DeclarationId = declarationId;
                    declarationMamanSpecialActionPM.MamanSpecialActionCode = "2";
                    declarationMamanSpecialActionPM.ChangeSetOp = ChangeSetOperation.Insert;
                    DeclarationMamanSpecialActionUpdateService service = new DeclarationMamanSpecialActionUpdateService(myContext, new Dictionary<string, IContext>(), tenant);
                    service.Update(declarationMamanSpecialActionPM, true);
                }
                ICourierGWMessageECSpcRequestService courierGWMessageECSpclRequestService = null;

                DeclarationQueryService declarationQueryService = new DeclarationQueryService(myContext);
                DeclarationPM declaration = declarationQueryService.GetSingle(declarationId, true, false);
                if (declaration != null && declaration.Consignments != null && declaration.Consignments.Count() > 0)
                {
                    var amitalContext = AmitalContext.GetContext(tenant);
                    var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);
                    var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", false, true);

                    if (def.DEFDATA.Contains("ILMMN") && declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILMMN") // Maman
                    {
                        courierGWMessageECSpclRequestService = new CourierGWMessageECSpclMamanRequestService();
                    }
                    else if (def.DEFDATA.Contains("ILOVL") && declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL") // OVS
                    {
                        courierGWMessageECSpclRequestService = new Logitude.Customs.BL.Messaging.ILOVS.CourierOVSSpecialActionRequestService();
                    }
                    else if (def.DEFDATA.Contains("ILSWS") && declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILSWS") // OVS
                    {
                        courierGWMessageECSpclRequestService = new Logitude.Customs.BL.Messaging.ILSWS.CourierSWSSpecialActionRequestService();
                    }
                }

                string actionResultString = "";
                if (courierGWMessageECSpclRequestService != null)
                {
                    MamanActionCodeUpdateOrCancel mamanActionCode = MamanActionCodeUpdateOrCancel.Upsert;
                    MamanSpecialCode mamanSpecialCode = MamanSpecialCode.ReceivingDelayCertificate_DelayIt;
                    actionResultString = courierGWMessageECSpclRequestService.BuildQueueSendWebAPI(declarationId, tenant, mamanActionCode, mamanSpecialCode);
                }
                else
                {
                    actionResultString = "לא קיימת הרשאה";
                }
                mess.AppendLine(actionResultString);
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBDelayFormWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

    }
}
