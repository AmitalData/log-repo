using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System.ServiceModel.DomainServices.Server;
using WebFreight.Web.DataContracts;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Data.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Logitude.Server.Tools.Counters;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.CRM.Data.BusinessUnitFilters;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.CRM.BL.WorkRoles;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public TicketPM GetSingleTicketPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ticketQuery = new TicketQueryService(crmContext);
            TicketPM entityPM = ticketQuery.GetSingle(id, true, false);
            entityPM.EntityNumber = entityPM.ShipmentNumber != null ? entityPM.ShipmentNumber : entityPM.QuoteNumber;

            if (!string.IsNullOrEmpty(entityPM.SLAId))
            {
                SLAHeaderRepository mySLAHeaderRepository = new SLAHeaderRepository(tenant);
                SLAHeader mySLAHeader = mySLAHeaderRepository.GetSingle(entityPM.SLAId, tenant);
                entityPM.SLAName = mySLAHeader.Name;
            }
            
            #region Attachment
            //Docs In 
            ObjectTableRepository tableRepository = new ObjectTableRepository(0);
            ObjectTable table = tableRepository.GetObjectTableByName("Ticket", 0, false);

            DocumentsFilingRepository rep = new DocumentsFilingRepository(tenant);
            //List<DocumentsFiling> docsIn = rep.GetDocumentsFilingsForEntityTableId(entityPM.Id, table.Id, tenant);

            DocumentsFilingQuery query = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingPM> docsIn = query.GetDocumentsFilingPMsByEntityId(tenant,entityPM.Id,table.Id);


            if (!string.IsNullOrEmpty(entityPM.QuoteId))
            {
                DocumentsFilingPM documentsFilingPM = GetQuotationLastVersionDocumentsFilingPM(entityPM);
                if (documentsFilingPM != null) docsIn.Add(documentsFilingPM);
            }

            CorrespondenceQueryService correspondenceQuery = new CorrespondenceQueryService(tenant);
            List<CorrespondencePM> correspondencesList = correspondenceQuery.GetAllCorrespondencesByEntityIdAndTenant(entityPM.Id, tenant);

            List<string> correspondencesListIds = correspondencesList.Select(a => a.Id).ToList();

            CorrespondencesAttachmentRepository attachmentrep = new CorrespondencesAttachmentRepository(tenant);
            List<CorrespondencesAttachment> attachmentsCorrespondencesList = attachmentrep.GetAllCorrespondencesAttachmentByListOfIds(correspondencesListIds, tenant);

            foreach (CorrespondencePM item in correspondencesList)
            {
                entityPM.TicketCorrespondence.Add(item);

                List<CorrespondencesAttachment> myCorrespondence = attachmentsCorrespondencesList.Where(a => a.CorrespondenceId == item.Id).ToList();

                foreach (CorrespondencesAttachment attach in myCorrespondence)
                {
                    DocumentsFilingPM doc = docsIn.Where(a => a.Id == attach.DocumentFilingId).FirstOrDefault();
                    if (doc != null)
                    {
                        DocumentDataPM datapm = new DocumentDataPM()
                        {
                            Id = doc.Id,
                            EntityId = doc.EntityId,
                            DocumentTypeId = doc.DocumentTypeId,
                            Code = doc.Code,
                            ReceivedDate = doc.CreateDate,
                            Tenant = doc.Tenant,
                            DocumentId = doc.DocumentId,
                            DocumentTypeName = doc.CustomsDocumentTypeName,
                            FileName = doc.FileName != null && doc.FileExtension != null ? doc.FileName + "." + doc.FileExtension : null,
                            SecurityId = doc.SecurityId,
                            CorrespondenceId = attach.CorrespondenceId,
                            FileExtension = doc.FileExtension != null ? doc.FileExtension : null,
                            FileSize = doc.FileSize,
                            CreateDate = doc.CreateDate,
                            UpdateDate = doc.UpdateDate,
                        };

                        entityPM.TicketDocumentData.Add(datapm);
                    }

                }
            }

            #endregion

            #region Time Color

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            entityPM.IsResolveDue = (entityPM.FirstResolveDate != null) ? false : true;

            entityPM.ResolveColor = (entityPM.FirstResolveDate != null) ? "Green" : (entityPM.ResolveWithinDue > todayDateTime ? "Orange" : "Red");
            entityPM.IsResolveExamination = (entityPM.FirstResolveDate != null && entityPM.ResolveWithinDue < entityPM.FirstResolveDate) ? true : false;

            entityPM.IsResponseDue = (entityPM.FirstResponseTime != null) ? false : (true);
            entityPM.ResponseColor = (entityPM.FirstResponseTime != null) ? "Green" : (entityPM.FirstResponseDue > todayDateTime ? "Orange" : "Red");
            entityPM.IsResponseExamination = (entityPM.FirstResponseTime != null && entityPM.FirstResponseDue < entityPM.FirstResponseTime) ? true : false;

            entityPM.TicketFirstResponseTime = (entityPM.FirstResponseTime != null) ? entityPM.FirstResponseTime : entityPM.FirstResponseDue;
            entityPM.TicketFirstResolveTime = (entityPM.FirstResolveDate != null) ? entityPM.FirstResolveDate : entityPM.ResolveWithinDue;
            #endregion

            

            return entityPM;
        }

        private static DocumentsFilingPM GetQuotationLastVersionDocumentsFilingPM(TicketPM entityPM)
        {
            string objectTableId = ObjectTableRepository.GetObjectTableByName("Quote");
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(entityPM.Tenant);
            var documentTypeId = documentTypeQuery.GetDocumentTypeIdByCode("QUOTE", entityPM.Tenant);
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(entityPM.Tenant);
            DocumentsFilingPM documentsFilingPM = documentsFilingQuery.GetDocumentsFilingPMByEntityIdAndObjectTableIdAndDocumentTypeId(entityPM.QuoteId, objectTableId, documentTypeId, entityPM.Tenant);
            return documentsFilingPM;
        }

        public TicketList GetSingleTicketList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketListQueryService listService = new TicketListQueryService(crmContext);
            TicketList myResult = listService.GetSingle(id);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Ticket", tenant, new List<TicketList> { myResult }.Cast<object>().ToList());

            return myResult;
        }

        public void UpdateTicketList(TicketList list)
        {

        }

        public List<TicketList> GetTicketLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketListQueryService listService = new TicketListQueryService(crmContext);
            List<TicketList> myResult = listService.GetList(tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Ticket", tenant, myResult.Cast<object>().ToList());

            return myResult;
        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public IQueryable<TicketList> GetTicketFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketListQueryService listService = new TicketListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            var rankFilter = queryOperations.QueryFilterItems.FirstOrDefault(f => f.FieldName == "RankId");
            if (rankFilter != null)
                rankFilter.DisplayInList = true;

            List<TicketList> myResult = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Ticket", tenant, myResult.Cast<object>().ToList());

            return myResult.AsQueryable();
        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<TicketList> GetTicketFiltersHybrid(byte[] xmlFilters, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketListQueryService listService = new TicketListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<TicketList> myResult = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Ticket", tenant, myResult.Cast<object>().ToList());

            return myResult;
        }

        public int GetTicketFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketListQueryService queryService = new TicketListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            var rankFilter = queryOperations.QueryFilterItems.FirstOrDefault(f => f.FieldName == "RankId");
            if (rankFilter != null)
                rankFilter.DisplayInList = true;

            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertTicket(TicketPM entityPM)
        {
            try
            {
                bool supportActivated = true;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenatManagement = tenantManagementRepository.GetSingleTenantManagement(entityPM.Tenant);
                    supportActivated = tenatManagement.SupportActivated;
                    scope.Complete();
                }

                if (supportActivated)
                {
                    this.InsertNewTicket(entityPM);

                }
                else
                {
                    throw new Exception("Tickets are not enabled, please contact your system administrator");
                }
            }

            catch (Exception e)
            {
                throw;
            }
        }

        private void InsertNewTicket(TicketPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Ticket", "NEW", entityPM.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.CreateDate = todayDate;
            entityPM.UpdateDate = todayDate;

            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                entityPM.CreatedByContactId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            TicketUpdateService service = new TicketUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Ticket", 0, true);
            ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", entityPM.UpdatedByUserId);

            bool isRightToLeft = false;
            TenantRepository repo = new TenantRepository(entityPM.Tenant);
            Tenant tenant = repo.GetSingleByTenant(entityPM.Tenant);
            if (tenant != null && tenant.IsCorrespondRightToLeftEnabled == true)
            {
                isRightToLeft = true;
            }

            CorrespondencePM myCorrespondencePM = new CorrespondencePM()
            {
                Tenant = entityPM.Tenant,
                EntityId = entityPM.Id,
                CreateDate = todayDate,
                CreatedByContactId = entityPM.CreatedByContactId,
                IsInternal = false,
                Description = entityPM.TicketDescription,
                ObjectTableId = objectTable.Id,
                IsFirst = true,
                ChangeSetOp = ChangeSetOperation.Insert,
                NotifyMe = false,
                NotifyOwner = true,
                Direction = "O",
                RightToLeft  = isRightToLeft,
            };

            CorrespondenceUpdateService myCorrespondenceUpdateService = new CorrespondenceUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            myCorrespondenceUpdateService.Update(myCorrespondencePM, true);

            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            string contactEmail = contactRep.GetEmailContactByIdAndTenant(entityPM.Tenant, entityPM.ContactId);

            InboundEmailPM myHeader = new InboundEmailPM()
            {
                Tenant = entityPM.Tenant,
                EntityId = entityPM.Id,
                ObjectTableId = objectTable.Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                CreatedByContactId = entityPM.CreatedByContactId,
            };

            string senderEmail = GetSenderEmail(entityPM.Tenant, entityPM.GuidId, entityPM.SupportMailboxId);

            InboundEmailLinePM line = new InboundEmailLinePM()
            {
                Tenant = entityPM.Tenant,
                Sender = senderEmail,
                Recepient = contactEmail,
                Subject = entityPM.Subject,
                Body = entityPM.TicketDescription,
                FullBody = entityPM.TicketDescription,
                CCs = entityPM.CCs,
                Bcc = entityPM.Bcc,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                Direction = "O",
                EntityLineId = myCorrespondencePM.Id,
            };

            myHeader.InboundEmailLines.Add(line);

            IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
            InboundEmailService inboundEmailService = new InboundEmailService(objectContext, myHeader.Tenant);
            inboundEmailService.Create(myHeader);
        }

        public void UpdateTicket(TicketPM entityPM)
        {
            ticketRepository = new TicketRepository(entityPM.Tenant);
            TicketKeys keys = new TicketKeys() { Id = entityPM.Id };
            Ticket entity_Poco = ticketRepository.GetSingle(keys);

            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Ticket", "UPDATE", entityPM.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdateDate = todayDate;

            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            TicketUpdateService service = new TicketUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Ticket", 0, true);
            ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", entityPM.UpdatedByUserId);

            //if (crmContext.Correspondences.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert).Any())
            //{
            //    IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
            //    InboundEmailService inboundEmailService = new InboundEmailService(objectContext, entityPM.Tenant);
            //    inboundEmailService.Update(entityPM);
            //}
        }

        [Invoke]
        public DateTime InsertTicketTraceEvent(TicketPM entityPM, DateTime? eventDate, string note, string eventTypeId, string objectTableId, string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            string entityId = entityPM.Id;
                        
            //EventTypeRepository eventTypeRep = new EventTypeRepository(tenant);
            //EventType eventType = eventTypeRep.GetSingleEventType(eventTypeId, tenant);
            //WebFreightDomainService webFreightService = new WebFreightDomainService();
            //Ticket Ticket = ticketRepository.GetSingle(TicketPM.Id, TicketPM.Tenant);
            
            TraceEvent newTraceEvent = new TraceEvent();
            newTraceEvent.Id = Guid.NewGuid().ToString();
            newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.Notes = note;
            newTraceEvent.ObjectTableId = objectTableId;
            newTraceEvent.Tenant = tenant;
            newTraceEvent.UserId = userId;
            newTraceEvent.EventTypeId = eventTypeId;
            newTraceEvent.EventDateTime = eventDate != null ? eventDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.EntityId = entityId;
            newTraceEvent.Deleted = false;
            newTraceEvent.IsAddedManually = true;

            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            traceEventRep.Add(newTraceEvent);
            traceEventRep.SubmitChanges();

            return newTraceEvent.LogDateTime;
        }

        [Invoke]
        public void DeleteTicketTraceEvent(TicketPM entityPM, string traceEventId, int tenant, bool external)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            traceEvent.Deleted = true;
            traceEventRep.Update(traceEvent);
            traceEventRep.SubmitChanges();
        }

        public List<TicketList> GetRecentTickets(string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            string mail = SecurityUtility.GetAuthenticatedUser();
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM contact = contactQuery.GetContactByEmailOnly(mail, tenant);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Ticket", 0, true);

            TicketListQueryService queryService = new TicketListQueryService(crmContext);
            IQueryable<TicketList> myResult = queryService.GetRecentEntityLists(ownerId, employeeGroupId , tenant, contact.Id, objectTable.Id).AsQueryable();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Ticket", tenant, myResult.Cast<object>().ToList());
            return myResult.ToList();
        }

        public List<ChartingDataClass> GetOpenTicketsGroupByClassification(string ownerId, string employeeGroupId, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetOpenTicketsGroupByClassification(ownerId, employeeGroupId, tenant, isTopTen);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage myStage = repository.GetTicketStageByCode("RE", tenant);

            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    OwnerId = item.OwnerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    ClassificationId = item.ClassificationId,
                    LabelProperty = item.LabelProperty,
                    TicketStageId = myStage.Id,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetOpenTicketsByDueTime(string ownerId, string employeeGroupId, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetOpenTicketsByDueTime(ownerId, employeeGroupId, tenant, isTopTen);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    OwnerId = item.OwnerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    LabelProperty = item.LabelProperty,
                    LabelColor = item.LabelColor,
                });
            }

            return myResult;
        }

        [Invoke]
        public string CheckOwnerEmployeeGroup(string ownerId, string groupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            EmployeeGroupLineQueryService myQuery = new EmployeeGroupLineQueryService(tenant);
           // List<EmployeeGroupLinePM> linesGroup = myQuery.GetEmployeeGroupLinesByGroupId(groupId, tenant);

            EmployeeGroupLinePM defaultOwner = myQuery.GetEmployeeGroupLinesByGroupId(groupId, tenant);
            string myUserId = "";
            if (defaultOwner != null)
            {
                myUserId = defaultOwner.UserId;
            }

            return myUserId;
        }

        public List<TicketList> GetTicketListByShipmentId(string shipmentId, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketListQueryService listService = new TicketListQueryService(crmContext);
            List<TicketList> myResult = listService.GetTicketListByShipmentIdList(shipmentId,tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Ticket", tenant, myResult.Cast<object>().ToList());

            return myResult;
        }

        #region Closed Tickets Charts

        public List<ChartingDataClass> GetClosedTicketsGroupByClassification(string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetClosedTicketsGroupByClassification(code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);

            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = item.OwnerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                    ClassificationId = item.ClassificationId,
                    LabelProperty = item.LabelProperty,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetClosedTicketsGroupBySeverity(string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetClosedTicketsGroupBySeverity(code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = item.OwnerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                    SeverityId = item.SeverityId,
                    LabelProperty = item.LabelProperty,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetClosedTicketsGroupByType(string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetClosedTicketsGroupByType(code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = item.OwnerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                    LabelProperty = item.LabelProperty,
                    TicketTypeId = item.TicketTypeId,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetClosedTicketsBySLAViolation(int selectedIndex, string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetClosedTicketsBySLAViolation(selectedIndex, code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    IntegerProperty = item.IntegerProperty,
                    LabelProperty = item.LabelProperty,
                    IndexOrder = item.IndexOrder,

                    Day = days,
                    OwnerId = ownerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                    DateTimeProperty = item.DateTimeProperty,
                    GroupByCode = item.GroupByCode,

                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetClosedTicketsBySolvedStage(int selectedIndex, string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetClosedTicketsBySolvedStage(selectedIndex, code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = ownerId,
                    EmployeeGroupId = employeeGroupId,
                    Code = code,
                    ShortLabelProperty = myShortLabelProperty,
                    LabelProperty = item.LabelProperty,
                    IndexOrder = item.IndexOrder,
                    GroupByCode = item.GroupByCode,
                    DateTimeProperty = item.DateTimeProperty,
                });
            }

            return myResult;
        }

        #endregion 

        #region Opened Tickets Chart
        public List<ChartingDataClass> GetOpenedTicketsGroupByClassification(string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetOpenedTicketsGroupByClassification(code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = item.OwnerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                    ClassificationId = item.ClassificationId,
                    LabelProperty = item.LabelProperty,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetOpenedTicketsGroupBySeverity(string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetOpenedTicketsGroupBySeverity(code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = item.OwnerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                    SeverityId = item.SeverityId,
                    LabelProperty = item.LabelProperty,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetOpenedTicketsGroupByOwner(string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetOpenedTicketsGroupByOwner(code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = item.OwnerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                    LabelProperty = item.LabelProperty,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetOpenedTicketsBySLAViolation(int selectedIndex, string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetOpenedTicketsBySLAViolation(selectedIndex, code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    IntegerProperty = item.IntegerProperty,
                    LabelProperty = item.LabelProperty,
                    IndexOrder = item.IndexOrder,

                    Day = days,
                    OwnerId = ownerId,
                    EmployeeGroupId = employeeGroupId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                    DateTimeProperty = item.DateTimeProperty,
                    GroupByCode = item.GroupByCode,

                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetOpenedTicketsByOpenedStage(int selectedIndex, string code, string ownerId, string employeeGroupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetOpenedTicketsByOpenedStage(selectedIndex, code, ownerId, employeeGroupId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            //int days = Convert.ToInt32(str);
            int days;
            Int32.TryParse(str, out days);
            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = ownerId,
                    EmployeeGroupId = employeeGroupId,
                    Code = code,
                    ShortLabelProperty = myShortLabelProperty,
                    LabelProperty = item.LabelProperty,
                    IndexOrder = item.IndexOrder,
                    GroupByCode = item.GroupByCode,
                    DateTimeProperty = item.DateTimeProperty,
                });
            }

            return myResult;
        }

        #endregion 

        #region Overview Tab Performance
        public List<ChartingDataClass> GetTicketOverviewPerformance(string ticketId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            ticketQuery = new TicketQueryService(crmContext);

            List<CRMChartingClass> data = ticketQuery.GetTicketOverviewPerformance(ticketId, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    IntegerProperty = item.IntegerProperty,
                    DoubleProperty = item.DoubleProperty,
                    ShortLabelProperty = myShortLabelProperty,
                    LabelProperty = item.LabelProperty,
                    IndexOrder = item.IndexOrder,
                    GroupByCode = item.GroupByCode,
                    StringProperty = item.StringProperty,
                    DataTypeCode = item.DataTypeCode,
                    TimeProperty = item.TimeProperty,
                });
            }

            return myResult;
        }

        #endregion 

        public CRMSummary GetTicketOverViewStatisticsSummary(string ticketId, int tenant)
        {
            CRMSummary summaryClass = new CRMSummary() { Id = tenant };

            CorrespondenceRepository correspondenceRepository = new CorrespondenceRepository(tenant);
            List<Correspondence> dataSource = correspondenceRepository.GetCorrespondenceByEntityId(ticketId, tenant);

            summaryClass.Tickets_ExternalLines = dataSource.Where(a => !a.IsInternal && a.ActivityId == null).Count();
            summaryClass.Tickets_InternalLines = dataSource.Where(a => a.IsInternal == true).Count();

            ActivityRepository activityRepository = new ActivityRepository(tenant);
            summaryClass.Tickets_Activities = activityRepository.GetActivitiesByTicketId(ticketId, tenant).Count();

            TicketEscalationRepository escalationRepository = new TicketEscalationRepository(tenant);
            summaryClass.Tickets_Escalations = escalationRepository.GetTicketEscalations(ticketId, tenant).Count();

            return summaryClass;
        }

        public TicketLogsSummary CalculatingBusinessHours(string id, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ticketQuery = new TicketQueryService(crmContext);
            TicketPM entityPM = ticketQuery.GetSingle(id, true, false);
            TicketLogsSummary summaryClass = new TicketLogsSummary() { Id = tenant };
            SLALineRepository slaRep = new SLALineRepository(tenant);
            BusinessHourRepository businessHourRep = new BusinessHourRepository(tenant);
            SLALine slaLine = slaRep.GetSLALineBySeverityId(tenant, entityPM.SeverityId , entityPM.SLAId);
            BusinessHour businessHour = new BusinessHour();
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            TicketStageRepository stageRep = new TicketStageRepository(tenant);
            TicketStage stage = stageRep.GetTicketStageByCode("OP", tenant);


            if (slaLine != null)
            {
                businessHour = businessHourRep.GetSingleBusinessHours(slaLine.BusinessHoursId, slaLine.Tenant);
                BusinessHourCalcualtions businessCalculation = new BusinessHourCalcualtions(businessHour);

                if (entityPM.FirstResolveDate == null && entityPM.ResolveWithinDue != null && entityPM.ResolveWithinDue < todayDate)
                {
                    summaryClass.ResolveSLAViolated = TimeSpan.FromMinutes(businessCalculation.CalculateBusinessHours(entityPM.ResolveWithinDue.Value, todayDate, businessHour.Is247));
                    summaryClass.ResolveSLAViolatedTotalMinutes = summaryClass.ResolveSLAViolated.TotalMinutes;
                }

                if (entityPM.FirstResolveDate != null && (entityPM.ResolveWithinDue != null && entityPM.ResolveWithinDue < entityPM.FirstResolveDate))
                {
                    summaryClass.ResolveSLAViolated = TimeSpan.FromMinutes(businessCalculation.CalculateBusinessHours(entityPM.ResolveWithinDue.Value, entityPM.FirstResolveDate.Value, businessHour.Is247));
                    summaryClass.ResolveSLAViolatedTotalMinutes = summaryClass.ResolveSLAViolated.TotalMinutes;
                }

                if (entityPM.FirstResponseTime == null && entityPM.FirstResponseDue != null && entityPM.FirstResponseDue < todayDate)
                {
                    summaryClass.ResponseSLAViolated = TimeSpan.FromMinutes(businessCalculation.CalculateBusinessHours(entityPM.FirstResponseDue.Value, todayDate, businessHour.Is247));
                    summaryClass.ResponseSLAViolatedTotalMinutes = summaryClass.ResponseSLAViolated.TotalMinutes;
                }

                if (entityPM.FirstResponseTime != null && (entityPM.FirstResponseDue != null && entityPM.FirstResponseDue < entityPM.FirstResponseTime))
                {
                    summaryClass.ResponseSLAViolated = TimeSpan.FromMinutes(businessCalculation.CalculateBusinessHours(entityPM.FirstResponseDue.Value, entityPM.FirstResponseTime.Value, businessHour.Is247));
                    summaryClass.ResponseSLAViolatedTotalMinutes = summaryClass.ResponseSLAViolated.TotalMinutes;
                }

                if (stage.Id != entityPM.StageId)
                {
                    
                    summaryClass.OpenPeriodMinutes = entityPM.OpenPeriodMinutes != null ?  entityPM.OpenPeriodMinutes.Value : 0;
                }
                else
                {
                    int d = businessCalculation.CalculateBusinessHours(entityPM.OpenDate, todayDate, businessHour.Is247);
                    if (entityPM.OpenPeriodMinutes == null)
                    {
                        summaryClass.OpenPeriodMinutes = d;
                    }
                    else
                    {
                        summaryClass.OpenPeriodMinutes = entityPM.OpenPeriodMinutes.Value + d;
                    }
                }
            }

            return summaryClass;
        }

        public List<CommunicationLogPM> GetCommunicationLogs(string entityId, int tenant)
        {
            CommunicationLogQuery query = new CommunicationLogQuery(tenant);
            List<CommunicationLogPM> list = query.GetCommunicationLogPMsByEntityIdForTicket(entityId, tenant).ToList();

            return list;
        }
    }
}