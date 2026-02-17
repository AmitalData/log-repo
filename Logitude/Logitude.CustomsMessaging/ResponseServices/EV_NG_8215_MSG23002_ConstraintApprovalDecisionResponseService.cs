using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.MessageLib.Collateral;
using UnifreightIIG.Common.MessageLib.Constraint;
using UnifreightIIG.Common.MessageLib.Ransom;



namespace Logitude.CustomsMessaging.ResponseServices
{
    public class EV_NG_8215_MSG23002_ConstraintApprovalDecisionResponseService
        : ResponseServiceBase<INF_MSG_GenericResponseData, EV_NG_8215_MSG23002_ConstraintApprovalDecision, GenericRequestParams>
    {
        public override void Update(EV_NG_8215_MSG23002_ConstraintApprovalDecision customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 8215 - Constraint Approval (DCA)
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(dbContext);
            var declarationConstraintQueryService = new DeclarationConstraintQueryService(dbContext);
            var declarationConstraintUpdateService = new DeclarationConstraintUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            string description = "אילוץ - " + customResponse.ConstraintApprovalDecision.constraintId.ToString();

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = description;

            var declaration = declarationQueryService.GetIdByDeclarationNumber(customResponse.ConstraintApprovalDecision.LeadDocumentIDNum, requestParams.Tenant);
            if (declaration == null)
            {
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can't find declarationConstraint: \nDeclarationId:" + customResponse.ConstraintApprovalDecision.LeadDocumentIDNum + " ConstraintId:" + customResponse.ConstraintApprovalDecision.constraintId;
                LogMessagingUtil.Instance.AppendLine("ConstraintApprovalDecision: \nDeclarationId:" + customResponse.ConstraintApprovalDecision.LeadDocumentIDNum + " is missing");
                return;
            }

            DeclarationPM declarationPM = declarationQueryService.GetSingle(declaration, false, false);
            DeclarationConstraintPM declarationConstraintPM = declarationConstraintQueryService.GetSingle(declaration, customResponse.ConstraintApprovalDecision.constraintId.ToString(), true, false);
            if (declarationConstraintPM == null)
            {
                this.MyRequestSheetParam.EntityId1 = declaration;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");

                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can't find declarationConstraint: \nDeclarationId:" + customResponse.ConstraintApprovalDecision.LeadDocumentIDNum + " ConstraintId:" + customResponse.ConstraintApprovalDecision.constraintId;
                LogMessagingUtil.Instance.AppendLine("ConstraintApprovalDecision, Can't find declarationConstraint: \nDeclarationId:" + customResponse.ConstraintApprovalDecision.LeadDocumentIDNum + " ConstraintId:" + customResponse.ConstraintApprovalDecision.constraintId);
                return;
            }

            LogMessagingUtil.Instance.AppendLine("Analyze message Constraint Approval for " + declarationConstraintPM.ConstraintNumber);

            declarationConstraintPM.ChangeSetOp = ChangeSetOperation.Update;
            if (customResponse.ConstraintApprovalDecision.ConstraintStatus > 0)
            {
                declarationConstraintPM.ConstraintStatusCode = customResponse.ConstraintApprovalDecision.ConstraintStatus.ToString();
                EventContextTagModel myInsertEventContextTagModel = null;
                switch (customResponse.ConstraintApprovalDecision.approvalDecision)//Eitan h 26/5/15 13631
                //switch (customResponse.ConstraintApprovalDecision.ConstraintStatus)
                {
                    //case 4: // Deny
                    //case 7:
                    //case 8:
                    case 1://Eitan h 26/5/15 13631
                        myInsertEventContextTagModel = new EventContextTagModel()
                        {
                            CallProccessID = EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceDeny,
                            EventCode = "CDC",
                            EventRemarks = "Constraint Declined by Customs",
                        };
                        description = "אילוץ - " + customResponse.ConstraintApprovalDecision.constraintId.ToString() + " נדחה";
                        break;
                    //case 5: // Approved
                    case 2://Eitan h 26/5/15 13631
                        myInsertEventContextTagModel = new EventContextTagModel()
                        {
                            CallProccessID = EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceApproved,
                            EventCode = "RAM",
                            EventRemarks = "Constraint Approved by Customs",
                        };
                        description = "אילוץ - " + customResponse.ConstraintApprovalDecision.constraintId.ToString() + " אושר";
                        break;
                    //case 1: // Conditional Approval
                    //case 2:
                    case 3://Eitan h 26/5/15 13631
                        myInsertEventContextTagModel = new EventContextTagModel()
                        {
                            CallProccessID = EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceConditionalApproval,
                            EventCode = "CDA",
                            EventRemarks = "Constraint Conditional Approval",

                        };
                        description = "אילוץ - " + customResponse.ConstraintApprovalDecision.constraintId.ToString() + " אושר בתנאי";
                        break;
                }
                declarationConstraintPM.CurrentContextTag = myInsertEventContextTagModel;
            }
            declarationConstraintPM.ApprovalNote = customResponse.ConstraintApprovalDecision.approvalNote;
            declarationConstraintPM.ApprovalAuthorityDate = customResponse.ConstraintApprovalDecision.approvalAuthorityDate;
            declarationConstraintPM.ApprovalUserName = customResponse.ConstraintApprovalDecision.approvalUserNam;
            if (customResponse.ConstraintApprovalDecision.approvalDecision > 0)
            {
                declarationConstraintPM.ApprovalDecision = customResponse.ConstraintApprovalDecision.approvalDecision.ToString();
            }
            declarationConstraintPM.CustomsRequestsSheetId = requestParams.CustomsRequestsSheetId;
            declarationConstraintUpdateService.Update(declarationConstraintPM, true);

            this.MyRequestSheetParam.EntityId1 = declarationConstraintPM.ConstraintNumber;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.DeclarationConstraint");
            this.MyRequestSheetParam.EntityId2 = declarationConstraintPM.DeclarationID;
            this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.CustomFileNo = declarationPM.CustomFileNo;
            this.MyRequestSheetParam.RequestDescription = description;

            if (customResponse.ConstraintApprovalDecision.CollateralRequestMsg != null) // Create Collateral
            {
                LogMessagingUtil.Instance.AppendLine("ConstraintApprovalDecision: Create Collateral");
                var requestXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Constraint.CollateralRequestDetails[]>
                    .SerializeObject(customResponse.ConstraintApprovalDecision.CollateralRequestMsg);
                var collateralArry = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.CollateralRequestDetails[]>.DeSerializeObject(requestXml);

                COLT_NG_8211_MSG10040_CollateralRequestMsg myCOLT_NG_8211_MSG10040_CollateralRequestMsg = new COLT_NG_8211_MSG10040_CollateralRequestMsg();
                var responseContentHeader = customResponse.GetResponseContentHeader();
                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader = new ResponseContentHeader();
                if (responseContentHeader!=null)
                {
                    myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.ApplicationID = responseContentHeader.ApplicationID;
                    myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.Remark = responseContentHeader.Remark;
                    myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.TransmitionDateTime = responseContentHeader.TransmitionDateTime;
                }
                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.CollateralRequestDetails = collateralArry;
                var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>
                    .SerializeObject(myCOLT_NG_8211_MSG10040_CollateralRequestMsg);

                var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>.DeSerializeObject(xml);
                var DF_MSG10040_CollateralRequestMsgResponseService = new DF_8211_CollateralRequestMsgResponseService();
                DF_MSG10040_CollateralRequestMsgResponseService.Update(ser, requestParams);
            }

            if (customResponse.ConstraintApprovalDecision.DocumentRequestDetails != null) // Create Document 
            {
                LogMessagingUtil.Instance.AppendLine("ConstraintApprovalDecision: Create Document");
                var headerXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Constraint.RequestContentHeader>
                                .SerializeObject(customResponse.RequestContentHeader);
                var header = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Ransom.RequestContentHeader>.DeSerializeObject(headerXml);

                foreach (var documentItem in customResponse.ConstraintApprovalDecision.DocumentRequestDetails)
                {
                    //Get Document Detail
                    var documentXml = new UnifreightIIG.Common.MessageLib.Ransom.RequiredDocumentDetails();
                    documentXml.documentID = documentItem.documentID;
                    documentXml.remarks = documentItem.remarks;
                    documentXml.requiredDocumentMessageType = documentItem.requiredDocumentMessageType;
                    documentXml.typeID = documentItem.typeID.ToString();
                    //Get Entity Details
                    RequiredDocumentRequestParams documentRequestParams = new RequiredDocumentRequestParams();
                    documentRequestParams.Tenant = requestParams.Tenant;
                    documentRequestParams.ParentEntityCode = "Declaration";
                    documentRequestParams.ParentEntityId = declarationConstraintPM.DeclarationID;
                    documentRequestParams.Child1EntityCode = "Constraint";
                    documentRequestParams.Child1EntityId = declarationConstraintPM.ConstraintNumber;
                    var listConnectedEntity = new List<UnifreightIIG.Common.MessageLib.Ransom.ConnectedEntity>();
                    var entityXml = new UnifreightIIG.Common.MessageLib.Ransom.ConnectedEntity();
                    entityXml.entityType = 1055;
                    entityXml.entityIdKey1 = customResponse.ConstraintApprovalDecision.LeadDocumentIDNum;
                    listConnectedEntity.Add(entityXml);
                    
                    VAL_NG_8227_MSG_520_RequiredDocumentMessage myVAL_NG_8227_MSG_520_RequiredDocumentMessage = new VAL_NG_8227_MSG_520_RequiredDocumentMessage();
                    myVAL_NG_8227_MSG_520_RequiredDocumentMessage.RequestContentHeader = header;
                    myVAL_NG_8227_MSG_520_RequiredDocumentMessage.RequiredDocumentDetails = documentXml;

                    myVAL_NG_8227_MSG_520_RequiredDocumentMessage.RelatedEntity = listConnectedEntity.ToArray();
                    if (customResponse.ConstraintApprovalDecision.CollateralRequestMsg != null)
                    {
                        var worker = new UnifreightIIG.Common.MessageLib.Ransom.Worker();
                        worker.customsHouse = customResponse.ConstraintApprovalDecision.CollateralRequestMsg[0].Worker.customsHouse;
                        worker.organizationUnitType = customResponse.ConstraintApprovalDecision.CollateralRequestMsg[0].Worker.organizationUnitType;
                        worker.workerName = customResponse.ConstraintApprovalDecision.CollateralRequestMsg[0].Worker.workerName;
                        myVAL_NG_8227_MSG_520_RequiredDocumentMessage.Worker = worker;
                    }
                    var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Ransom.VAL_NG_8227_MSG_520_RequiredDocumentMessage>
                        .SerializeObject(myVAL_NG_8227_MSG_520_RequiredDocumentMessage);

                    var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Ransom.VAL_NG_8227_MSG_520_RequiredDocumentMessage>.DeSerializeObject(xml);
                    var myVAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService = new VAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService();
                    myVAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService.Update(ser, documentRequestParams);
                }
            }


            if (declaration != null)
            {
                string returnMessage = SendDeclarationStatus(declarationPM, declarationConstraintPM);
                description = description + "\n" + returnMessage;
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = declarationConstraintPM.ConstraintNumber,
                Succeeded = true,
                UserMessage = description,
            };
        }


        public override INF_MSG_GenericResponseData GetResponse(EV_NG_8215_MSG23002_ConstraintApprovalDecision customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public string SendDeclarationStatus(DeclarationPM declarationPM, DeclarationConstraintPM declarationConstraintPM) // moran 28.12.14 - Task 9739
        {
            string returnUserMessage = "";
            DeclarationStatusRequestParams searchParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,

                CustomFileNo = declarationPM.CustomFileNo,
                DeclarationNumber = declarationPM.DeclarationNumber,
                Tenant = declarationPM.Tenant,
                RequestName = "Declaration Status Search",
                ResponseName = "Declaration Status Search",
                SuppressSplitWR = true
            };

            LogMessagingUtil.Instance.AppendLine("Start sending message DeclarationStatus(8250)");
            searchParams.RequestVIA = SendRequestVIA.WebServiceBatch;
            var myRequestMessagingService = new DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService();
            DeclarationStatusResponseData responseData = myRequestMessagingService.Send(searchParams);
            if (!responseData.Succeeded)
            {
                returnUserMessage = "שליחת מסר עדכון סטטוס הצהרה נכשל";
                LogMessagingUtil.Instance.AppendLine("Send Declaration Status Request Failed " + responseData.CustomsRequestsSheetId + "\n" + "UserMessage: " + responseData.UserMessage + " WarningMessage: " + responseData.WarningMessage);
                return returnUserMessage;
            }
            returnUserMessage = "נשלח ברקע מסר לעידכון סטטוס";
            LogMessagingUtil.Instance.AppendLine("Send Declaration Status Request Succeeded " + responseData.CustomsRequestsSheetId + " DeclarationStatusCode: " + responseData.DeclarationStatusCode);

            return returnUserMessage;
        }
    }
}
