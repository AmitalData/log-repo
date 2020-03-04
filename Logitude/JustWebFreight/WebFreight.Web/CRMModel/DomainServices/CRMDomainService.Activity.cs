using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Social.BL.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public ActivityPM GetSingleActivityPM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            activityQuery = new ActivityQueryService(crmContext);
            ActivityPM entityPM = activityQuery.GetSingle(id, true, false);

            if (entityPM.ActivityTypeCode == "EI")
            {
                ObjectTableRepository tableRepository = new ObjectTableRepository(0);
                ObjectTable table  = tableRepository.GetObjectTableByName("Activity", 0, false);
                string entityId = entityPM.Id;
                
                //if (!string.IsNullOrEmpty(entityPM.OpportunityId))
                //{
                //    table = tableRepository.GetObjectTableByName("Opportunity", 0, false);
                //    entityId = entityPM.OpportunityId;
                //}

                //else if (!string.IsNullOrEmpty(entityPM.CustomerId))
                //{
                //    table = tableRepository.GetObjectTableByName("Customer", 0, false);
                //    entityId = entityPM.CustomerId;
                //}

                //else
                //{
                //    table = tableRepository.GetObjectTableByName("Activity", 0, false);
                //}


                DocumentsFilingRepository rep = new DocumentsFilingRepository(tenant);
                //List<DocumentIn> docsIn = rep.GetDocumentInsByEntityId(entityPM.Id, tenant);
                List<DocumentsFiling> docsIn = rep.GetDocumentsFilingsForEntityTableId(entityId, table.Id, tenant);
                foreach (DocumentsFiling doc in docsIn)
                {
                    if (doc.Document != null && doc.Document.HasFile)
                    {
                        DocumentDataPM datapm = new DocumentDataPM()
                        {
                            EntityId = doc.EntityId,
                            ChildEntityId = doc.ChildEntityId,
                            ChildEntityReference = doc.ChildEntityReference,
                            DocumentTypeId = doc.DocumentTypeId,
                            Code = doc.Code,
                            Notes = doc.Notes,
                            ReceivedDate = doc.CreateDate,
                            Tenant = doc.Tenant,
                            DocumentId = doc.DocumentId,
                            DocumentTypeName = doc.DocumentType.Name,
                            FileName = doc.Document != null ? doc.Document.FileName + "." + doc.Document.Extension : null,
                        };

                        entityPM.ActivityDocumentDatas.Add(datapm);
                    }
                }

            }

            return entityPM;
        }

        public ActivityList GetSingleActivityList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            ActivityList list = listService.GetSingle(id);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Activity", tenant, new List<ActivityList> { list }.Cast<object>().ToList());

            return list;
        }

        public List<ActivityList> GetActivityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public void UpdateActivityList(ActivityList list)
        {

        }

        public List<ActivityList> GetActivitiesByOpportunityId(string opportunityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            return listService.GetActivitiesByOpportunityId(opportunityId, tenant);
        }

        public List<ActivityList> GetActivitiesByQuoteId(string quoteId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            return listService.GetActivitiesByQuoteId(quoteId, tenant);
        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<ActivityList> GetActivityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<ActivityList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Activity", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetActivityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ActivityListQueryService queryService = new ActivityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public List<ActivityList> GetRecentActivities(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            string mail = SecurityUtility.GetAuthenticatedUser();
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM contact = contactQuery.GetContactByEmailOnly(mail, tenant);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Activity", 0, true);

            ActivityListQueryService queryService = new ActivityListQueryService(crmContext);
            IQueryable<ActivityList> first = queryService.GetRecentEntityLists(tenant, contact.Id, objectTable.Id).AsQueryable();
            IQueryable<ActivityList> list = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);
            return list.ToList();
        }

        public List<ActivityList> GetUpcomigActivities(string ownerId, string businessUnitId, int tenant, string activityTypeCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ActivityListQueryService queryService = new ActivityListQueryService(crmContext);
            IQueryable<ActivityList> first = queryService.GetUpcomigEntityLists(ownerId, businessUnitId, tenant, activityTypeCode, "S").AsQueryable();
            IQueryable<ActivityList> list = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);
            return list.ToList();
        }

        [Invoke]
        public void CompleteActivity(string activityId, bool post, string summary, int tenant)
        {
            SecurityUtility.CheckContactFeature("Activity", "UPDATE", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            activityRepository = new ActivityRepository(crmContext);
            activityQuery = new ActivityQueryService(crmContext);
           
            ActivityPM entityPM = activityQuery.GetSingle(activityId, true, false);

            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);

            //ActivityUpdateService activityUpdateService = new ActivityUpdateService(crmContext);
            
            if (entityPM != null)
            {
                CloseActivityAndSaveChanges(summary, entityPM, loggedContact);

                Activity entity = activityRepository.GetSingle(activityId, tenant);
                //entity.IsOpen = false;
                //entity.ActivityStatusCode = "C";
                //entity.MeetingSummary = summary;
                //entity.NeedSynchronization = true;
                //entity.CompleteDate = todayDateTime;
                //entity.UpdateDate = todayDateTime;
                //this.InitializeSortingFields(entity);
                //if (loggedContact != null)
                //{
                //    entity.UpdatedByUserId = loggedContact.Id;
                //}
                //activityRepository.Update(entity);
                //activityRepository.SubmitChanges();
                //this.CreateEntityEvent(entity.UpdatedByUserId, entity, "CM");


                if (entity.ActivityTypeCode == "AP" && post)
                {
                    string message = "Meeting completed:" + Environment.NewLine + entity.MeetingSummary;
                    AutomaticPosting.CreatePost(entity.Id, "Activity", loggedContact.Id, entity.Subject, message, true, entity.Tenant);
                }

                if (!string.IsNullOrEmpty(entity.OpportunityId))
                {
                    #region
                    OpportunityRepository opportunityRepository = new OpportunityRepository(crmContext);
                    Opportunity opportunity = opportunityRepository.GetSingle(entity.OpportunityId, tenant);

                    if (opportunity != null)
                    {
                        opportunity.UpdateDate = entity.UpdateDate;
                        opportunity.UpdatedByUserId = entity.UpdatedByUserId;
                        opportunity.LastActivitySubject = entity.Subject;
                        opportunity.LastCompletedActivityTypeCode = entity.ActivityTypeCode;
                        opportunity.LastCompletedActivityDate = entity.CompleteDate;
                        opportunityRepository.Update(opportunity);
                        opportunityRepository.SubmitChanges();

                        //Next activity
                        IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByOpportunityId(entity.OpportunityId, tenant);
                        iQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);
                        Activity nextDBActivity = iQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();

                        string nextSubject = null;
                        string nextTypeCode = null;
                        DateTime? nextDateTime = null;
                        if (nextDBActivity != null)
                        {
                            nextSubject = nextDBActivity.Subject;
                            nextTypeCode = nextDBActivity.ActivityTypeCode;
                            nextDateTime = nextDBActivity.StartDateTime;
                        }

                        opportunity.NextActivityTypeCode = nextTypeCode;
                        opportunity.NextActivityDate = nextDateTime;
                        opportunity.NextActivitySubject = nextSubject;
                        opportunityRepository.Update(opportunity);
                        opportunityRepository.SubmitChanges();
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(entity.QuoteId))
                {
                    #region
                    QuoteRepository quoteRepository = new QuoteRepository(tenant);
                    Quote quote = quoteRepository.GetSingleQuote(entity.QuoteId, tenant);

                    if (quote != null)
                    {
                        //quote.UpdateDate = entity.UpdateDate;
                        //quote.UpdatedByUserId = entity.UpdatedByUserId;
                        quote.LastActivitySubject = entity.Subject;
                        quote.LastActivityTypeCode = entity.ActivityTypeCode;
                        quote.LastActivityDate = entity.CompleteDate;
                        quoteRepository.Update(quote);
                        quoteRepository.SubmitChanges();

                        //Next activity
                        IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByQuoteId(entity.QuoteId, tenant);
                        iQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);
                        Activity nextDBActivity = iQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();

                        string nextSubject = null;
                        string nextTypeCode = null;
                        DateTime? nextDateTime = null;
                        if (nextDBActivity != null)
                        {
                            nextSubject = nextDBActivity.Subject;
                            nextTypeCode = nextDBActivity.ActivityTypeCode;
                            nextDateTime = nextDBActivity.StartDateTime;
                        }

                        quote.NextActivityTypeCode = nextTypeCode;
                        quote.NextActivityDate = nextDateTime;
                        quote.NextActivitySubject = nextSubject;
                        quoteRepository.Update(quote);
                        quoteRepository.SubmitChanges();
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(entity.CustomerId))
                {
                    #region
                    CardRepository cardRepository = new CardRepository(tenant);
                    Card card = cardRepository.GetSingleCard(entity.CustomerId, tenant);

                    if (card != null)
                    {
                        card.UpdateDate = entity.UpdateDate;
                        card.UpdatedByUserId = entity.UpdatedByUserId;
                        cardRepository.Update(card);
                        cardRepository.SubmitChanges();
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(entity.TicketId))
                {
                    #region Ticket

                    TicketRepository TicketRepository = new TicketRepository(crmContext);
                    Ticket Ticket = TicketRepository.GetSingle(entity.TicketId, tenant);

                    if (Ticket != null)
                    {
                        Ticket.UpdateDate = entity.UpdateDate;
                        Ticket.UpdatedByUserId = entity.UpdatedByUserId;
                        Ticket.LastCompletedActivitySubject = entity.Subject;
                        Ticket.LastCompletedActivityTypeCode = entity.ActivityTypeCode;
                        Ticket.LastCompletedActivityDate = entity.CompleteDate;
                        TicketRepository.Update(Ticket);
                        TicketRepository.SubmitChanges();

                        //Next activity
                        IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByTicketId(entity.TicketId, tenant);
                        iQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);
                        Activity nextDBActivity = iQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();

                        string nextSubject = null;
                        string nextTypeCode = null;
                        DateTime? nextDateTime = null;
                        if (nextDBActivity != null)
                        {
                            nextSubject = nextDBActivity.Subject;
                            nextTypeCode = nextDBActivity.ActivityTypeCode;
                            nextDateTime = nextDBActivity.StartDateTime;
                        }

                        Ticket.NextActivityTypeCode = nextTypeCode;
                        Ticket.NextActivityDate = nextDateTime;
                        Ticket.NextActivitySubject = nextSubject;
                        TicketRepository.Update(Ticket);
                        TicketRepository.SubmitChanges();
                    }


                    this.AddCorrespondenceLine(entity.Id, "LineCompleted");

                    #endregion
                }
            }
        }

        private void CloseActivityAndSaveChanges(string summary, ActivityPM entityPM, Contact loggedContact)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.IsOpen = false;
            entityPM.ActivityStatusCode = "C";
            entityPM.MeetingSummary = summary;
            entityPM.NeedSynchronization = true;
            entityPM.CompleteDate = todayDateTime;
            entityPM.UpdateDate = todayDateTime;
            this.InitializeSortingFields(entityPM);
            if (loggedContact != null)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            ActivityUpdateService service = new ActivityUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            service.InitializeEntityPM(entityPM);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(entityPM, true);
        }

        [Invoke]
        public void ReopenActivity(string activityId, int tenant)
        {
            SecurityUtility.CheckContactFeature("Activity", "UPDATE", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            activityRepository = new ActivityRepository(tenant);
            Activity entity = activityRepository.GetSingle(activityId, tenant);

            if (entity != null)
            {
                entity.IsOpen = true;
                entity.ActivityStatusCode = "N";
                entity.MeetingSummary = null;
                entity.CompleteDate = null;
                entity.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                this.InitializeSortingFields(entity);

                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                if (loggedContact != null)
                {
                    entity.UpdatedByUserId = loggedContact.Id;
                }

                activityRepository.Update(entity);
                activityRepository.SubmitChanges();

                this.CreateEntityEvent(entity.UpdatedByUserId, entity, "RP");

                if (!string.IsNullOrEmpty(entity.OpportunityId))
                {
                    IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByOpportunityId(entity.OpportunityId, entity.Tenant);

                    IQueryable<Activity> nextIQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);
                    IQueryable<Activity> lastIQueryableActivities = iQueryableActivities.Where(d => d.CompleteDate != null && !d.IsOpen);

                    Activity nextDBActivity = nextIQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();
                    Activity lastDBActivity = lastIQueryableActivities.OrderByDescending(d => d.CompleteDate).FirstOrDefault();

                    string nextSubject = null;
                    string nextTypeCode = null;
                    DateTime? nextDateTime = null;
                    if (nextDBActivity != null)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }

                    string lastSubject = null;
                    string lastTypeCode = null;
                    DateTime? lastDateTime = null;
                    if (lastDBActivity != null)
                    {
                        lastSubject = lastDBActivity.Subject;
                        lastTypeCode = lastDBActivity.ActivityTypeCode;
                        lastDateTime = lastDBActivity.StartDateTime;
                    }

                    OpportunityRepository opportunityRepository = new OpportunityRepository(entity.Tenant);
                    Opportunity opportunity = opportunityRepository.GetSingle(entity.OpportunityId, entity.Tenant);

                    if (opportunity != null)
                    {
                        opportunity.UpdateDate = entity.UpdateDate;
                        opportunity.UpdatedByUserId = entity.UpdatedByUserId;

                        opportunity.NextActivitySubject = nextSubject;
                        opportunity.NextActivityTypeCode = nextTypeCode;
                        opportunity.NextActivityDate = nextDateTime;

                        opportunity.LastActivitySubject = lastSubject;
                        opportunity.LastCompletedActivityTypeCode = lastTypeCode;
                        opportunity.LastCompletedActivityDate = lastDateTime;

                        opportunityRepository.Update(opportunity);
                        opportunityRepository.SubmitChanges();
                    }
                }

                if (!string.IsNullOrEmpty(entity.QuoteId))
                {
                    IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByQuoteId(entity.QuoteId, entity.Tenant);

                    IQueryable<Activity> nextIQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);
                    IQueryable<Activity> lastIQueryableActivities = iQueryableActivities.Where(d => d.CompleteDate != null && !d.IsOpen);

                    Activity nextDBActivity = nextIQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();
                    Activity lastDBActivity = lastIQueryableActivities.OrderByDescending(d => d.CompleteDate).FirstOrDefault();

                    string nextSubject = null;
                    string nextTypeCode = null;
                    DateTime? nextDateTime = null;
                    if (nextDBActivity != null)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }

                    string lastSubject = null;
                    string lastTypeCode = null;
                    DateTime? lastDateTime = null;
                    if (lastDBActivity != null)
                    {
                        lastSubject = lastDBActivity.Subject;
                        lastTypeCode = lastDBActivity.ActivityTypeCode;
                        lastDateTime = lastDBActivity.StartDateTime;
                    }

                    QuoteRepository quoteRepository = new QuoteRepository(entity.Tenant);
                    Quote quote = quoteRepository.GetSingleQuote(entity.QuoteId, entity.Tenant);

                    if (quote != null)
                    {
                        //quote.UpdateDate = entity.UpdateDate;
                        //quote.UpdatedByUserId = entity.UpdatedByUserId;

                        quote.NextActivitySubject = nextSubject;
                        quote.NextActivityTypeCode = nextTypeCode;
                        quote.NextActivityDate = nextDateTime;

                        quote.LastActivitySubject = lastSubject;
                        quote.LastActivityTypeCode = lastTypeCode;
                        quote.LastActivityDate = lastDateTime;

                        quoteRepository.Update(quote);
                        quoteRepository.SubmitChanges();
                    }
                }

                if (!string.IsNullOrEmpty(entity.CustomerId))
                {
                    CardRepository cardRepository = new CardRepository(tenant);
                    Card card = cardRepository.GetSingleCard(entity.CustomerId, tenant);

                    if (card != null)
                    {
                        card.UpdateDate = entity.UpdateDate;
                        card.UpdatedByUserId = entity.UpdatedByUserId;
                        cardRepository.Update(card);
                        cardRepository.SubmitChanges();
                    }
                }

                if (!string.IsNullOrEmpty(entity.TicketId))
                {
                    #region Ticket

                    IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByTicketId(entity.TicketId, entity.Tenant);

                    IQueryable<Activity> nextIQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);
                    IQueryable<Activity> lastIQueryableActivities = iQueryableActivities.Where(d => d.CompleteDate != null && !d.IsOpen);

                    Activity nextDBActivity = nextIQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();
                    Activity lastDBActivity = lastIQueryableActivities.OrderByDescending(d => d.CompleteDate).FirstOrDefault();

                    string nextSubject = null;
                    string nextTypeCode = null;
                    DateTime? nextDateTime = null;
                    if (nextDBActivity != null)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }

                    string lastSubject = null;
                    string lastTypeCode = null;
                    DateTime? lastDateTime = null;
                    if (lastDBActivity != null)
                    {
                        lastSubject = lastDBActivity.Subject;
                        lastTypeCode = lastDBActivity.ActivityTypeCode;
                        lastDateTime = lastDBActivity.StartDateTime;
                    }

                    TicketRepository TicketRepository = new TicketRepository(entity.Tenant);
                    Ticket Ticket = TicketRepository.GetSingle(entity.TicketId, entity.Tenant);

                    if (Ticket != null)
                    {
                        Ticket.UpdateDate = entity.UpdateDate;
                        Ticket.UpdatedByUserId = entity.UpdatedByUserId;

                        Ticket.NextActivitySubject = nextSubject;
                        Ticket.NextActivityTypeCode = nextTypeCode;
                        Ticket.NextActivityDate = nextDateTime;

                        Ticket.LastCompletedActivitySubject = lastSubject;
                        Ticket.LastCompletedActivityTypeCode = lastTypeCode;
                        Ticket.LastCompletedActivityDate = lastDateTime;

                        TicketRepository.Update(Ticket);
                        TicketRepository.SubmitChanges();
                    }

                    this.AddCorrespondenceLine(entity.Id, "LineOpened");

                    #endregion
                }
            }
        }

        private void InitializeSortingFields(ActivityPM entityPM)
        {
            switch (entityPM.ActivityTypeCode)
            {
                case "TS":
                    {
                        if (entityPM.CompleteDate != null)
                        {
                            entityPM.SortingDate = entityPM.CompleteDate;
                            entityPM.SortingBy = "Complete Date";
                        }

                        else if (entityPM.DueDate != null)
                        {
                            entityPM.SortingDate = entityPM.DueDate;
                            entityPM.SortingBy = "Due Date";
                        }

                        else if (entityPM.StartDateTime != null)
                        {
                            entityPM.SortingDate = entityPM.StartDateTime;
                            entityPM.SortingBy = "Start Date";
                        }

                        else if (entityPM.CreateDate != null)
                        {
                            entityPM.SortingDate = entityPM.CreateDate;
                            entityPM.SortingBy = "Create Date";
                        }

                        break;
                    }

                case "AP":
                    {
                        if (entityPM.CompleteDate != null)
                        {
                            entityPM.SortingDate = entityPM.CompleteDate;
                            entityPM.SortingBy = "Complete Date";
                        }

                        else if (entityPM.StartDateTime != null)
                        {
                            entityPM.SortingDate = entityPM.StartDateTime;
                            entityPM.SortingBy = "Start Date";
                        }

                        break;
                    }

                case "CL":
                    {
                        if (entityPM.CompleteDate != null)
                        {
                            entityPM.SortingDate = entityPM.CompleteDate;
                            entityPM.SortingBy = "Complete Date";
                        }

                        else if (entityPM.DueDate != null)
                        {
                            entityPM.SortingDate = entityPM.DueDate;
                            entityPM.SortingBy = "Due Date";
                        }

                        else if (entityPM.CreateDate != null)
                        {
                            entityPM.SortingDate = entityPM.CreateDate;
                            entityPM.SortingBy = "Create Date";
                        }

                        break;
                    }

                case "EI":
                    {
                        if (entityPM.SendReceiveDate != null)
                        {
                            entityPM.SortingDate = entityPM.SendReceiveDate;
                            entityPM.SortingBy = "Send/Receive Date";
                        }
                        break;
                    }

                case "EO":
                    {
                        if (entityPM.SendReceiveDate != null)
                        {
                            entityPM.SortingDate = entityPM.SendReceiveDate;
                            entityPM.SortingBy = "Send/Receive Date";
                        }
                        break;
                    }
            }
        }
        private void InitializeSortingFields(Activity entity)
        {
            switch (entity.ActivityTypeCode)
            {
                case "TS":
                    {
                        if (entity.CompleteDate != null)
                        {
                            entity.SortingDate = entity.CompleteDate;
                            entity.SortingBy = "Complete Date";
                        }

                        else if (entity.DueDate != null)
                        {
                            entity.SortingDate = entity.DueDate;
                            entity.SortingBy = "Due Date";
                        }

                        else if (entity.StartDateTime != null)
                        {
                            entity.SortingDate = entity.StartDateTime;
                            entity.SortingBy = "Start Date";
                        }

                        else if (entity.CreateDate != null)
                        {
                            entity.SortingDate = entity.CreateDate;
                            entity.SortingBy = "Create Date";
                        }

                        break;
                    }

                case "AP":
                    {
                        if (entity.CompleteDate != null)
                        {
                            entity.SortingDate = entity.CompleteDate;
                            entity.SortingBy = "Complete Date";
                        }

                        else if (entity.StartDateTime != null)
                        {
                            entity.SortingDate = entity.StartDateTime;
                            entity.SortingBy = "Start Date";
                        }

                        break;
                    }

                case "CL":
                    {
                        if (entity.CompleteDate != null)
                        {
                            entity.SortingDate = entity.CompleteDate;
                            entity.SortingBy = "Complete Date";
                        }

                        else if (entity.DueDate != null)
                        {
                            entity.SortingDate = entity.DueDate;
                            entity.SortingBy = "Due Date";
                        }

                        else if (entity.CreateDate != null)
                        {
                            entity.SortingDate = entity.CreateDate;
                            entity.SortingBy = "Create Date";
                        }

                        break;
                    }

                case "EI":
                    {
                        if (entity.SendReceiveDate != null)
                        {
                            entity.SortingDate = entity.SendReceiveDate;
                            entity.SortingBy = "Send/Receive Date";
                        }
                        break;
                    }

                case "EO":
                    {
                        if (entity.SendReceiveDate != null)
                        {
                            entity.SortingDate = entity.SendReceiveDate;
                            entity.SortingBy = "Send/Receive Date";
                        }
                        break;
                    }
            }
        }

        private void CreateEntityEvent(string loggedUserId, Activity entity, string eventTypeCode)
        {
            string myActivityTypeName = null;
            ActivityTypeRepository typeRepository = new ActivityTypeRepository(entity.Tenant);
            ActivityType type = typeRepository.GetSingle(entity.ActivityTypeCode);
            if (type != null)
            {
                myActivityTypeName = type.Name;
            }

            string myEventCode = null;
            string myEventEntityId = null;
            string myEventTableName = null;
            string myEventNotes = myActivityTypeName + ": " + entity.Subject;

            if (!string.IsNullOrEmpty(entity.OpportunityId))
            {
                myEventEntityId = entity.OpportunityId;
                myEventTableName = "Opportunity";

                switch (eventTypeCode)
                {
                    case "CR": { myEventCode = "OACR"; break; }
                    case "CM": { myEventCode = "OACM"; break; }
                    case "RP": { myEventCode = "OARP"; break; }
                }
            }

            else if (!string.IsNullOrEmpty(entity.QuoteId))
            {
                myEventEntityId = entity.QuoteId;
                myEventTableName = "Quote";

                switch (eventTypeCode)
                {
                    case "CR": { myEventCode = "QACR"; break; }
                    case "CM": { myEventCode = "QACM"; break; }
                    case "RP": { myEventCode = "QARP"; break; }
                }
            }

            else if (!string.IsNullOrEmpty(entity.CustomerId))
            {
                myEventEntityId = entity.CustomerId;
                myEventTableName = "Customer";

                switch (eventTypeCode)
                {
                    case "CR": { myEventCode = "CACR"; break; }
                    case "CM": { myEventCode = "CACM"; break; }
                    case "RP": { myEventCode = "CARP"; break; }
                }
            }

            else
            {
                myEventEntityId = entity.Id;
                myEventTableName = "Activity";

                switch (eventTypeCode)
                {
                    case "CR": { myEventCode = "CRAV"; break; }
                    case "CM": { myEventCode = "COAV"; break; }
                    case "RP": { myEventCode = "ROAV"; break; }
                }
            }

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = entity.Id,
                ObjectTableName = "Activity",
                Tenant = entity.Tenant,
                UserId = loggedUserId,
                EventTypeCode = "UPAV",
            });

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EventTypeCode = myEventCode,
                EntityId = myEventEntityId,
                ObjectTableName = myEventTableName,
                Tenant = entity.Tenant,
                UserId = loggedUserId,
                Notes = myEventNotes,
            });
        }

        public void InsertActivity(ActivityPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckFeatureAccessLevelPermission("Activity", "NEW", entityPM.OwnerId, entityPM.BusinessUnitId, entityPM.Tenant);

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
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            SetActivityNotesChangeSet(entityPM);
            SetActivityInviteesChangeSet(entityPM);
            SetActivityEmailRecipientsChangeSet(entityPM);

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            ActivityUpdateService service = new ActivityUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            service.Update(entityPM, true);
        }

        public void UpdateActivity(ActivityPM entityPM)
        {
            activityRepository = new ActivityRepository(entityPM.Tenant);
            ActivityKeys keys = new ActivityKeys() { Id = entityPM.Id };
            Activity entity_Poco = activityRepository.GetSingle(keys);

            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckFeatureAccessLevelPermission("Activity", "UPDATE", entity_Poco.OwnerId, entity_Poco.BusinessUnitId, entityPM.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdateDate = todayDate;

            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            SetActivityNotesChangeSet(entityPM);
            SetActivityInviteesChangeSet(entityPM);

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            ActivityUpdateService service = new ActivityUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            service.Update(entityPM, true);
        }

        private void SetActivityNotesChangeSet(ActivityPM entityPM)
        {
            List<ActivityNotePM> entityChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ActivityNotes).Cast<ActivityNotePM>().ToList();

            foreach (ActivityNotePM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ActivityNotePM currentItemPM = entityPM.ActivityNotes.Where(d => d.ActivityId == itemPM.ActivityId && d.ChangeSetOp != ChangeSetOperation.Insert && d.Id == null).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ActivityNotePM currentItemPM = entityPM.ActivityNotes.Where(d => d.ActivityId == itemPM.ActivityId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            ActivityNotePM currentItemPM = new ActivityNotePM() { ChangeSetOp = ChangeSetOperation.Delete, ActivityId = itemPM.ActivityId, Id = itemPM.Id };
                            entityPM.DeletedActivityNotes.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            ActivityNotePM currentItemPM = entityPM.ActivityNotes.Where(d => d.ActivityId == itemPM.ActivityId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetActivityInviteesChangeSet(ActivityPM entityPM)
        {
            List<ActivityInviteePM> activityInviteesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ActivityInvitees).Cast<ActivityInviteePM>().ToList();

            foreach (ActivityInviteePM itemPM in activityInviteesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ActivityInviteePM currentItemPM = entityPM.ActivityInvitees.Where(d => d.ActivityId == itemPM.ActivityId && d.Email == itemPM.Email && d.ContactId == itemPM.ContactId && d.IsRequired == itemPM.IsRequired).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ActivityInviteePM currentItemPM = entityPM.ActivityInvitees.Where(d => d.ActivityId == itemPM.ActivityId && d.Email == itemPM.Email && d.ContactId == itemPM.ContactId && d.IsRequired == itemPM.IsRequired && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            ActivityInviteePM currentItemPM = new ActivityInviteePM() { ChangeSetOp = ChangeSetOperation.Delete, ActivityId = itemPM.ActivityId, Email = itemPM.Email, ContactId = itemPM.ContactId, IsRequired = itemPM.IsRequired, Id = itemPM.Id };
                            entityPM.DeletedActivityInvitees.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            ActivityInviteePM currentItemPM = entityPM.ActivityInvitees.Where(d => d.ActivityId == itemPM.ActivityId && d.Email == itemPM.Email && d.ContactId == itemPM.ContactId && d.IsRequired == itemPM.IsRequired && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetActivityEmailRecipientsChangeSet(ActivityPM entityPM)
        {
            List<ActivityEmailRecipientPM> entityChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ActivityEmailRecipients).Cast<ActivityEmailRecipientPM>().ToList();

            foreach (ActivityEmailRecipientPM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ActivityEmailRecipientPM currentItemPM = entityPM.ActivityEmailRecipients.Where(d => d.ActivityId == itemPM.ActivityId && d.Email == itemPM.Email && d.ContactId == itemPM.ContactId && d.RecipientTypeCode == itemPM.RecipientTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ActivityEmailRecipientPM currentItemPM = entityPM.ActivityEmailRecipients.Where(d => d.ActivityId == itemPM.ActivityId && d.Email == itemPM.Email && d.ContactId == itemPM.ContactId && d.RecipientTypeCode == itemPM.RecipientTypeCode && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            ActivityEmailRecipientPM currentItemPM = new ActivityEmailRecipientPM() { ChangeSetOp = ChangeSetOperation.Delete, ActivityId = itemPM.ActivityId, Email = itemPM.Email, ContactId = itemPM.ContactId, RecipientTypeCode = itemPM.RecipientTypeCode, Id = itemPM.Id };
                            entityPM.DeletedActivityEmailRecipients.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            ActivityEmailRecipientPM currentItemPM = entityPM.ActivityEmailRecipients.Where(d => d.ActivityId == itemPM.ActivityId && d.Email == itemPM.Email && d.ContactId == itemPM.ContactId && d.RecipientTypeCode == itemPM.RecipientTypeCode && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public List<ChartingDataClass> GetActivitiesDashBoard(string ownerId, string businessUnitId, int tenant, string activityTypeCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            activityQuery = new ActivityQueryService(crmContext);

            List<CRMChartingClass> data = activityQuery.GetActivitiesDashBoard(ownerId, businessUnitId, tenant, activityTypeCode, "S");

            List<ChartingDataClass> result = new List<ChartingDataClass>();
            foreach (CRMChartingClass item in data)
            {
                result.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DateTimeProperty = item.DateTimeProperty,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    LabelProperty = item.LabelProperty,
                    TypeIndex = item.TypeIndex,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                    DataTypeCode=item.DataTypeCode
                });
            }

            return result;
        }
        public List<ChartingDataClass> GetActivitiesChartDataCustom(DateTime?FromDate,DateTime?ToDate, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            activityQuery = new ActivityQueryService(crmContext);

            List<CRMChartingClass> data = activityQuery.GetActivitiesChartDataCustom(FromDate,ToDate, ownerId, businessUnitId, chartCode, tenant);
         

            foreach (CRMChartingClass item in data)
            {
                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    TypeIndex = item.TypeIndex,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetActivitiesChartData(string code, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            activityQuery = new ActivityQueryService(crmContext);

            List<CRMChartingClass> data = activityQuery.GetActivitiesChartData(code, ownerId, businessUnitId, chartCode, tenant);

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            int days = Convert.ToInt32(str);

            foreach (CRMChartingClass item in data)
            {
                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    TypeIndex = item.TypeIndex,
                    Day = days,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                    Code = code,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetActivitiesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            activityQuery = new ActivityQueryService(crmContext);

            List<CRMChartingClass> data = activityQuery.GetActivitiesGroupBySalesman(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            int days = Convert.ToInt32(str);

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
                    BusinessUnitId = businessUnitId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                });
            }

            return myResult;
        }
        public List<ChartingDataClass> GetActivitiesGroupBySalesmanCustom(DateTime? FromDate,DateTime? ToDate, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            activityQuery = new ActivityQueryService(crmContext);

            List<CRMChartingClass> data = activityQuery.GetActivitiesGroupBySalesmanCustom(FromDate,ToDate, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

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
                    BusinessUnitId = businessUnitId,
                    ShortLabelProperty = myShortLabelProperty,
                });
            }

            return myResult;
        }

        public List<EntityPartner> GetActivityEntityPartners(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            List<EntityPartner> list = new List<EntityPartner>();

            return list;
        }

        public List<ActivityList> GetActivitiesByTicketId(string ticketId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            return listService.GetActivitiesByTicketId(ticketId, tenant);
        }

        private void AddCorrespondenceLine(string id, string isCompleted)
        {
            activityQuery = new ActivityQueryService(crmContext);
            ActivityPM entityPM = activityQuery.GetSingle(id, true, false);

            if (!string.IsNullOrEmpty(entityPM.TicketId))
            {
                CorrespondenceRepository correspondenceRepository = new CorrespondenceRepository(entityPM.Tenant);
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(entityPM.Tenant);
                ObjectTable objectTable = objectTableRepository.GetObjectTableByName("Ticket", 0, true);
                DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                TicketRepository ticketRep = new TicketRepository(entityPM.Tenant);
                Ticket ticket = ticketRep.GetSingle(entityPM.TicketId, entityPM.Tenant);

                string ownername = "";
                string Description = "";
                string meetingSummary = "";

                if (!string.IsNullOrEmpty(entityPM.ActivityTypeCode))
                {
                    if (entityPM.ActivityTypeCode == "AP" && !string.IsNullOrEmpty(entityPM.MeetingSummary))
                    {
                        meetingSummary = "\n" + "Metting Summary : " + entityPM.MeetingSummary;
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.OwnerId))
                {
                    UserRepository userRepository = new UserRepository(entityPM.Tenant);
                    User user = userRepository.GetSingleUser(entityPM.OwnerId, entityPM.Tenant, false);
                    if (user != null)
                    {
                        ownername = user.Contact.EnglishName;
                    }
                }

                if (string.IsNullOrEmpty(ownername))
                {
                    ownername = "";
                }

                else
                {
                    ownername = "Owner : " + ownername;
                }

                if (isCompleted == "LineCreated")
                {
                    Description = "New " + entityPM.ActivityTypeName + " Created\n" +
                                  "Subject : " + entityPM.Subject + "\n" +
                                   ownername + meetingSummary;
                }

                if (isCompleted == "LineCompleted")
                {
                    Description = entityPM.ActivityTypeName + " Completed\n" +
                                 "Subject : " + entityPM.Subject + "\n" +
                                  ownername + meetingSummary;
                }

                if (isCompleted == "LineOpened")
                {
                    Description = entityPM.ActivityTypeName + " Re-Opened\n" +
                                "Subject : " + entityPM.Subject + "\n" +
                                ownername + meetingSummary;
                }

                Correspondence line = new Correspondence()
                {
                    Id = IdCounter.GetNumber("Correspondence", entityPM.Tenant),
                    ActivityId = entityPM.Id,
                    ActivitySubject = entityPM.Subject,
                    ActivityTypeCode = entityPM.ActivityTypeCode,
                    Tenant = entityPM.Tenant,
                    EntityId = entityPM.TicketId,
                    CreateDate = myDate,
                    CreatedByContactId = entityPM.CreatedByUserId,
                    IsInternal = false,
                    Description = Description,
                    ObjectTableId = objectTable.Id,
                    NotifyMe = false,
                };

                correspondenceRepository.Add(line);
                correspondenceRepository.SubmitChanges();
            }
        }
    }
}