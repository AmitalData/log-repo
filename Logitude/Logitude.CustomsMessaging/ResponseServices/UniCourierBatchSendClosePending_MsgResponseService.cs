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
    public class UniCourierBatchSendClosePending_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBClosePendingWithResponseContentHeader, GenericRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBClosePendingWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBClosePendingWithResponseContentHeader customResponse, GenericRequestParams requestParams)
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
                List<string> DeclarationIdList=new List<string>();
                if (customResponse.declarationList != null && customResponse.declarationList.Length > 0)
                {
                    DeclarationIdList = customResponse.declarationList.ToList();
                }
                else
                {
                    DeclarationIdList = qs.GetByMasterID_DeclarationIdList(requestParams.Tenant, requestParams.AppicationId);
                }
                DeclarationIdList.ChunkBy(50).ForEach(list50 =>
                {
                    //CreateCRS(customResponse, requestParams,list50);
                    customResponse.ServerSplitDeclarationsList = list50;
                    customResponse.LoggingUserId = requestParams.LoggingUserId;
                    var myCRSUtil = new CRSUtil();
                    myCRSUtil
                    .CreateCRS_DCAIn<DCAInUCBClosePendingWithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));

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
                    var closePendingService = new ClosePendingService(context1);
                    var courierStatusPM = qs.GetSingle(decId, true, false);
                    if (courierStatusPM != null && courierStatusPM.DeclarationPendings.Find(d => d.Status == "A") != null)
                    {
                        closePendingService.ClosePending(customResponse.PendingCode, requestParams, mess, objectTableId, objectTableIdCourierMaster, lockedDeclarations, courierStatusPM);
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

    class ClosePendingService
    {
        private ICustomContext context;

        public ClosePendingService(ICustomContext context)
        {
            this.context = context;
        }

        public void ClosePending(string[] PendingCode, GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationPM> lockedDeclarations, DeclarationCourierStatusPM itemPM)
        {


            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            try
            {
                var myDeclarationQueryService = new DeclarationQueryService(context);
                myDeclarationQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
                DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(itemPM.DeclarationId, true, false);
                if (declarationPM != null)
                {
                    bool isUpdateDeclaration = true;
                    /*long lCUSTOMFILENO;
                    if (!long.TryParse(declarationPM.CustomFileNo, out lCUSTOMFILENO))
                    {
                        throw new BusinessErrorException("_DirtyDeclarationPaymentPM.DeclarationId could not convert to long ");
                    }
                    var myCCUFILEMRepository = new CCUFILEMRepository(declarationPM.Tenant);
                    var ccufilem = myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
                    var myCCUQUELOCKRepository = new CCUQUELOCKRepository(requestParams.Tenant);
                    try
                    {
                        var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());
                    }
                    catch (System.Exception)
                    {
                        LogMessagingUtil.Instance.AppendLine($"GetSingleGeneralLockNOWAIT(CCUFILEM, {declarationPM.CustomFileNo}) ==> Already Lock => try later (*5) ");
                        isUpdateDeclaration = false;
                    }

                    if (!myDeclarationUpdateService.CheckIfUpdatingAllowed(declarationPM))
                    {
                        LogMessagingUtil.Instance.AppendLine($"CheckIfUpdatingAllowed({declarationPM.CustomFileNo}) ");
                        isUpdateDeclaration = false;
                    }
                    */
                    if (isUpdateDeclaration)
                    {
                        CourierPendingReasonRepository courierPendingReasonRepository = new CourierPendingReasonRepository(declarationPM.Tenant);

                        foreach (var pendingCode in PendingCode)
                        {
                            var declarationPendingPM = itemPM.DeclarationPendings.Where(r => r.DeclarationID == declarationPM.Id && r.CourierPendingReasonCode == pendingCode).FirstOrDefault();
                            if (declarationPendingPM != null && declarationPendingPM.Status == "A")
                            {
                                //UPDATE to solve
                                declarationPendingPM.Status = "S";
                                declarationPendingPM.ChangeSetOp = ChangeSetOperation.Update;
                                if (itemPM.ChangeSetOp == ChangeSetOperation.None)
                                {
                                    itemPM.ChangeSetOp = ChangeSetOperation.Update;
                                }
                            }
                        }
                        DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), declarationPM.Tenant);
                        declarationCourierStatusUpdateService.Update(itemPM, true);
                    }
                    else
                    {
                        lockedDeclarations.Add(declarationPM);
                    }
                }
            }
            catch (System.Exception ee1)
            {

                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
            }
        }

    }
}
