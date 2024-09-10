using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.HybridMapping;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.TraceEvents;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsDocumentsTicketUpdateService
    {
        private string connectedDeclarationId;
        private bool hasNoDeclaration;
        protected override void OnCreating(CustomsDocumentsTicketPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomsDocumentsTicket", entityPM.Tenant);

        }

        protected override void UpdateComposition(CustomsDocumentsTicketPM entityPM)
        {
            CustomsDocumentPointerUpdateService customsDocumentPointerUpdateService = new CustomsDocumentPointerUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            customsDocumentPointerUpdateService.UpdateMulti(entityPM.CustomsDocumentPointers, entityPM.DeletedCustomsDocumentPointers, entityPM, false);
        }

        protected override void OnUpdating(CustomsDocumentsTicketPM entityPM)
        {
            bool updateCustomsDoc = false;
            if (entityPM.DocumentsFilingId == null && EntityPOCO.DocumentsFilingId != null) //disconnected.
            {
                ICustomContext context = MainContext as CustomContext;
                CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(context);
                CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQueryService = new CustomsDocumentMetaDataValueQueryService(context);
                CustomsDocumentPM customDocument = customsDocumentQueryService.GetSingle(EntityPOCO.DocumentsFilingId, false, false);
                CustomsDocumentUpdateService customsdocumentUpdateService = new CustomsDocumentUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                List<CustomsDocumentMetaDataValuePM> metaDataValues = customsDocumentMetaDataValueQueryService.GetCustomDocumentMetaDataValues(EntityPOCO.DocumentsFilingId, entityPM.Tenant);
                if (customDocument.IsPartOfDeclaration)
                {
                    updateCustomsDoc = true;
                    customDocument.ChangeSetOp = ChangeSetOperation.Update;
                    customDocument.IsPartOfDeclaration = false;

                    this._CreateHybridTask = true; ;//Bug 36694: Disconnecting document from the ticket does not create trigger to UNF
                }
                if (metaDataValues.Count == 0)
                {
                    if (customDocument != null)
                    {
                        updateCustomsDoc = true;
                        customDocument.ChangeSetOp = ChangeSetOperation.Update;
                        customDocument.DocumentTypeCode = null;
                    }
                }
                if (updateCustomsDoc)
                {
                    customsdocumentUpdateService.Update(customDocument, true);
                }
            }
            else if (entityPM.DocumentsFilingId != null && EntityPOCO.DocumentsFilingId == null) //connect
            {
                ICustomContext context = MainContext as CustomContext;
                CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(context);
                CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQueryService = new CustomsDocumentMetaDataValueQueryService(context);
                CustomsDocumentPM customDocument = customsDocumentQueryService.GetSingle(entityPM.DocumentsFilingId, false, false);
                if (customDocument == null)
                {
                    var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                    CustomsDocumentPM customsDocumentPM = new CustomsDocumentPM();
                    customsDocumentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    customsDocumentPM.DocumentsFilingId = entityPM.DocumentsFilingId;
                    customsDocumentPM.DocumentTypeCode = entityPM.DocumentTypeCode;
                    customsDocumentPM.CurrentCustomsDocumentsTicketId = entityPM.Id;
                    customsDocumentPM.Tenant = entityPM.Tenant;
                    foreach (CustomsDocumentMetaDataValuePM value in customsDocumentPM.CustomsDocumentMetaDataValues)
                    {
                        value.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    myCustomsDocumentUpdateService.Update(customsDocumentPM, true);
                }
                if (customDocument != null)
                {
                    if (customDocument.DocumentTypeCode == null)
                        customDocument.DocumentTypeCode = entityPM.DocumentTypeCode;

                    CustomsDocumentUpdateService customsdocumentUpdateService = new CustomsDocumentUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                    customDocument.ChangeSetOp = ChangeSetOperation.Update;
                    this.UpdateIsPartOfDeclaration(customDocument, entityPM);
                    this._CreateHybridTask = true;//Bug 36694: Disconnecting document 
                    customsdocumentUpdateService.Update(customDocument, true);

                }
            }
            UpdateNotification(entityPM);
            SetDeclarationAsChanged(entityPM);

        }
        bool _CreateHybridTask = false;
        protected override void OnUpdating(CustomsDocumentsTicketPM entityPM, CustomsDocumentsTicket entityPOCO)
        {
            try
            {
                if (this._CreateHybridTask)
                {
                    string documentsFilingId = entityPM.DocumentsFilingId ?? EntityPOCO.DocumentsFilingId;
                    if (!string.IsNullOrWhiteSpace(documentsFilingId))
                    {
                        ///AddHybridTaskDocumentFilingChange(entityPM, documentsFilingId); //Bug 36694: Disconnecting document from the ticket  does not create trigger to UNF
                    }
                }
                base.OnUpdating(entityPM, entityPOCO);
            }
            finally
            {
                var myLogChangesService = new LogChangesService();
                myLogChangesService.
                    LogIt<CustomsDocumentsTicketPM, CustomsDocumentsTicket>("20180826HD315750.LogUntilDateyyyyMMdd", entityPM, entityPOCO);
            }
        }
        //private void AddHybridTaskDocumentFilingChange(CustomsDocumentsTicketPM customsDocumentsTicketPM, string documentsFilingId)//Bug 36694: Disconnecting document from the ticket  does not create trigger to UNF
        //{
        //    var tenant = customsDocumentsTicketPM.Tenant;
        //    var loggedUserId = AuthenticationUtil.ResolveUserId(tenant);
        //    var table = ObjectTableQuery.GetObjectTableByCode("DocumentsFiling", 0);
        //    var documentsFilingQuery = new DocumentsFilingQuery(tenant);
        //   var pm= documentsFilingQuery.GetSinglePM(documentsFilingId, customsDocumentsTicketPM.Tenant);
        //    if (pm == null) return;

        //    CommunicationsParams logParams = new CommunicationsParams()
        //    {
        //        Tenant = tenant,
        //        CommunicationLogTypeCode = "Q",
        //        QueueName = "externaltasksqueue" + tenant + 1,
        //        Priority = 1,
        //        InOut = "O",
        //        Status = "W",
        //        LoggingUserId = loggedUserId,
        //        LoggingObjectTableId = table.Id,
        //        LoggingEntityId = customsDocumentsTicketPM.Id,
        //        Subject = "New Documents Filing Created",
        //        FolderName = "ExternalTasksQueue",
        //    };
        //    DocumentsFilingPM mappedPM = DocumentsFilingHybridMapping.MapEntityToHybrid(pm);

        //    string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(mappedPM);
        //    List<QueueTask> queue1Tasks = new List<QueueTask>();


        //    queue1Tasks.Add(new QueueTask()
        //    {
        //        Action = "DocumentsFiling.Upsert",
        //        Parameters = new List<Parameter>()
        //                                     {
        //                                        new Parameter{ Name = "DocumentMetaData", Order = 1, Value = xmlstring }
        //                                     }
        //    });

        //    logParams.ByteData = LogitudeXmlSerializer.SerializeObject(queue1Tasks);
        //    Communications.AddCommunicationLog(logParams);
        //}

        private void ApplySetDeclarationAsChanged(string declarationId, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            //-------------------- mohammad task 33591 prevent concurrency error for client------------------------//
            //DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);
            //DeclarationPM declaration = declarationQueryService.GetSingle(declarationId, false, false);
            //if (declaration != null)
            //{
            //    if (!declaration.IsChanged)
            //    {
            //        declaration.MarkAsChanged = true;
            //        declaration.IsChanged = true;
            //        declaration.ChangeSetOp = ChangeSetOperation.Update;
            //        DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);
            //        declarationUpdateService.Update(declaration, true);
            //    }
            //}


            DeclarationRepository declarationRep = new DeclarationRepository(context);
            Declaration declaration = declarationRep.GetSingle(declarationId, tenant);
            if (declaration != null)
            {
                if (!declaration.IsChanged)
                {
                    //declaration.IsChanged = true;
                    //declarationRep.Update(declaration);
                    //declarationRep.SubmitChanges();
                    declarationRep.SetIsChangedAndSubmitChanges(declaration);
                }
            }

            //------------------------------------------------------------------------------------------------------------//
        }

        private void SetDeclarationAsChanged(CustomsDocumentsTicketPM entityPM)
        {
            if (!this.hasNoDeclaration && string.IsNullOrEmpty(this.connectedDeclarationId))
            {
                ICustomContext context = MainContext as CustomContext;
                List<CustomsDocumentPointerPM> pointers = entityPM.CustomsDocumentPointers;
                var declarationPtr = pointers.Where(d => d.ParentEntityCode == "Declaration").FirstOrDefault();
                if (entityPM.CustomsDocumentPointers.Count == 0 || declarationPtr == null)
                {
                    CustomsDocumentPointerQueryService CustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
                    pointers = CustomsDocumentPointerQueryService.GetPointersForTicket(entityPM.Id, entityPM.Tenant);
                    declarationPtr = pointers.Where(d => d.ParentEntityCode == "Declaration").FirstOrDefault();
                }
                if (declarationPtr != null)
                {
                    ApplySetDeclarationAsChanged(declarationPtr.ParentEntityId, entityPM.Tenant);
                }
            }
            else if (!string.IsNullOrEmpty(connectedDeclarationId))
            {
                ApplySetDeclarationAsChanged(connectedDeclarationId, entityPM.Tenant);
            }
        }

        private void UpdateNotification(CustomsDocumentsTicketPM entityPM)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(Tenant);

            string notificationDefinitionCode = "";
            var eventContextTagModel = entityPM.CurrentContextTag as EventContextTagModel;
            string unifrieghtStatus = "";
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.VAL_NG_8228_RequiredDocumentVerificationDecisionVerified:
                    case EventContextTagModel.ProccessEnum.VAL_NG_8228_RequiredDocumentVerificationDecisionVerifiedWithClient:
                        notificationDefinitionCode = "8228A";
                        break;
                    case EventContextTagModel.ProccessEnum.VAL_NG_8228_RequiredDocumentVerificationDecisionReject:
                        notificationDefinitionCode = "8228D";
                        break;
                }



            }

            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(entityPM);
                DoUpdateNotification(entityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode, eventContextTagModel.FUStatusRemarks);
                if(connectedDeclarationPM!= null)
                {
                    if(eventContextTagModel.CallProccessID== EventContextTagModel.ProccessEnum.VAL_NG_8228_RequiredDocumentVerificationDecisionVerifiedWithClient)
                    {
                        eventContextTagModel.EventCode = "RDC";
                    }
                    eventContextTagModel.StatusCustomFileNo = connectedDeclarationPM.CustomFileNo;
                   
                    
                    SendEvent(eventContextTagModel.EventCode, eventContextTagModel, loggingUserId, connectedDeclarationPM.Id, entityPM.VerificationRemarks, entityPM.RequestedCustomsDocId);
                    
                }
            }
        }


        private void SendEvent(string statusId , EventContextTagModel eventContextTagModel , string loggingUserId, string declarationId,string  remarks, string requestedCustomsDocId)
        {
            try
            {

                
                Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel myAmitalEventTracerModel;
                
                    myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                    {
                        Tenant =  Tenant,
                        objectTableName = "Customs.Declaration",
                        EventCode = statusId,
                        notes = "DocumentId :" + requestedCustomsDocId + '\n' + remarks,
                        CommunicationLoggingEntityReference = declarationId,
                        EntityId = declarationId,
                        UserId = loggingUserId,
                        CommunicationSubject = "FU Status from logitude ",
                        MyFUStatus = new AmitalEventTracerModel.FUStatus()
                        {
                            entname = "CFIFILEM",
                            primary_number = eventContextTagModel.StatusCustomFileNo,
                            status = "new",
                            xml_status = "new",
                            status_id = statusId,
                            status_DateTime = DateTime.Now,
                            //status_place = "FRA",
                            //status_save = "no_fail",
                            comments = eventContextTagModel.FUStatusRemarks,
                        }

                    };
                 
               
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }


        private DeclarationPM GetConnectedDeclarationPM(CustomsDocumentsTicketPM dirtyEntityPM)
        {
            DeclarationPM myDBEntity = null;
            var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(dirtyEntityPM.Tenant);
            if (dirtyEntityPM.CustomsDocumentPointers != null && dirtyEntityPM.CustomsDocumentPointers.Count() > 0)
            {
                myDBEntity = declarationQueryService.GetSingle(dirtyEntityPM.CustomsDocumentPointers.FirstOrDefault().ParentEntityId, false, false);
            }
            return myDBEntity ?? new DeclarationPM();
        }

        private void DoUpdateNotification(CustomsDocumentsTicketPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode, string remarks)
        {
            ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
            this.currentContext = dbContext;
            string desc = "";
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = dirtyEntityPM.Tenant;

            switch (notificationDefinitionCode)
            {
                case "8228A":
                    newNotificationPM.NotificationDefinitionCode = "8228A";
                    //desc = remarks;
                    if (connectedDeclarationPM != null)
                    {
                        //desc = "תיק " + connectedDeclarationPM.CustomFileNo + " - " + desc;
                        desc = "תיק " + connectedDeclarationPM.CustomFileNo + " - " + " דרישת מסמך אומתה" + "\n";
                    }
                    if (!String.IsNullOrWhiteSpace(dirtyEntityPM.CustomsDocId)) desc += "סימוכין מכס - " + dirtyEntityPM.CustomsDocId + "\n";
                    desc += "הערות - " + remarks;

                    newNotificationPM.AssigneToNotificationTypeCode = "I";
                    break;
                case "8228D":
                    newNotificationPM.NotificationDefinitionCode = "8228D";
                    //desc = remarks;
                    if (connectedDeclarationPM != null) desc = "תיק " + connectedDeclarationPM.CustomFileNo + " - " + " דרישת מסמך נדחתה" + "\n";
                    if (!String.IsNullOrWhiteSpace(dirtyEntityPM.CustomsDocId)) desc += "סימוכין מכס - " + dirtyEntityPM.CustomsDocId + "\n";
                    desc += "הערות - " + remarks + "\n";

                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    break;
            }

            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.EntityId = dirtyEntityPM.DocumentsFilingId;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument");
            newNotificationPM.Reference2Number = dirtyEntityPM.CustomsDocId;

            string oldassigneId = null;
            string customerId = null;
            string referentUserId = null;

            if (connectedDeclarationPM != null)
            {
                newNotificationPM.EntityId = connectedDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                customerId = connectedDeclarationPM.CustomerId;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, oldassigneId);

            NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "8228");
            if (notificationDefinitionCode == "8228A" || notificationDefinitionCode == "8228D")
            {
                NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "8227N");
            }
            notificationUpdateService.Update(newNotificationPM, true);
        }

        protected override void AfterUpdating(CustomsDocumentsTicketPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.IsMetaDataReady)
            {
                //SendMessageToQueue(entityPM);
            }
            //if (entityPM.DocumentTypeCode == "380")
            //{
            UpdateDeclarationCourierStatus(entityPM);
            //}
        }

        public void UpdateIsPartOfDeclaration(CustomsDocumentPM customDoc, CustomsDocumentsTicketPM ticket)
        {
            ICustomContext context = MainContext as CustomContext;
            List<CustomsDocumentPointerPM> pointers = ticket.CustomsDocumentPointers;
            var declarationPtr = pointers.Where(d => d.ParentEntityCode == "Declaration").FirstOrDefault();
            if (ticket.CustomsDocumentPointers.Count == 0 || declarationPtr == null)
            {
                CustomsDocumentPointerQueryService CustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
                pointers = CustomsDocumentPointerQueryService.GetPointersForTicket(ticket.Id, ticket.Tenant);
                declarationPtr = pointers.Where(d => d.ParentEntityCode == "Declaration").FirstOrDefault();
            }

            if (declarationPtr != null)
            {
                this.hasNoDeclaration = false;
                this.connectedDeclarationId = declarationPtr.ParentEntityId;
                customDoc.IsPartOfDeclaration = true;
            }
            else
            {
                this.hasNoDeclaration = true;
                this.connectedDeclarationId = null;
            }

              
        }

        private void UpdateDeclarationCourierStatus(CustomsDocumentsTicketPM entityPM)
        {

            ICustomContext context = MainContext as CustomContext;

            DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(entityPM);

            string ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");//task10676 
            string status = null;
            bool inProgress = false;
            bool DocumentStatusCodeIsX = false;
            //  var customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(context);
            //var requests=  customsRequestsSheetQuery.GetRequestInProgress(Tenant, "2715", null,null, null, null, connectedDeclarationPM.CustomFileNo);


            if (connectedDeclarationPM != null && connectedDeclarationPM.IsCourierDeclaration)
            {

                CustomsDocumentsTicketQueryService customsDocumentsTicketQuery = new CustomsDocumentsTicketQueryService(entityPM.Tenant);
                List<CustomsDocumentsTicketPM> customsDocumentsTickets = customsDocumentsTicketQuery.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(connectedDeclarationPM.Id, null, null, null, connectedDeclarationPM.Tenant, "Declaration");

                var customsDocumentsTicketsInProgress = customsDocumentsTickets.Where(x => x.DocumentStatusCode == "7");

                if (customsDocumentsTicketsInProgress != null && customsDocumentsTicketsInProgress.Count() > 0)
                {
                    inProgress = true;
                    status = "I";
                }
                else
                {


                    CustomDocumentTypeQueryService docTypeQuery = new CustomDocumentTypeQueryService(context);
                    CustomDocumentTypePM docType = docTypeQuery.GetSingleCustomDocumentTypeWithTenant(entityPM.DocumentTypeCode, entityPM.Tenant);
                    if (docType != null && docType.IsCourierManadatory)
                    ////if (entityPM.DocumentTypeCode == "380")
                    {
                        //ICustomContext context = MainContext as CustomContext;

                        if (entityPM.CustomsDocumentPointers != null && entityPM.CustomsDocumentPointers.Count() > 0) LogMessagingUtil.Instance.AppendLine("ticket pointer connected entity: " + entityPM.CustomsDocumentPointers.FirstOrDefault().ParentEntityId);
                        CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(context);
                        LogMessagingUtil.Instance.AppendLine("found connected entity: " + connectedDeclarationPM.Id);

                        List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(connectedDeclarationPM.Id, "", "", "", entityPM.Tenant, "Declaration").Where(r => r.DocumentTypeCode == entityPM.DocumentTypeCode).ToList();

                        if (customsDocumentsTicketPMList == null || customsDocumentsTicketPMList.Count() < 1)
                        {
                            status = "M";
                        }
                        else
                        {
                            var DocumentsFilingIdList = new List<string>();
                            foreach (var customsDocumentsTicketPM in customsDocumentsTicketPMList)
                            {
                                if (!string.IsNullOrWhiteSpace(customsDocumentsTicketPM.DocumentsFilingId))
                                {
                                    DocumentsFilingIdList.Add(customsDocumentsTicketPM.DocumentsFilingId);
                                }
                            }
                            var customsDocumentPMList = new List<CustomsDocumentPM>();
                            if (DocumentsFilingIdList != null)
                            {
                                var myCustomsDocumentQueryService = new CustomsDocumentQueryService(context);
                                customsDocumentPMList = myCustomsDocumentQueryService.GetCustomsDocumentList(DocumentsFilingIdList, entityPM.Tenant);
                            }
                            if ((customsDocumentPMList == null || customsDocumentPMList.Count() < 1) && docType.IsCourierManadatory)
                            {
                                status = "M";
                            }
                            else
                            {
                                if (entityPM.ChangeSetOp != ChangeSetOperation.Delete)
                                {
                                    var myQueryService = new CustomsDocumentQueryService(context);
                                    var customsDocumentPM = myQueryService.GetSingle(entityPM.DocumentsFilingId, false, true);
                                    if (customsDocumentPM != null)
                                    {
                                        LogMessagingUtil.Instance.AppendLine("customsDocumentPM DocumentStatusCode: " + customsDocumentPM.DocumentStatusCode);
                                        if (customsDocumentPM.DocumentStatusCode != "2")
                                        {
                                            status = "V";
                                        }
                                        else
                                        {
                                            status = "X";

                                            DocumentStatusCodeIsX = true;
                                        }

                                    }
                                }

                            }
                        }
                    }

                    else
                    {
                        var myQueryService = new CustomsDocumentQueryService(context);
                        var customsDocumentPM = myQueryService.GetSingle(entityPM.DocumentsFilingId, false, true);
                        if (customsDocumentPM != null)
                        {
                            LogMessagingUtil.Instance.AppendLine("customsDocumentPM DocumentStatusCode: " + customsDocumentPM.DocumentStatusCode);
                            if (customsDocumentPM.DocumentStatusCode != "2")
                            {
                                status = "V";
                            }
                        }
                        }

                }
            }
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(connectedDeclarationPM.Id, true, false);

            if (!string.IsNullOrWhiteSpace(status))
            {
                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), connectedDeclarationPM.Tenant);
                if (currentDeclarationCourierStatusPM == null)
                {
                    currentDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                    {
                        DeclarationId = connectedDeclarationPM.Id,
                        Tenant = connectedDeclarationPM.Tenant,
                        IsClosedForFollowUp = false,
                        IsCourierMissingClassification = false,
                    };
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                currentDeclarationCourierStatusPM.DocumentStatusCode = status;
                declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                LogMessagingUtil.Instance.AppendLine("currentDeclarationCourierStatusPM.DocumentStatusCode: " + currentDeclarationCourierStatusPM.DocumentStatusCode);
            }

            if (currentDeclarationCourierStatusPM != null)
            {
                CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(connectedDeclarationPM, connectedDeclarationPM.Id, connectedDeclarationPM.Tenant);
                string prevValCourierDeclarationStatusCode = null;
                string currvValCourierDeclarationStatusCode = null;
                string prevValCourierDocumentStatusCode = null;
                string currvValCourierDocumentStatusCode = null;
                string prevValMissingDocumentStatusCode = null;
                string currvValMissingDocumentStatusCode = null;

                prevValCourierDeclarationStatusCode = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;
                prevValCourierDocumentStatusCode = currentDeclarationCourierStatusPM.DocumentStatusCode;
                prevValMissingDocumentStatusCode = currentDeclarationCourierStatusPM.MissedDocumentStatusCode;

                calculateDeclarationCourierStatus.CalcCourierDeclarationStatusCode(currentDeclarationCourierStatusPM);
                calculateDeclarationCourierStatus.CalcMissingDocumentStatusCode(currentDeclarationCourierStatusPM);

                if (!inProgress && !DocumentStatusCodeIsX)
                {
                    calculateDeclarationCourierStatus.CalcDocumentStatusCode(currentDeclarationCourierStatusPM);
                }
                currvValCourierDocumentStatusCode = currentDeclarationCourierStatusPM.DocumentStatusCode;
                currvValCourierDeclarationStatusCode = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;
                currvValMissingDocumentStatusCode = currentDeclarationCourierStatusPM.MissedDocumentStatusCode;

                if (prevValCourierDeclarationStatusCode != currvValCourierDeclarationStatusCode 
                    || prevValCourierDocumentStatusCode != currvValCourierDocumentStatusCode 
                    || prevValMissingDocumentStatusCode != currvValMissingDocumentStatusCode)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), connectedDeclarationPM.Tenant);
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                }
                LogMessagingUtil.Instance.AppendLine("currentDeclarationCourierStatusPM.CourierDeclarationStatusCode: " + currentDeclarationCourierStatusPM.CourierDeclarationStatusCode);
            }

        }

    }
}
