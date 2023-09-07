using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;
using Microsoft.Practices.Unity;
using Logitude.CustomsMessaging.Utils;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchApproveAllPending_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBApproveAllPendingWithResponseContentHeader, GenericRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBApproveAllPendingWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBApproveAllPendingWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            var qs = new DeclarationCourierStatusQueryService(context);
            List<DeclarationPM> lockedDeclarations = new List<DeclarationPM>();
            if (customResponse.ServerSplitDeclarationsList == null || (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count == 0))
            {
                LogMessagingUtil.Instance.AppendLine("Splitter to 50 - Create new CRS");
                mess.AppendLine($"Splitter to 50 - Create new CRS master {requestParams.AppicationId} ");
                List<string> DeclarationIdList = new List<string>();
                if (customResponse.declarationList != null && customResponse.declarationList.Length > 0)
                {
                    DeclarationIdList = customResponse.declarationList.ToList();
                }
                else
                {
                    DeclarationIdList = qs.GetByMasterID_DeclarationIdList(requestParams.Tenant, requestParams.AppicationId, customResponse.IsWorkSheetFromExcel,requestParams.LoggingUserId);
                }
                DeclarationIdList.ChunkBy(50).ForEach(list50 =>
                {
                    //CreateCRS(customResponse, requestParams,list50);
                    customResponse.ServerSplitDeclarationsList = list50;
                    customResponse.LoggingUserId = requestParams.LoggingUserId;
                    var myCRSUtil = new CRSUtil();
                    myCRSUtil
                    .CreateCRS_DCAIn<DCAInUCBApproveAllPendingWithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));

                });
                this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
                this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
                this.MyResponseData.UserMessage = mess.ToString();
                this.MyResponseData.Succeeded = true;

                return;
            }

            LogMessagingUtil.Instance.AppendLine("Handle 50 DeclarationIdList");

            foreach (string decId in customResponse.ServerSplitDeclarationsList)
            {
                using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                {
                    var context1 = CustomContext.GetContext(requestParams.Tenant);//context each CRS TRANS
                    var approveAllPending = new ApproveAllPendingClass(context1);
                    var courierStatusPM = qs.GetSingle(decId, true, false);
                    if (courierStatusPM != null && courierStatusPM.DeclarationPendings.Find(d => d.CourierPendingRequireApr == true && d.Approval != true) != null)
                    {
                        approveAllPending.ApproveAllPending(requestParams, mess, objectTableId, objectTableIdCourierMaster, lockedDeclarations, courierStatusPM, customResponse);
                    }
                    scopeNewCRS.Complete();
                }

            }



            if (lockedDeclarations != null && lockedDeclarations.Count() > 0)
            {
                List<string> declarationsList = new List<string>();
                /* string message = string.Concat("אתר אחסון בטיסה השתנה ל ", customResponse.StorageSiteCode, ", אך ההצהרה לא ניתנת לעידכון. נא לעדכן ידנית");
                 if (customResponse.UnLoadPortCode != null)
                 {
                      message = string.Concat("אתר פריקה בטיסה השתנה ל ", customResponse.UnLoadPortCode, ", אך ההצהרה לא ניתנת לעידכון. נא לעדכן ידנית");
                 }*/
                mess.AppendLine("\n" + "Locked Declarations: " + "\n");
                foreach (DeclarationPM itemDeclaration in lockedDeclarations)
                {
                    declarationsList.Add(itemDeclaration.CustomFileNo);
                    mess.AppendLine($" ( {itemDeclaration.Id} ),");
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }
    }

    class ApproveAllPendingClass
    {
        private ICustomContext context;

        public ApproveAllPendingClass(ICustomContext context)
        {
            this.context = context;
        }

        public void ApproveAllPending(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationPM> lockedDeclarations, DeclarationCourierStatusPM itemPM, DCAInUCBApproveAllPendingWithResponseContentHeader customResponse)
        {


            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            try
            {
                bool isUpdateDeclaration = true;
                if (isUpdateDeclaration)
                {
                    var declarationPendingList = itemPM.DeclarationPendings.Where(r => r.CourierPendingRequireApr == true && r.Approval != true);
                    if (customResponse.PendingCode != "A" && customResponse.PendingCode != "NotApproved")
                    {
                        declarationPendingList = declarationPendingList.Where(r => r.CourierPendingReasonCode == customResponse.PendingCode);
                    }
                    declarationPendingList = declarationPendingList.ToList();
                    foreach (var declarationPending in declarationPendingList)
                    {
                        declarationPending.Approval = true;
                        declarationPending.ChangeSetOp = ChangeSetOperation.Update;
                        if (itemPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                        itemPM.ApprovedCourierPendingList += "," + declarationPending.CourierPendingReasonCode;
                    }
                }
                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), itemPM.Tenant);
                declarationCourierStatusUpdateService.Update(itemPM, true);

            }
            catch (System.Exception ee1)
            {

                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
            }
        }

    }
}
