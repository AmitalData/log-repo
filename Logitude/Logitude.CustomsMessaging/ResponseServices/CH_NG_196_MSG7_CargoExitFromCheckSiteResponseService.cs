using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.PhysicalCheck;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CH_NG_196_MSG7_CargoExitFromCheckSiteResponseService
        : ResponseServiceBase<INF_MSG_GenericResponseData, CH_NG_196_MSG7_CargoExitFromCheckSite, GenericRequestParams>
    {
        
        public override INF_MSG_GenericResponseData GetResponse(CH_NG_196_MSG7_CargoExitFromCheckSite customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(CH_NG_196_MSG7_CargoExitFromCheckSite customResponse, GenericRequestParams requestParams)
        {

            try
            {
                ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
                PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(requestParams.Tenant);
                PhysicalCheckUpdateService physicalCheckUpdateService = new PhysicalCheckUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

                var id = physicalCheckQueryService.GetIdByCheckId(customResponse.generalDetails.checkId.ToString(), requestParams.Tenant);
                bool exitIfNotFound = false;
                if (string.IsNullOrWhiteSpace(id) && exitIfNotFound)
                {
                    //throw new System.Exception("CustomResponse.checkId:" + customResponse.generalDetails.checkId.ToString() + " not found in DB");
                    LogMessagingUtil.Instance.AppendLine("CustomResponse.checkId:" + customResponse.generalDetails.checkId.ToString() + " not found in DB");
                    this.MyResponseData = new INF_MSG_GenericResponseData();
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "Notice To Client: Can not found checkId number " + customResponse.generalDetails.checkId.ToString();
                    return;
                }

                PhysicalCheckPM phsicalCheckPM = null;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    phsicalCheckPM = physicalCheckQueryService.GetSingle(id, false, false);
                }
                if (phsicalCheckPM == null && exitIfNotFound)
                {
                    LogMessagingUtil.Instance.AppendLine("NoticeToClient: Can not found checkId number " + customResponse.generalDetails.checkId.ToString());
                    this.MyResponseData = new INF_MSG_GenericResponseData();
                    this.MyResponseData.ApplicationID = id;
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "NoticeToClient: Can not found checkId number " + customResponse.generalDetails.checkId.ToString();
                    return;
                }

                if(phsicalCheckPM == null)
                {
                    string description = "Physical Check Id number " + customResponse.generalDetails.checkId.ToString() + " (not found in DB)";
                    DoUpdateNotification("196E", customResponse, requestParams.Tenant, customResponse.generalDetails.checkId.ToString(), description, "A");
                    LogMessagingUtil.Instance.AppendLine("NoticeToClient: Can not found checkId number " + customResponse.generalDetails.checkId.ToString());
                    this.MyResponseData = new INF_MSG_GenericResponseData();
                    //this.MyResponseData.ApplicationID = id;
                    this.MyResponseData.Succeeded = true;
                    //this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "NoticeToClient: Can not found checkId number " + customResponse.generalDetails.checkId.ToString();
                    return;
                }

                //phsicalCheckPM.StorageSiteCode = customResponse.generalDetails.typeDestination.ToString();
                phsicalCheckPM.IsClosed = true;
                var myEventContextTagModel = new EventContextTagModel()
                {
                    CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_196_MSG7_CargoExitFromCheckSiteResponseServiceUpdate,
                    EventCode = "PCE",
                    EventRemarks = "End date: " + customResponse.generalDetails.endDate + ", Destination type: " + customResponse.generalDetails.typeDestination,
                    FUStatusCode = "PCE",
                    FUStatusRemarks = "End date: " + customResponse.generalDetails.endDate + ", Destination type: " + customResponse.generalDetails.typeDestination
                };
                {
                    List<EventContextTagModel> eventContextTagModelList = new List<EventContextTagModel>(); //Yuval Chalup 08.03.2016 TASK-19919
                    eventContextTagModelList.Add(myEventContextTagModel); //Yuval Chalup 08.03.2016 TASK-19919
                    var myEventContextTagModel2 = new EventContextTagModel();
                    switch (customResponse.generalDetails.typeDestination)
                    {
                        case 1: // End - moved to another site
                            //phsicalCheckPM.StorageSiteCode = customResponse.generalDetails.typeDestination.ToString();
                            phsicalCheckPM.OperationCode = "6";
                            //<--- Yuval Chalup 08.03.2016 TASK-19919 - Check if the Date has been changed - To raise also "STC" status
                            myEventContextTagModel2 = new EventContextTagModel()
                            {
                                CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_196_MSG7_CargoExitFromCheckSiteResponseServiceUpdate,
                                EventCode = "SPR",
                                EventRemarks = "מטען שוחרר מאתר משקף ללקוח",
                                FUStatusCode = "SPR",
                                FUStatusRemarks = "מטען שוחרר מאתר משקף ללקוח",
                            };
                            eventContextTagModelList.Add(myEventContextTagModel2);
                            //Yuval Chalup 08.03.2016 TASK-19919 --->
                            break;
                        case 2: // End - Moved to Custom
                            //phsicalCheckPM.StorageSiteCode = customResponse.generalDetails.typeDestination.ToString();
                            phsicalCheckPM.OperationCode = "5";
                            //<--- Yuval Chalup 08.03.2016 TASK-19919 - Check if the Date has been changed - To raise also "STC" status
                            myEventContextTagModel2 = new EventContextTagModel()
                            {
                                CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_196_MSG7_CargoExitFromCheckSiteResponseServiceUpdate,
                                EventCode = "SRF",
                                EventRemarks = "החזרת מטען מאתר משקף לאחסון",
                                FUStatusCode = "SRF",
                                FUStatusRemarks = "מטען שוחרר מאתר משקף ללקוח",
                            };
                            eventContextTagModelList.Add(myEventContextTagModel2);
                            //Yuval Chalup 08.03.2016 TASK-19919 --->
                            break;
                        default:
                            phsicalCheckPM.OperationCode = "7";
                            break;

                    }
                    phsicalCheckPM.CurrentContextTag = eventContextTagModelList; //Yuval Chalup 08.03.2016 TASK-19919
                }

                PhysicalCheckUpdateService updateService = new PhysicalCheckUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                phsicalCheckPM.ChangeSetOp = ChangeSetOperation.Update;

                //phsicalCheckPM.CurrentContextTag = myEventContextTagModel; //Yuval Chalup 08.03.2016 TASK-19919 (Replaced by Code for multi Status above )
                updateService.Update(phsicalCheckPM, true);

                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.ApplicationID = phsicalCheckPM.Id;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "שחרור מטען מאתר בדיקה " + phsicalCheckPM.CheckId;

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PhysicalCheck");
                this.MyRequestSheetParam.EntityId1 = phsicalCheckPM.Id;
                this.MyRequestSheetParam.RequestDescription = "שחרור מטען מאתר בדיקה " + phsicalCheckPM.CheckId;
                if (phsicalCheckPM.DeclarationId != null)
                {
                    var declarationQueryService = new DeclarationQueryService(dbContext);
                    string customfileNumber = declarationQueryService.GetCustomFileNoByDeclarationId(phsicalCheckPM.DeclarationId, phsicalCheckPM.Tenant);
                    this.MyRequestSheetParam.CustomFileNo = customfileNumber;
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = phsicalCheckPM.DeclarationId;

                    List<PhysicalCheckList> physicalchecks = physicalCheckQueryService.GetPhysicalChecksByDeclarationId(phsicalCheckPM.DeclarationId, phsicalCheckPM.Tenant);
                    Boolean physicalchecksclosed = true;
                    foreach (var item in physicalchecks)
                    {
                        if (item.CheckId == phsicalCheckPM.CheckId)
                        {
                            if (!phsicalCheckPM.IsClosed)
                            {
                                physicalchecksclosed = false;
                            }
                        }
                        else
                        {
                            if (!item.IsClosed)
                            {
                                physicalchecksclosed = false;
                            }
                        }
                    }
                    if (physicalchecksclosed)
                    {
                        var decPM = declarationQueryService.GetSingleDeclarationById(phsicalCheckPM.DeclarationId, phsicalCheckPM.Tenant);
                        DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                        decPM.PhysicalCheck = 2;
                        decPM.ChangeSetOp = ChangeSetOperation.Update;
                        declarationUpdateService.Update(decPM, true);
                    }

                }

            }
            catch (System.Exception ee)
            {
                ///MyResponseData = new INF_MSG_GenericResponseData() { HasException = true, ExceptionMessage = ee.ToString() };
                throw;
            }

        }

        private void DoUpdateNotification(string notificationDefinitionCode, CH_NG_196_MSG7_CargoExitFromCheckSite customResponse, int tenant, string reference1Number, string description, string assigneToNotificationTypeCode)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.AssigneToNotificationTypeCode = assigneToNotificationTypeCode;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.Reference1Number = reference1Number;

            string referentUserId = null;
            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, null, referentUserId, notificationDefinitionCode, "");

            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
