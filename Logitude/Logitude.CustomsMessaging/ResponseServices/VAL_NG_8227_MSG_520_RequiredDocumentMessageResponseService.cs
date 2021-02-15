using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Ransom;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService: ResponseServiceBase
        <RequiredDocumentResponseData, VAL_NG_8227_MSG_520_RequiredDocumentMessage, RequiredDocumentRequestParams>
    {

        public override void Update(VAL_NG_8227_MSG_520_RequiredDocumentMessage customResponse, RequiredDocumentRequestParams requestParams)
        {
            //Analyze message 8227 - Required Document (DCA)
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var myCustomsDocumentQueryService = new CustomsDocumentQueryService(dbContext);
            var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(dbContext);
            var myCustomsDocumentPointerUpdateService = new CustomsDocumentPointerUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(dbContext);
            var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(dbContext);
            string DeclarationConvertionText = "";

            CustomsDocumentPointerPM customsDocumentPointerPM = new CustomsDocumentPointerPM();
            CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();

            string requestId = null;
            string requestEntity = null;
            EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "מסר מסמך נדרש " + customResponse.RequiredDocumentDetails.documentID.ToString();
            if (customResponse.RequiredDocumentDetails.requiredDocumentMessageType == 2) 
            {
                this.MyRequestSheetParam.RequestDescription = "ביטול דרישת מסמך " + customResponse.RequiredDocumentDetails.documentID.ToString();
            }

            string docParentEntityCode = null;
            string docParentEntityId = null;
            string docChild1EntityCode = null;
            string docChild1EntityId = null;
            string docChild2EntityCode = null;
            string docChild2EntityId = null;
            DeclarationPM myDeclarationPM = null;

            InitMyResponseData(customResponse, requestParams); //Get Message Details (Display in RequestsSheetMassagingView)

            if (!String.IsNullOrWhiteSpace(requestParams.ParentEntityCode) && !String.IsNullOrWhiteSpace(requestParams.ParentEntityId))
            {
                docParentEntityCode = requestParams.ParentEntityCode;
                docParentEntityId = requestParams.ParentEntityId;

                if (docParentEntityCode == "Declaration" && !string.IsNullOrWhiteSpace(docParentEntityId))
                {
                    myDeclarationPM = myDeclarationQueryService.GetSingle(docParentEntityId, true, false);
                    requestId = myDeclarationPM.Id;
                    requestEntity = "Customs.Declaration";
                }

                if (!String.IsNullOrWhiteSpace(requestParams.Child1EntityCode) && !String.IsNullOrWhiteSpace(requestParams.Child1EntityId))
                {
                    docChild1EntityCode = requestParams.Child1EntityCode;
                    docChild1EntityId = requestParams.Child1EntityId;
                }
                if (!String.IsNullOrWhiteSpace(requestParams.Child2EntityCode) && !String.IsNullOrWhiteSpace(requestParams.Child2EntityId))
                {
                    docChild2EntityCode = requestParams.Child2EntityCode;
                    docChild2EntityId = requestParams.Child2EntityId;
                }
            }
            else
            {
                var firstRelatedEntity = customResponse.RelatedEntity.FirstOrDefault();
                if (customResponse.RelatedEntity != null &&
                    (firstRelatedEntity.entityType == 1055 || firstRelatedEntity.entityType == 11157 || firstRelatedEntity.entityType == 11184 || firstRelatedEntity.entityType == 11185 || firstRelatedEntity.entityType == 12414 || firstRelatedEntity.entityType == 11188 || firstRelatedEntity.entityType == 12397 || firstRelatedEntity.entityType == 12396)) //1055 or 11157 = Declaration //11184 = SupplierInvoice //11185 = SupplierInvoiceItem
                {
                    //Search Declaration by entityIdKey1
                    if (customResponse.RequiredDocumentDetails.requiredDocumentMessageType == 1)
                    {
                        DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                        myDeclarationPM = declarationUpdateService.GetSertByConvertedDeclarationNumber(firstRelatedEntity.entityIdKey1, requestParams.Tenant,true);
                    }
                    else if (customResponse.RequiredDocumentDetails.requiredDocumentMessageType == 2)
                    {
                        //myDeclarationPM = myDeclarationQueryService.GetSingle(firstRelatedEntity.entityIdKey1, true, false);
                        myDeclarationPM = myDeclarationQueryService.GetSingleDeclarationByNumber(firstRelatedEntity.entityIdKey1, requestParams.Tenant,true);
                    }
                    if (myDeclarationPM == null || string.IsNullOrWhiteSpace(myDeclarationPM.Id))
                    {
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "Can't find declaration: \nDeclaration: " + firstRelatedEntity.entityIdKey1;
                        LogMessagingUtil.Instance.AppendLine("RequiredDocumentMessage: \nDeclaration: " + firstRelatedEntity.entityIdKey1 + " is missing");
                        return;
                    }

                    if (myDeclarationPM.IsConvertedDeclaration)
                    {
                        DeclarationConvertionText = "\n" + myDeclarationPM.UserNotes;
                    }

                    docParentEntityCode = "Declaration";
                    docParentEntityId = myDeclarationPM.Id;

                    requestId = myDeclarationPM.Id;
                    requestEntity = "Customs.Declaration";

                    //Search Supplier Invoice by entityIdKey1 + entityIdKey2
                    if (!string.IsNullOrWhiteSpace(firstRelatedEntity.entityIdKey2) && myDeclarationPM.IsConvertedDeclaration != true)
                    {
                        int supplierInvoiceSeq = 0;
                        int.TryParse(firstRelatedEntity.entityIdKey2, out supplierInvoiceSeq);
                        var supplierInvoicePM = myDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.SequenceNumeric == supplierInvoiceSeq);

                        if (supplierInvoicePM != null)
                        {
                            docChild1EntityCode = "SupplierInvoice";
                            docChild1EntityId = supplierInvoicePM.InvoiceCounterKey.ToString();

                            //Search Supplier Invoice Item by entityIdKey1 + entityIdKey2 + entityIdKey3
                            int supplierInvoiceItemSeq = 0;
                            int.TryParse(firstRelatedEntity.entityIdKey3, out supplierInvoiceItemSeq);
                            var supplierInvoiceItemPM = supplierInvoicePM.SupplierInvoiceItems.FirstOrDefault(si => si.SequenceNumeric == supplierInvoiceItemSeq);

                            if (supplierInvoiceItemPM != null)
                            {
                                docChild2EntityCode = "SupplierInvoiceItem";
                                docChild2EntityId = supplierInvoiceItemPM.LineNumber.ToString();
                            }
                        }
                    }
                }
                else
                {
                    //Search Declaration by cargo (Consignment Table)
                    var myConsignmentQueryService = new ConsignmentQueryService(dbContext);
                    var myDeclaration = myConsignmentQueryService.GetDeclarationIdByConsignmentCargoId(firstRelatedEntity.entityIdKey1, firstRelatedEntity.entityIdKey2, firstRelatedEntity.entityIdKey3, requestParams.Tenant);
                    if (string.IsNullOrWhiteSpace(myDeclaration))
                    {
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "Can't find declaration: \nDeclaration: " + firstRelatedEntity.entityIdKey1;
                        LogMessagingUtil.Instance.AppendLine("RequiredDocumentMessage: \nDeclaration: " + firstRelatedEntity.entityIdKey1 + " is missing");
                        return;
                    }

                    docParentEntityCode = "Declaration";
                    docParentEntityId = myDeclaration;

                    myDeclarationPM = myDeclarationQueryService.GetSingle(myDeclaration, true, false);
                    requestId = myDeclarationPM.Id;
                    requestEntity = "Customs.Declaration";
                }
            }

            string workerRemarks = "";
            if (customResponse.Worker != null)
            {
                if (customResponse.Worker.customsHouse > 0)
                {
                    CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(requestParams.Tenant);
                    CustomsHouseTypePM customsHouseTypePM = customsHouseTypeQueryService.GetSingle(customResponse.Worker.customsHouse.ToString(), false, true);
                    workerRemarks = "תחנת מכס-" + customsHouseTypePM.LocalName;
                }
                if (customResponse.Worker.organizationUnitType > 0)
                {
                    OrganizationUnitTypeQueryService organizationUnitTypeQueryService = new OrganizationUnitTypeQueryService(requestParams.Tenant);
                    OrganizationUnitTypePM organizationUnitTypePM = organizationUnitTypeQueryService.GetSingle(customResponse.Worker.organizationUnitType.ToString(), false, true);
                    workerRemarks = workerRemarks + " \n " + "יחידה מקצועית-" + organizationUnitTypePM.LocalName;
                }
                if (!string.IsNullOrWhiteSpace(customResponse.Worker.workerName))
                {
                    workerRemarks = workerRemarks + " \n " + "עובד מכס-" + customResponse.Worker.workerName;
                }
            }
            workerRemarks = string.Concat(customResponse.RequiredDocumentDetails.remarks, "\n", workerRemarks);//Eitan H 6/6/18 Bug 39871: 8227 Notification display call# 310246 (make same remarks for all uses)
            var isNew = true;
            if (customResponse.RequiredDocumentDetails.requiredDocumentMessageType == 1) // Craete new CustomsDocumentPointers
            {
                var myDocumentId = myCustomsDocumentQueryService.GetDocumentInIdByCustomsDocId(customResponse.RequiredDocumentDetails.documentID.ToString(), requestParams.Tenant);
                this.MyRequestSheetParam.RequestDescription = "מסמך נדרש " + customResponse.RequiredDocumentDetails.documentID.ToString() + DeclarationConvertionText;

                List<CustomsDocumentPointerPM> customsDocumentPointerList= myCustomsDocumentPointerQueryService.GetCustomsDocumentPointerPMsByRequiredDocID(customResponse.RequiredDocumentDetails.documentID.ToString(), requestParams.Tenant);
                if (customsDocumentPointerList == null || customsDocumentPointerList.Count == 0)
                {
                    customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Insert;
                    customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Insert;
                    isNew = true;

                }
                else
                {
                    customsDocumentPointerPM = myCustomsDocumentPointerQueryService.GetSingle(customsDocumentPointerList.FirstOrDefault().Id, true, false);
                    customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Update;
                    customsDocumentsTicketPM = myCustomsDocumentsTicketQueryService.GetSingle(customsDocumentPointerPM.CustomsDocumentsTicketId,true,false);
                    customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Update;
                    isNew = false;
                    if (!string.IsNullOrWhiteSpace(customsDocumentsTicketPM.DocumentsFilingId))
                    {
                        this.MyResponseData.ApplicationID = requestId;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "קיימת כבר בקשה לדרישה למסמך מספר " + customResponse.RequiredDocumentDetails.documentID.ToString() + " ולדרישה זו כבר קושר מסמך " + "\n" + "לא בוצע ניתוח למסר זה";

                        this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName(requestEntity);
                        this.MyRequestSheetParam.EntityId1 = requestId;
                        LogMessagingUtil.Instance.AppendLine("קיימת כבר בקשה לדרישה למסמך מספר " + customResponse.RequiredDocumentDetails.documentID.ToString() + " ולדרישה זו כבר קושר מסמך ");
                        return;
                    }
                }

                // Create Documents Ticket 
                customsDocumentsTicketPM.Tenant = requestParams.Tenant;              
                customsDocumentsTicketPM.DocumentsFilingId = myDocumentId;
                customsDocumentsTicketPM.DocumentTypeCode = customResponse.RequiredDocumentDetails.typeID.ToString();
                //customsDocumentsTicketPM.Remarks = string.Concat(customResponse.RequiredDocumentDetails.remarks,"\n",workerRemarks);//Eitan H 6/6/18 Bug 39871: 8227 Notification display call# 310246 (make same remarks for all uses)
                customsDocumentsTicketPM.Remarks = workerRemarks;
                customsDocumentsTicketPM.RequestedCustomsDocId = customResponse.RequiredDocumentDetails.documentID.ToString(); // bug 35587 - remove initiation of status 3 - call 297998
                myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);

                // Create Document Pointer (every document pointer has a ticket)
                customsDocumentPointerPM.Tenant = requestParams.Tenant;
                customsDocumentPointerPM.CustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                customsDocumentPointerPM.ParentEntityCode = docParentEntityCode;
                customsDocumentPointerPM.ParentEntityId = docParentEntityId;
                customsDocumentPointerPM.Child1EntityCode = docChild1EntityCode;
                customsDocumentPointerPM.Child1EntityId = docChild1EntityId;
                customsDocumentPointerPM.Child2EntityCode = docChild2EntityCode;
                customsDocumentPointerPM.Child2EntityId = docChild2EntityId;
              
                //customsDocumentPointerPM.DocumentStatusCode = "3";
                customsDocumentPointerPM.DocumentRemarks = customsDocumentsTicketPM.Remarks;
                customsDocumentPointerPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                //Create Notification
                if(isNew)
                {
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.VAL_NG_8227_MSG_520_RequiredDocumentMessageInsert;
                    myInsertEventContextTagModel.EventCode = "CRD";

                   if(myDeclarationPM.Direction=="E")
                    {
                        myInsertEventContextTagModel.EventRemarks = "DocumentID: " + customResponse.RequiredDocumentDetails.documentID + '\n' + "TypeID: " + customResponse.RequiredDocumentDetails.typeID + '\n' +   "RequiredDocumentMessageType: New";

                    }
                    else

                    {
                        myInsertEventContextTagModel.EventRemarks = "Document Request By Customs" + DeclarationConvertionText;

                    }

                    myDeclarationPM.RequestedCustomsDocId = 1; 

                    myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

                }
                requestParams.LoggingObjectTableId = customsDocumentPointerPM.ParentEntityCode;
                requestParams.LoggingEntityId = customsDocumentPointerPM.ParentEntityId;
            }
                
            else if (customResponse.RequiredDocumentDetails.requiredDocumentMessageType == 2) // Delete CustomsDocumentPointer
            {
                this.MyRequestSheetParam.RequestDescription = "ביטול דרישת מסמך " + customResponse.RequiredDocumentDetails.documentID.ToString() + DeclarationConvertionText;
                List<CustomsDocumentPointerPM> customsDocumentPointerList = myCustomsDocumentPointerQueryService.GetCustomsDocumentPointerPMsByRequiredDocID(customResponse.RequiredDocumentDetails.documentID.ToString(),requestParams.Tenant);
                if (customsDocumentPointerList != null)
                {
                    foreach (var customsDocumentPointerItem in customsDocumentPointerList)
                    {
                        if (customsDocumentPointerItem.ParentEntityCode == docParentEntityCode && customsDocumentPointerItem.ParentEntityId == docParentEntityId)
                        {
                            // Delete Documents Ticket 
                            customsDocumentsTicketPM = myCustomsDocumentsTicketQueryService.GetSingle(customsDocumentPointerItem.CustomsDocumentsTicketId, false, false);
                            customsDocumentPointerPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                            customsDocumentPointerPM.DocumentRemarks = customResponse.RequiredDocumentDetails.remarks;
                            customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Delete;
                            myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);

                            // Delete Document Pointer
                            customsDocumentPointerPM = customsDocumentPointerItem;
                            customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Delete;

                            //Create Notification
                            myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.VAL_NG_8227_MSG_520_RequiredDocumentMessageDelete;
                            myInsertEventContextTagModel.EventCode = "CRC";


                            if (myDeclarationPM.Direction == "E")
                            {
                                myInsertEventContextTagModel.EventRemarks = "DocumentID: " + customResponse.RequiredDocumentDetails.documentID + '\n' + "TypeID: " + customResponse.RequiredDocumentDetails.typeID + '\n' + "RequiredDocumentMessageType: Delete";

                            }
                            else
                            {
                                myInsertEventContextTagModel.EventRemarks = "Document requested Cancelled" + DeclarationConvertionText;

                            }
                            break;
                        }
                    }

                   var requestedCustomsDocId = myCustomsDocumentsTicketQueryService.CheckRequestedCustomsDocIdsByEntityIdAndChilds(myDeclarationPM.Id, myDeclarationPM.Tenant, "", customResponse.RequiredDocumentDetails.documentID.ToString());
                    if (requestedCustomsDocId!= myDeclarationPM.RequestedCustomsDocId)
                    myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;


                    myDeclarationPM.RequestedCustomsDocId = requestedCustomsDocId;

                }
            }


            DeclarationUpdateService declarationUpdateService1 = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            declarationUpdateService1.Update(myDeclarationPM, true);

            if (customsDocumentPointerPM != null)
            {
                if (docParentEntityCode == "Declaration")
                {
                    myInsertEventContextTagModel.StatusObjectTable = requestEntity;
                    myInsertEventContextTagModel.StatusEntityId = requestId;
                    myInsertEventContextTagModel.StatusCustomFileNo = myDeclarationPM.CustomFileNo;
                    myInsertEventContextTagModel.FUStatusRemarks = "הערות מכס-" + workerRemarks;
                }

                customsDocumentPointerPM.CustomsRequestsSheetId = requestParams.CustomsRequestsSheetId;
                customsDocumentPointerPM.CurrentContextTag = myInsertEventContextTagModel;
                customsDocumentPointerPM.CustomsDocId = customResponse.RequiredDocumentDetails.documentID.ToString();
                
                myCustomsDocumentPointerUpdateService.Update(customsDocumentPointerPM, true);
            }

            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName(requestEntity);
            this.MyRequestSheetParam.EntityId1 = requestId;
            this.MyResponseData.ApplicationID = requestId;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = this.MyRequestSheetParam.RequestDescription;
            this.MyResponseData.Title = this.MyRequestSheetParam.RequestDescription;
        }

        private void InitMyResponseData(VAL_NG_8227_MSG_520_RequiredDocumentMessage customResponse, RequiredDocumentRequestParams requestParams)
        {
            this.MyResponseData = new RequiredDocumentResponseData();
            this.MyResponseData.Succeeded = true;

            this.MyResponseData.Title = "מסר מסמך נדרש " + customResponse.RequiredDocumentDetails.documentID.ToString(); ;
            this.MyResponseData.DocumentNumber = customResponse.RequiredDocumentDetails.documentID.ToString();
            this.MyResponseData.DocumentTypeVisibility = "Visible";
            if (!string.IsNullOrWhiteSpace(customResponse.RequiredDocumentDetails.typeID.ToString()))
            {
                var documentTypeName = GetDocumentTypeName(customResponse.RequiredDocumentDetails.typeID.ToString(), requestParams.Tenant);
                this.MyResponseData.DocumentType = customResponse.RequiredDocumentDetails.typeID.ToString();
                this.MyResponseData.DocumentTypeName = string.Concat(customResponse.RequiredDocumentDetails.typeID.ToString(), " - ", documentTypeName);
            }
            if (customResponse.Worker != null)
            {
                this.MyResponseData.DocumentWorkerName = customResponse.Worker.workerName;
            }
            this.MyResponseData.Remarks = customResponse.RequiredDocumentDetails.remarks;
            this.MyResponseData.ReplacingDocumentIdVisibility = "Collapsed";
            this.MyResponseData.DocumentConnectedEntitiesList = new List<DocumentConnectedEntitiesResult>();
            if (customResponse.RelatedEntity != null)
            {
                foreach (var relatedEntityItem in customResponse.RelatedEntity)
                {
                    DocumentConnectedEntitiesResult connectedEntity = new DocumentConnectedEntitiesResult();
                    connectedEntity.EntityType = relatedEntityItem.entityType.ToString();
                    connectedEntity.EntityTypeName = GetEntityTypeName(relatedEntityItem.entityType.ToString(), requestParams.Tenant);
                    connectedEntity.EntityNumber = relatedEntityItem.entityIdKey1;
                    this.MyResponseData.DocumentConnectedEntitiesList.Add(connectedEntity);
                }
            }
        }

        private string GetDocumentTypeName(string documentTypeCode, int tenant)
        {
            string documentTypeName = "";
            if (!string.IsNullOrWhiteSpace(documentTypeCode))
            {
                CustomDocumentTypeQueryService customDocumentTypeQueryService = new CustomDocumentTypeQueryService(tenant);
                CustomDocumentTypePM customDocumentTypePM = customDocumentTypeQueryService.GetSingle(documentTypeCode, false, true);
                if (customDocumentTypePM != null)
                {
                    documentTypeName = customDocumentTypePM.LocalName;
                }
            }
            return documentTypeName;
        }

        private string GetEntityTypeName(string entityTypeCode, int tenant)
        {
            string entityTypeName = "";
            if (!string.IsNullOrWhiteSpace(entityTypeCode))
            {
                EntityTypeLookupQueryService entityTypeLookupQueryService = new EntityTypeLookupQueryService(tenant);
                EntityTypeLookupPM entityTypeLookupPM = entityTypeLookupQueryService.GetSingle(entityTypeCode, false, true);
                if (entityTypeLookupPM != null)
                {
                    entityTypeName = entityTypeLookupPM.LocalName;
                }
            }
            return entityTypeName;
        }

        public override RequiredDocumentResponseData GetResponse(VAL_NG_8227_MSG_520_RequiredDocumentMessage customResponse, RequiredDocumentRequestParams requestParams)
        {
            return this.MyResponseData;
        }

    }
}
