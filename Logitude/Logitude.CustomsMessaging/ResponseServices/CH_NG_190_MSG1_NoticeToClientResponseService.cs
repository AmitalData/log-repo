using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using UnifreightIIG.Common.MessageLib.PhysicalCheck190;
using Logitude.Customs.BL.TraceEvents;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityQueries;


namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CH_NG_190_MSG1_NoticeToClientResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, CH_NG_190_MSG1_NoticeToClient, GenericRequestParams>
    {

        private DeclarationPrintResponseData _SendDeclarationPrintResponse;


        public override void Update(CH_NG_190_MSG1_NoticeToClient customResponse, GenericRequestParams requestParams)
        {
            const string updatePhysicalCheck = "4";
            try
            {
                CH_NG_190_MSG1_NoticeToClientNoticeToClient NoticeToClient = customResponse.NoticeToClient;
                ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
                var physicalCheckQueryService = new PhysicalCheckQueryService(customContext);
                PhysicalCheckUpdateService physicalCheckUpdateService = new PhysicalCheckUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                PhysicalCheckPM physicalCheck = new PhysicalCheckPM();
                DeclarationPM myDeclarationPM = new DeclarationPM();
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                string declarationId = "";
                string declarationCustomerId = "";

                string DeclarationConvertionText = null;
                if (!string.IsNullOrWhiteSpace(NoticeToClient.declarationID))
                {
                    //<--- Yuval Chalup 19.11.2015 TASK-17450
                    myDeclarationPM = declarationUpdateService.GetSertByConvertedDeclarationNumber(NoticeToClient.declarationID.ToString(), requestParams.Tenant);
                    if (myDeclarationPM != null)
                    {
                        //If this is a Converted Declaration
                        if (myDeclarationPM.IsConvertedDeclaration)
                        {
                            DeclarationConvertionText = myDeclarationPM.UserNotes;
                            //SendDeclarationStatusRequest(myDeclarationPM);
                        }
                        declarationId = myDeclarationPM.Id;
                        declarationCustomerId = myDeclarationPM.CustomerId;
                    }
                    else
                    {
                        this.MyResponseData = new INF_MSG_GenericResponseData();
                        this.MyResponseData.Succeeded = false;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "Declaration not found";
                        return;
                    }
                    //Yuval Chalup 19.11.2015 TASK-17450 --->
                }

                if (string.IsNullOrWhiteSpace(declarationId))
                {
                    DeclarationQueryService myQueryService = new DeclarationQueryService(customContext);
                    if (customResponse.CheckEntity != null && customResponse.CheckEntity.cargoIdentifier != null && !string.IsNullOrWhiteSpace(customResponse.CheckEntity.cargoIdentifier.cargoIdentifierType.ToString()))
                    {
                        myDeclarationPM = myQueryService.GetDeclarationPMByCargoIdentifiers(customResponse.CheckEntity.cargoIdentifier.cargoIdentifierType.ToString(), customResponse.CheckEntity.cargoIdentifier.cargoIdentifierKey1, customResponse.CheckEntity.cargoIdentifier.cargoIdentifierKey2, requestParams.Tenant);
                    }
                    if (myDeclarationPM != null)
                    {
                        if (myDeclarationPM.IsConvertedDeclaration)
                        {
                            DeclarationConvertionText = myDeclarationPM.UserNotes;
                        }
                        declarationId = myDeclarationPM.Id;
                        declarationCustomerId = myDeclarationPM.CustomerId;
                    }
                    else
                    {
                        this.MyResponseData = new INF_MSG_GenericResponseData();
                        this.MyResponseData.Succeeded = false;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "Declaration not found";
                        return;
                    }
                       
                }

                if (string.IsNullOrWhiteSpace(declarationCustomerId) && customResponse.NoticeToClient.importerNumber!=null)
                {
/*task 35242 change from Client to Card 12/12/2017
                    ClientQueryService clientQueryService = new ClientQueryService(requestParams.Tenant);
                    ClientPM clientPM = clientQueryService.GetClientByCode(customResponse.NoticeToClient.importerNumber.ToString(), requestParams.Tenant);
                    if (clientPM != null)
                    {
                        declarationCustomerId = clientPM.Id;
                    }
*/
                    Card myCard = null;
                    var repository = new CardRepository (requestParams.Tenant);
                    myCard = repository.GetSingleCardByCode(customResponse.NoticeToClient.importerNumber.ToString(), requestParams.Tenant, false);
                    if (myCard != null)
                    {
                        declarationCustomerId = myCard.Id;
                    }
                }

                var id = physicalCheckQueryService.GetIdByCheckId(customResponse.NoticeToClient.checkId.ToString(), requestParams.Tenant);
                
                if (string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(DeclarationConvertionText))
                {
                    physicalCheck.ChangeSetOp = ChangeSetOperation.Insert;
                    physicalCheck.Tenant = requestParams.Tenant;
                    physicalCheck.CheckId = NoticeToClient.checkId.ToString();
                    physicalCheck.IsClosed = false;
                    physicalCheckUpdateService.Update(physicalCheck, true);
                    id = physicalCheckQueryService.GetIdByCheckId(customResponse.NoticeToClient.checkId.ToString(), requestParams.Tenant);
                }

                LogMessagingUtil.Instance.AppendLine("checkId = " + id);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    physicalCheck = physicalCheckQueryService.GetSingle(id, true, false);
                    LogMessagingUtil.Instance.AppendLine("NoticeToClient.operationCode = " + NoticeToClient.operationCode.ToString());
                    switch (NoticeToClient.operationCode.ToString())
                    {
                        case "1":
                        /*LogMessagingUtil.Instance.AppendLine("NoticeToClient.checkId:" + customResponse.NoticeToClient.checkId.ToString() + " already exist id=" + id);
                        this.MyResponseData = new INF_MSG_GenericResponseData();
                        this.MyResponseData.ApplicationID = physicalCheck.Id;
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = false;
                        this.MyResponseData.UserMessage = "NoticeToClient.checkId:" + customResponse.NoticeToClient.checkId.ToString() + " already exist id=" + id;
                        return;*/
                        case "2":
                            NoticeToClient.operationCode = 2;
                            physicalCheck.IsClosed = false;
                            var myUpdateEventContextTagModel = new EventContextTagModel()
                            {
                                CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceUpdate,
                                EventCode = "PUI",
                                EventRemarks = "Physical Checks Updated",
                                FUStatusRemarks = "עודכנו נתוני בדיקה פיזית מספר " + NoticeToClient.checkId + "\n" + "תאריך בדיקה: " + NoticeToClient.limitDate + "\n" + "אתר בדיקה: " + NoticeToClient.checkSiteNumber,
                            };
                            //<--- Yuval Chalup 19.11.2015 TASK-17450
                            if (!string.IsNullOrWhiteSpace(DeclarationConvertionText))
                            {
                                myUpdateEventContextTagModel.EventRemarks = myUpdateEventContextTagModel.EventRemarks + ". " + DeclarationConvertionText;
                                myUpdateEventContextTagModel.FUStatusRemarks = myUpdateEventContextTagModel.FUStatusRemarks + "\n" + DeclarationConvertionText;
                            }
                            //Yuval Chalup 19.11.2015 TASK-17450 --->
                            //<--- Yuval Chalup 08.03.2016 TASK-20003 - Check if the Date has been changed - To raise also "STC" status
                            List<EventContextTagModel> eventContextTagModelList = new List<EventContextTagModel>();
                            eventContextTagModelList.Add(myUpdateEventContextTagModel);
                            var limitDate = NoticeToClient.limitDateSpecified ? NoticeToClient.limitDate : null;
                            if (NoticeToClient.limitDate == null)
                            {
                                limitDate = NoticeToClient.openDate;
                            }
                            if (limitDate != physicalCheck.LimitDate)
                            {
                                var myUpdateEventContextTagModel2 = new EventContextTagModel()
                                {
                                    CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceUpdate,
                                    EventCode = "STC",
                                    EventRemarks = "Physical Checks Updated",
                                    FUStatusCode = "STC",
                                    FUStatusRemarks = "תאריך בדיקה קודם: " + physicalCheck.LimitDate + "\n" + "תאריך בדיקה חדש: " + limitDate,
                                };
                                if (!string.IsNullOrWhiteSpace(DeclarationConvertionText))
                                {
                                    myUpdateEventContextTagModel2.EventRemarks = myUpdateEventContextTagModel2.EventRemarks + ". " + DeclarationConvertionText;
                                    myUpdateEventContextTagModel2.FUStatusRemarks = myUpdateEventContextTagModel2.FUStatusRemarks + "\n" + DeclarationConvertionText;
                                }
                                if (string.Equals(physicalCheck.BringQueueForwardIndicatorS, updatePhysicalCheck, StringComparison.CurrentCultureIgnoreCase) )
                                {
                                    physicalCheck.BringQueueForwardIndicatorS = null;
                                }
                                eventContextTagModelList.Add(myUpdateEventContextTagModel2);
                            }

                            LogMessagingUtil.Instance.AppendLine("NoticeToClient.QueueType = " + NoticeToClient.QueueType?.ToString());
                            if (NoticeToClient.QueueType == 1 || NoticeToClient.QueueType == 3)
                            {
                                LogMessagingUtil.Instance.AppendLine("raise Event SFC , תיק זומן לבדיקה באתר משקף");
                                var myUpdateEventContextTagModel2 = new EventContextTagModel()
                                {
                                    CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceUpdate,
                                    EventCode = "SFC",
                                    EventRemarks = "תיק זומן לבדיקה באתר משקף (בדיקה פיזית מספר " + NoticeToClient.checkId + ")",
                                    FUStatusCode = "SFC",
                                    FUStatusRemarks = "תיק זומן לבדיקה באתר משקף (בדיקה פיזית מספר " + NoticeToClient.checkId + ")"
                                };
                                eventContextTagModelList.Add(myUpdateEventContextTagModel2);
                            }

                            physicalCheck.CurrentContextTag = eventContextTagModelList;
                           //Yuval Chalup 08.03.2016 TASK-20003 --->
                            //physicalCheck.CurrentContextTag = myUpdateEventContextTagModel; //Yuval Chalup 08.03.2016 TASK-20003 (Replaced by Code for multi Status above )
                            physicalCheck.ChangeSetOp = ChangeSetOperation.Update;
                            LogMessagingUtil.Instance.AppendLine("NoticeToClient.checkId:" + customResponse.NoticeToClient.checkId.ToString() + " Update");
                            break;
                        case "3":
                            var myDeleteEventContextTagModel = new EventContextTagModel()
                            {
                                CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceDelete,
                                EventCode = "PUC",
                                EventRemarks = "Physical Check Cancelled",
                                FUStatusRemarks = "בוטלה בדיקה פיזית מספר :" + NoticeToClient.checkId + " לתאריך" + NoticeToClient.limitDate,
                            };
                            //<--- Yuval Chalup 19.11.2015 TASK-17450
                            if (!string.IsNullOrWhiteSpace(DeclarationConvertionText))
                            {
                                myDeleteEventContextTagModel.EventRemarks = myDeleteEventContextTagModel.EventRemarks + ". " + DeclarationConvertionText;
                                myDeleteEventContextTagModel.FUStatusRemarks = myDeleteEventContextTagModel.FUStatusRemarks + "\n" + DeclarationConvertionText;
                            }
                            //Yuval Chalup 19.11.2015 TASK-17450 --->
                            physicalCheck.CurrentContextTag = myDeleteEventContextTagModel;
                            physicalCheck.ChangeSetOp = ChangeSetOperation.Update;
                            physicalCheck.IsClosed = true;
                            LogMessagingUtil.Instance.AppendLine("NoticeToClient.checkId:" + customResponse.NoticeToClient.checkId.ToString() + " Delete");
                            break;
                    }
                    
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("NoticeToClient.operationCode = " + NoticeToClient.operationCode.ToString());
                    if (NoticeToClient.operationCode == 2) NoticeToClient.operationCode = 1;
                    switch (NoticeToClient.operationCode.ToString())
                    {
                        case "1":
                            physicalCheck.ChangeSetOp = ChangeSetOperation.Insert;
                            List<EventContextTagModel> eventContextTagModelList = new List<EventContextTagModel>();
                            if (customResponse.CheckInstruction == null)
                            {
                                var myInsertEventContextTagModel = new EventContextTagModel()
                                {
                                    CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceInsert,
                                    EventCode = "PCF",
                                    EventRemarks = "נוצרה בדיקה פיזית מספר" + NoticeToClient.checkId + "תאריך בדיקה: " + NoticeToClient.limitDate + "אתר בדיקה: " + NoticeToClient.checkSiteNumber,
                                    FUStatusRemarks = "נוצרה בדיקה פיזית מספר " + NoticeToClient.checkId + "\n" + "תאריך בדיקה: " + NoticeToClient.limitDate + "\n" + "אתר בדיקה: " + NoticeToClient.checkSiteNumber,
                                };
                                //<--- Yuval Chalup 19.11.2015 TASK-17450
                                if (!string.IsNullOrWhiteSpace(DeclarationConvertionText))
                                {
                                    myInsertEventContextTagModel.EventRemarks = myInsertEventContextTagModel.EventRemarks + ". " + DeclarationConvertionText;
                                    myInsertEventContextTagModel.FUStatusRemarks = myInsertEventContextTagModel.FUStatusRemarks + "\n" + DeclarationConvertionText;
                                }
                                //Yuval Chalup 19.11.2015 TASK-17450 --->
                                eventContextTagModelList.Add(myInsertEventContextTagModel);
                                myDeclarationPM.PhysicalCheck = "1";
                                myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                                declarationUpdateService.Update(myDeclarationPM, true);

                            }
                            LogMessagingUtil.Instance.AppendLine("NoticeToClient.QueueType = " + NoticeToClient.QueueType?.ToString());

                            if (NoticeToClient.QueueType == 1 || NoticeToClient.QueueType == 3)
                            {
                                LogMessagingUtil.Instance.AppendLine("raise Event SFC , תיק זומן לבדיקה באתר משקף");

                                var myInsertEventContextTagModel2 = new EventContextTagModel()
                                {
                                    CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceUpdate,
                                    EventCode = "SFC",
                                    EventRemarks = "תיק זומן לבדיקה באתר משקף (בדיקה פיזית מספר " + NoticeToClient.checkId + ")",
                                    FUStatusCode = "SFC",
                                    FUStatusRemarks = "תיק זומן לבדיקה באתר משקף (בדיקה פיזית מספר " + NoticeToClient.checkId + ")"
                                };
                                eventContextTagModelList.Add(myInsertEventContextTagModel2);
                            }
                            physicalCheck.CurrentContextTag = eventContextTagModelList;
                            physicalCheck.IsClosed = false;
                            
                            break;
                        case "2":
                        case "3":
                            LogMessagingUtil.Instance.AppendLine("NoticeToClient: Can not found checkId number " + customResponse.NoticeToClient.checkId.ToString() + ", The operationCode is: " + NoticeToClient.operationCode.ToString());
                            //throw new System.Exception("NoticeToClient: Can not found checkId number " + customResponse.NoticeToClient.checkId.ToString() + " , The operationCode is: " + NoticeToClient.operationCode.ToString());
                            this.MyResponseData = new INF_MSG_GenericResponseData();
                            this.MyResponseData.ApplicationID = physicalCheck.Id;
                            this.MyResponseData.Succeeded = true;
                            this.MyResponseData.HasException = false;
                            this.MyResponseData.UserMessage = "NoticeToClient: Can not found checkId number " + customResponse.NoticeToClient.checkId.ToString() + " , The operationCode is: " + NoticeToClient.operationCode;
                            return;
                    }
                }


                ICommonDataContext commonDbContext = CommonDataContext.GetContext(myDeclarationPM.Tenant);
                UserRepository userRepository = new UserRepository(commonDbContext);
                var user = userRepository.GetSingleUserByCode("MEHES", myDeclarationPM.Tenant, true);

                var _requestDescription = "";//12192 -->
                switch (NoticeToClient.operationCode)
                {
                    case 1:
                        _requestDescription = "בדיקה פיזית חדשה ";

                        if(myDeclarationPM.Direction == "E") 
                        {                           
                            RaiseEvent(myDeclarationPM, user?.Id, status_id: "CHK");
                            if (myDeclarationPM.IsDiamondDeclaration)
                            {
                                var statusSoyRemarks = $"CODE-80-בדיקה-{myDeclarationPM.DeclarationNumber}" + "\n" + NoticeToClient.checkId+" ,"+ NoticeToClient.checkSiteNumber + " ,"+ NoticeToClient.storageSiteNumber;
                                RaiseEvent(myDeclarationPM, user?.Id, status_id: "SOY",comments: statusSoyRemarks);
                            }
                        }

                        break;
                    case 2:
                        _requestDescription = "עדכון בדיקה פיזית ";
                        break;
                    case 3:
                        _requestDescription = "בדיקה פיזית בוטלה ";
                        if(myDeclarationPM.Direction == "E")
                        {
                            RaiseEvent(myDeclarationPM, user?.Id, status_id: "SFA");
                        }    
                        
                        break;
                    default:
                        _requestDescription = "זימון לבדיקה ";
                        break;
                }
                _requestDescription = _requestDescription + NoticeToClient.checkId;


                physicalCheck.CargoIdentifierKey1 = customResponse.CheckEntity.cargoIdentifier.cargoIdentifierKey1;
                physicalCheck.CargoIdentifierKey2 = customResponse.CheckEntity.cargoIdentifier.cargoIdentifierKey2;
                physicalCheck.CargoIdentifierKey3 = customResponse.CheckEntity.cargoIdentifier.cargoIdentifierKey3;
                physicalCheck.CargoIdentifierTypeCode = customResponse.CheckEntity.cargoIdentifier.cargoIdentifierType.ToString();
                physicalCheck.ContainerNubmer = customResponse.CheckEntity.containerNumber;
                physicalCheck.RowNumber = customResponse.CheckEntity.rowNumber.ToString();

                physicalCheck.CargoTypeCode = NoticeToClient.entityType.ToString();
                physicalCheck.CheckId = NoticeToClient.checkId.ToString();
                physicalCheck.CheckSiteCode = NoticeToClient.checkSiteNumber?.ToString();

                physicalCheck.ImporterNumber = NoticeToClient.importerNumberSpecified ? NoticeToClient.importerNumber.ToString() : null;
                physicalCheck.InitiatorTypeCode = NoticeToClient.initiatorTypeSpecified ? NoticeToClient.initiatorType.ToString() : null;
                physicalCheck.LimitDate = NoticeToClient.limitDateSpecified ? NoticeToClient.limitDate : null;
                if (NoticeToClient.limitDate == null)
                {
                    physicalCheck.LimitDate = NoticeToClient.openDate;
                }

                physicalCheck.OpenDate = DateTime.Now;
                physicalCheck.OperationCode = NoticeToClient.operationCode.ToString();
                physicalCheck.QueueTypeCode = NoticeToClient.QueueTypeSpecified ? NoticeToClient.QueueType.ToString() : null;

                physicalCheck.StatusMessageCode = NoticeToClient.statusMessage.ToString();
                physicalCheck.StorageSiteCode = NoticeToClient.storageSiteNumber?.ToString();
                physicalCheck.Tenant = requestParams.Tenant;
                if (NoticeToClient.IsComprehensiveCheckSpecified == true)
                {
                    physicalCheck.IsComprehensiveCheck = (bool)NoticeToClient.IsComprehensiveCheck;
                }
                physicalCheck.CheckTypeCode = NoticeToClient.CheckType.ToString();
                physicalCheck.VehicleChassisNumber = NoticeToClient.VehicleChassisNumber;

                //<--- Yuval Chalup 19.11.2015 TASK-17450 - CHANGE FROM:
                //if (!string.IsNullOrWhiteSpace(NoticeToClient.declarationID))
                //{
                //    //DeclarationRepository declarationRep = new DeclarationRepository(customContext);
                //    //declarationPM = declarationRep.GetSingleDeclarationByNumber(declarationNo, requestParams.Tenant);
                //    DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);
                //    declarationId = declarationQueryService.GetIdByDeclarationNumber(NoticeToClient.declarationID.ToString(), requestParams.Tenant);
                //    if (string.IsNullOrWhiteSpace(declarationId))
                //    {
                //        LogMessagingUtil.Instance.AppendLine("NoticeToClient.declarationID = " + NoticeToClient.declarationID + " But DeclarationQueryService.GetSingle return null ");
                //    }
                //    else
                //    {
                //        physicalCheck.DeclarationId = declarationId;
                //    }
                //}
                //TO:
                if (string.IsNullOrWhiteSpace(declarationId))
                {
                    LogMessagingUtil.Instance.AppendLine("NoticeToClient.declarationID = " + NoticeToClient.declarationID + " But DeclarationUpdateService.GetSertByConvertedDeclarationNumber returned null DeclarationPM");
                }
                else
                {
                    physicalCheck.DeclarationId = declarationId;
                }
                //Yuval Chalup 19.11.2015 TASK-17450 --->

                if (string.IsNullOrWhiteSpace(declarationId))
                {
                    physicalCheck.CustomerId = declarationCustomerId;
                }

                if (customResponse.CheckInstruction != null && customResponse.CheckInstruction.Count() > 0)
                {
                    if (customResponse.CheckInstruction.Where(o => o.checkEssenceCode == 71).Count() > 0) physicalCheck.NoEscortRequired = true;
                    if (customResponse.CheckInstruction.Where(o => o.checkEssenceCode != 71).Count() > 0) physicalCheck.CheckEssence = customResponse.CheckInstruction.Where(o => o.checkEssenceCode != 71).FirstOrDefault().checkEssence;
                }

                physicalCheck.CustomsRequestsSheetId = requestParams.CustomsRequestsSheetId;
                physicalCheckUpdateService.Update(physicalCheck, true);

                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.ApplicationID = physicalCheck.Id;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.RequestDescription = _requestDescription;
                //<--- Yuval Chalup 19.11.2015 TASK-17450
                if (!string.IsNullOrWhiteSpace(DeclarationConvertionText))
                {
                    this.MyRequestSheetParam.RequestDescription = _requestDescription + "\n" + DeclarationConvertionText;
                }
                //Yuval Chalup 19.11.2015 TASK-17450 --->
                this.MyRequestSheetParam.EntityId1 = physicalCheck.Id;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PhysicalCheck");
                if (!string.IsNullOrWhiteSpace(declarationId))
                {
                    //this.MyRequestSheetParam.CustomFileNo = declarationPM.CustomFileNo;
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = declarationId;
                }

                if (myDeclarationPM.IsCourierDeclaration)
                {
                    SendDeclarationPrint(myDeclarationPM, requestParams);

                    FeatureQuery featureQuery = new FeatureQuery();
                    var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);
                    var feature = features.Features.FirstOrDefault(x => x.Code == "Pending900InDetainedOrPhysicalCheck");

                    if (NoticeToClient.operationCode == 1 && feature != null)
                    {


                        DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(customContext);
                        DeclarationCourierStatusPM _MyDeclarationCourierStatusPM = new DeclarationCourierStatusPM();
                        _MyDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(myDeclarationPM.Id, true, false);

                        var declarationPendingPM_900 = _MyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == _MyDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == "900").FirstOrDefault();

                        if (declarationPendingPM_900 == null)
                        {
                            declarationPendingPM_900 = new DeclarationPendingPM();
                            declarationPendingPM_900.CourierPendingReasonCode = "900";
                            declarationPendingPM_900.Status = "A";
                            declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Insert;
                            _MyDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_900);
                        }
                        else if (declarationPendingPM_900.Status != "A")
                        {
                            declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                            declarationPendingPM_900.Status = "A";
                        }
                        if (declarationPendingPM_900.ChangeSetOp != ChangeSetOperation.None)
                        {
                            if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) 
                                _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                        if (_MyDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
                        {
                            DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
                            //_MyDeclarationCourierStatusPM = declarationCourierStatusUpdateService.CalculateDeclarationCourierStatus(myDeclarationPM, _MyDeclarationCourierStatusPM: _MyDeclarationCourierStatusPM);
                            declarationCourierStatusUpdateService.Update(_MyDeclarationCourierStatusPM, true);
                        }
                       
                    }
                }

            }
            catch (System.Exception ee)
            {
                ///MyResponseData = new INF_MSG_GenericResponseData() { HasException = true, ExceptionMessage = ee.ToString() };
                throw;
            }

        }

        public override INF_MSG_GenericResponseData GetResponse(CH_NG_190_MSG1_NoticeToClient customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        void SendDeclarationStatusRequest(DeclarationPM myDeclarationPM)
        {
            var newSearchDeclarationStatusRequestParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,
                //LoggingUserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
                CustomFileNo = myDeclarationPM.CustomFileNo,
                DeclarationNumber = myDeclarationPM.DeclarationNumber,
                //CargoTypeCode = myDeclarationPM.Consignments[0].CargoTypeCode,
                //ManifestNumber = myDeclarationPM.Consignments[0].ManifestNumber,
                //SecondCargoID = myDeclarationPM.Consignments[0].SecondCargoID,
                //ThirdCargoID = myDeclarationPM.Consignments[0].ThirdCargoID,
                Tenant = myDeclarationPM.Tenant,
                RequestName = "Declaration Status Search (from Notice To Client Response)",
                ResponseName = "Declaration Status Search (from Notice To Client Response)",
                //TestCase = SelectedTest,
                CargoRadio = false,
                DeclarationRadio = true,
                OldReshimonRadio = false,
                OldReshimonNumber = null,
                LoggingEntityId = myDeclarationPM.Id,
                RequestVIA = SendRequestVIA.WebServiceBatch,
            };

            var service = new DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService();
            var responseData = service.Send(newSearchDeclarationStatusRequestParams);
            if (!responseData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Request Failed " + responseData.CustomsRequestsSheetId + ", Message: " + responseData.UserMessage);
                return;
            }
            LogMessagingUtil.Instance.AppendLine("Request Succeeded " + responseData.CustomsRequestsSheetId);
        }


        private static void RaiseEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string status_id,string comments = null)
        {

            try
            {

                string primary_number = $"{dirtyDeclarationPM.CustomFileNo},EFIFILEM";
                if (dirtyDeclarationPM.TransportModeId != "A")
                {
                    primary_number = $"{dirtyDeclarationPM.CustomFileNo},MFIFILEM";
                }

                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = status_id,
                    notes = "",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,
    
                    CommunicationSubject = "FU Status " + status_id + " from logitude",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname =  "BFIFILE",
                        primary_number = primary_number,
                        status = "new",
                        xml_status = "new",
                        status_id = status_id,
                        status_DateTime = DateTime.Now,
                        comments = comments!=null? comments:dirtyDeclarationPM.Id,
    
    
                    }
                };
                bool isExport = dirtyDeclarationPM?.Direction == "E" ? true : false;
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, suppress_RAISE_EVENT: true, iscustomUser: true, isExport: isExport);
            }
            catch(System.Exception ex)
            {
                throw;
            }


        }

        public bool SendDeclarationPrint(DeclarationPM myDeclarationPM, GenericRequestParams requestParams)
        {
            LogMessagingUtil.Instance.AppendLine("SendDeclarationPrint");
            string decNum = myDeclarationPM.DeclarationNumber;
            var decNumList = new List<string>();
            decNumList.Add(decNum);
            DF_NG_8302_Web03_DeclarationPrintRequestParams searchParams = new DF_NG_8302_Web03_DeclarationPrintRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = myDeclarationPM.CustomFileNo,
                DeclarationNumber = decNumList, //declarationPM.DeclarationNumber,
                Tenant = myDeclarationPM.Tenant,
                RequestName = "Declaration Print(190)",
                ResponseName = "Declaration Print(190)",
                LoggingEntityId = myDeclarationPM.Id,
                RequestVIA = SendRequestVIA.WebServiceBatch,




                LoggingUserId = requestParams.LoggingUserId,
            };



            var myRequestMessagingService = new DF_NG_8302_Web03_DeclarationPrintMessagingService();
            var resData = myRequestMessagingService.Send(searchParams);
            _SendDeclarationPrintResponse = resData;
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Declaration Print Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                return false;
            }
            LogMessagingUtil.Instance.AppendLine("Declaration Print Request Succeeded " + resData.CustomsRequestsSheetId);
            return true;
        }

    }

}
