using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.CustomFilters;
using Simplog.Data.Helpers;
using Logitude.CRM.Data.BusinessUnitFilters;
using Logitude.CRM.Data.EntityKeys;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class ActivityListQueryService
    {
	    private IQueryable<ActivityList> GetIqueryableList(IQueryable<Activity> iQueryable)
        {
            int tenant = 0;
            if (iQueryable.Count() > 0)
            {
                tenant = iQueryable.First().Tenant;

                ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
                iQueryable = filter.RunFilter(iQueryable);
            }

            IQueryable<ActivityList> query = (from a in iQueryable.Include("Customer").Include("ActivityType").Include("ActivityStatus").Include("ActivityPriority").Include("Owner").Include("Opportunity").Include("BusinessUnit").Include("Owner.Contact").Include("CreatedByUser.Contact").Include("CallWith").Include("BusinessProcessQueue").Include("Team")
                                              select new ActivityList()
                                              {
                                                  Id = a.Id,
                                                  Subject = a.Subject,
                                                  DueDate = a.DueDate,
                                                  PriorityCode = a.PriorityCode,
                                                  StartDateTime = a.StartDateTime,
                                                  EndDateTime = a.EndDateTime,
                                                  Description = a.Description,
                                                  CallPurpose = a.CallPurpose,
                                                  CallDetails = a.CallDetails,
                                                  CallResult = a.CallResult,
                                                  PhoneNumber = a.PhoneNumber,
                                                  Duration = a.Duration,
                                                  Notes = a.Notes,
                                                  CreateDate = a.CreateDate,
                                                  UpdateDate = a.UpdateDate,
                                                  UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                                                  AllDayEvent = a.AllDayEvent,
                                                  Location = a.Location,
                                                  SearchFields = a.SearchFields,
                                                  ActivityTypeCode = a.ActivityTypeCode,
                                                  ActivityStatusCode = a.ActivityStatusCode,
                                                  ActivityStatusName = a.ActivityStatus == null ? "" : a.ActivityStatus.Name,
                                                  PriorityName = a.ActivityPriority == null ? "" : a.ActivityPriority.Name,
                                                  OwnerName = a.Owner == null ? "" : (a.Owner.Contact == null ? "" : a.Owner.Contact.EnglishName),
                                                  IsLeftVoiceMail = a.IsLeftVoiceMail,
                                                  OutlookId = a.OutlookId,
                                                  NeedSynchronization = a.NeedSynchronization,
                                                  //Background = a.IsOpen ? "White" : "#7DEBEBEB",
                                                  IsOpen = a.IsOpen,
                                                  ActivityTypeName = (a.ActivityTypeCode == "CL" && a.IsLeftVoiceMail) ? "Voice Mail" : (a.ActivityType == null ? "" : a.ActivityType.Name),
                                                  ActivityTypePathCode = (a.ActivityTypeCode == "CL" && a.IsLeftVoiceMail) ? "VM" : a.ActivityTypeCode,
                                                  OpportunitySubject = a.Opportunity != null ? a.Opportunity.Subject : null,
                                                  OpportunityId = a.OpportunityId,
                                                  Field1 = a.Field1,
                                                  Field2 = a.Field2,
                                                  Field3 = a.Field3,
                                                  Field4 = a.Field4,
                                                  Field5 = a.Field5,
                                                  Field6 = a.Field6,
                                                  Field7 = a.Field7,
                                                  Field8 = a.Field8,
                                                  Field9 = a.Field9,
                                                  Field10 = a.Field10,
                                                  MeetingSummary = a.MeetingSummary,
                                                  CompleteDate = a.CompleteDate,
                                                  BusinessUnitId = a.BusinessUnitId,
                                                  BusinessUnitName = a.BusinessUnit == null ? null : a.BusinessUnit.Name,
                                                  CommunicationLogId = a.CommunicationLogId,
                                                  SenderEmail = a.SenderEmail,
                                                  CustomerName = a.Customer == null ? "" : a.Customer.EnglishName,
                                                  OwnerId = a.OwnerId,
                                                  CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                                                  CreatedByUserId = a.CreatedByUserId,
                                                  QuoteId = a.QuoteId,
                                                  CustomerId = a.CustomerId,
                                                  SortByDate = a.SortingDate,
                                                  SortingBy = a.SortingBy,
                                                  SendReceiveDate = a.SendReceiveDate,
                                                  ActivityWith = a.ActivityWith,
                                                  CallWithId = a.CallWithId,
                                                  From = a.From,
                                                  ArchiveDate = (!a.IsOpen && a.ActivityStatusCode == "C") ? a.CompleteDate : a.UpdateDate,
                                                  Cc = a.Cc,
                                                  To = a.To,
                                                  CallWithName = a.CallWith == null ? null : a.CallWith.EnglishName,
                                                  BusinessProcessQueueName = a.BusinessProcessQueue == null ? null : a.BusinessProcessQueue.Name,
                                                  TeamName = a.Team == null ? null : a.Team.Name,
                                                  DueDateDateField = a.DueDateDateField,
                                                  DueDateOffset = a.DueDateOffset,
                                              });
            return query;
		}

        public List<ActivityList> GetRecentEntityLists(int tenant, string userId, string objectTableId)
        {
            List<ActivityList> entityList = new List<ActivityList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            ActivityRepository entityRepository = new ActivityRepository(tenant);
            IQueryable<Activity> entities = entityRepository.GetAllFromIdList(ids, tenant);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            entities = filter.RunFilter(entities);

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                Activity entityPoco = (from a in entities.Include("Customer").Include("ActivityType").Include("ActivityStatus").Include("ActivityPriority").Include("Owner").Include("Owner.Contact").Include("Opportunity").Include("BusinessUnit").Include("CreatedByUser.Contact").Include("CallWith")
                                       where a.Id == lastActivity.EntityId
                                       select a).FirstOrDefault();

                if (entityPoco != null)
                {
                    ActivityList list = new ActivityList()
                    {
                        Id = entityPoco.Id,
                        Subject = entityPoco.Subject,
                        DueDate = entityPoco.DueDate,
                        PriorityCode = entityPoco.PriorityCode,
                        StartDateTime = entityPoco.StartDateTime,
                        EndDateTime = entityPoco.EndDateTime,
                        Description = entityPoco.Description,
                        CallPurpose = entityPoco.CallPurpose,
                        CallDetails = entityPoco.CallDetails,
                        CallResult = entityPoco.CallResult,
                        PhoneNumber = entityPoco.PhoneNumber,
                        Duration = entityPoco.Duration,
                        Notes = entityPoco.Notes,
                        CreateDate = entityPoco.CreateDate,
                        UpdateDate = entityPoco.UpdateDate,
                        AllDayEvent = entityPoco.AllDayEvent,
                        Location = entityPoco.Location,
                        ActivityStatusCode = entityPoco.ActivityStatusCode,
                        SearchFields = entityPoco.SearchFields,
                        ActivityTypeCode = entityPoco.ActivityTypeCode,
                        ActivityStatusName = entityPoco.ActivityStatus == null ? "" : entityPoco.ActivityStatus.Name,
                        PriorityName = entityPoco.ActivityPriority == null ? "" : entityPoco.ActivityPriority.Name,
                        OwnerName = entityPoco.Owner == null ? "" : (entityPoco.Owner.Contact == null ? "" : entityPoco.Owner.Contact.EnglishName),
                        //LastActivityDate = lastActivity.ActivityDate,
                        //LastActivityTypeName = lastActivity.ActivityType.Name,
                        //LastActivityByUserName = lastActivity.User.Contact.EnglishName,
                        IsLeftVoiceMail = entityPoco.IsLeftVoiceMail,
                        OutlookId = entityPoco.OutlookId,
                        NeedSynchronization = entityPoco.NeedSynchronization,
                        //Background = entityPoco.IsOpen ? "White" : "#7DEBEBEB",
                        IsOpen = entityPoco.IsOpen,
                        ActivityTypeName = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "Voice Mail" : (entityPoco.ActivityType == null ? "" : entityPoco.ActivityType.Name),
                        ActivityTypePathCode = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "VM" : entityPoco.ActivityTypeCode,
                        OpportunitySubject = entityPoco.Opportunity != null ? entityPoco.Opportunity.Subject : null,
                        OpportunityId = entityPoco.OpportunityId,
                        Field1 = entityPoco.Field1,
                        Field2 = entityPoco.Field2,
                        Field3 = entityPoco.Field3,
                        Field4 = entityPoco.Field4,
                        Field5 = entityPoco.Field5,
                        Field6 = entityPoco.Field6,
                        Field7 = entityPoco.Field7,
                        Field8 = entityPoco.Field8,
                        Field9 = entityPoco.Field9,
                        Field10 = entityPoco.Field10,
                        MeetingSummary = entityPoco.MeetingSummary,
                        CompleteDate = entityPoco.CompleteDate,
                        BusinessUnitId = entityPoco.BusinessUnitId,
                        BusinessUnitName = entityPoco.BusinessUnit == null ? null : entityPoco.BusinessUnit.Name,
                        CommunicationLogId = entityPoco.CommunicationLogId,
                        SenderEmail = entityPoco.SenderEmail,
                        CustomerName = entityPoco.Customer == null ? "" : entityPoco.Customer.EnglishName,
                        CustomerId = entityPoco.CustomerId,
                        OwnerId = entityPoco.OwnerId,
                        CreatedByUserName = entityPoco.CreatedByUser == null ? "" : (entityPoco.CreatedByUser.Contact == null ? "" : entityPoco.CreatedByUser.Contact.EnglishName),
                        CreatedByUserId = entityPoco.CreatedByUserId,
                        QuoteId = entityPoco.QuoteId,
                        SortByDate = entityPoco.SortingDate,
                        SortingBy = entityPoco.SortingBy,
                        SendReceiveDate = entityPoco.SendReceiveDate,
                        CallWithId = entityPoco.CallWithId,
                        From = entityPoco.From,
                        Cc = entityPoco.Cc,
                        To = entityPoco.To,
                        CallWithName = entityPoco.CallWith == null ? null : entityPoco.CallWith.EnglishName,
                    };

                    entityList.Add(list);
                }
            }

            return entityList;
        }

        public List<ActivityList> GetUpcomigEntityLists(string ownerId, string businessUnitId, int tenant, string activityTypeCode, string RecordsTypeCode)
        {
            List<ActivityList> entityList = new List<ActivityList>();

            ActivityRepository entityRepository = new ActivityRepository(tenant);
            IQueryable<Activity> entities = entityRepository.GetAll(tenant).Where(d => d.IsOpen && d.ActivityStatusCode != "X");

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            entities = filter.RunFilter(entities);

            if (!string.IsNullOrEmpty(activityTypeCode))
            {
                entities = entities.Where(d => d.ActivityTypeCode == activityTypeCode);
            }

            if (RecordsTypeCode == "C")
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    entities = entities.Where(d => d.CreatedByUserId == ownerId);
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    entities = entities.Where(d => d.OwnerId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    entities = entities.Where(d => d.BusinessUnitId == businessUnitId);
                }
            }

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date = todayDate.Value.AddYears(10);

            foreach (Activity entityPoco in entities.Include("Customer").Include("ActivityType").Include("ActivityStatus").Include("ActivityPriority").Include("Owner").Include("Owner.Contact").Include("BusinessUnit").Include("CreatedByUser.Contact"))
            {
                ActivityList list = new ActivityList()
                {
                    Id = entityPoco.Id,
                    Subject = entityPoco.Subject,
                    DueDate = entityPoco.DueDate,
                    PriorityCode = entityPoco.PriorityCode,
                    StartDateTime = entityPoco.StartDateTime,
                    EndDateTime = entityPoco.EndDateTime,
                    Description = entityPoco.Description,
                    CallPurpose = entityPoco.CallPurpose,
                    CallDetails = entityPoco.CallDetails,
                    CallResult = entityPoco.CallResult,
                    PhoneNumber = entityPoco.PhoneNumber,
                    Duration = entityPoco.Duration,
                    Notes = entityPoco.Notes,
                    CreateDate = entityPoco.CreateDate,
                    UpdateDate = entityPoco.UpdateDate,
                    AllDayEvent = entityPoco.AllDayEvent,
                    Location = entityPoco.Location,
                    ActivityStatusCode = entityPoco.ActivityStatusCode,
                    SearchFields = entityPoco.SearchFields,
                    ActivityTypeCode = entityPoco.ActivityTypeCode,
                    ActivityStatusName = entityPoco.ActivityStatus == null ? "" : entityPoco.ActivityStatus.Name,
                    PriorityName = entityPoco.ActivityPriority == null ? "" : entityPoco.ActivityPriority.Name,
                    OwnerName = entityPoco.Owner == null ? "" : (entityPoco.Owner.Contact == null ? "" : entityPoco.Owner.Contact.EnglishName),
                    IsLeftVoiceMail = entityPoco.IsLeftVoiceMail,
                    OutlookId = entityPoco.OutlookId,
                    NeedSynchronization = entityPoco.NeedSynchronization,
                    //Background = entityPoco.IsOpen ? "White" : "#7DEBEBEB",
                    ActivityTypeName = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "Voice Mail" : (entityPoco.ActivityType == null ? "" : entityPoco.ActivityType.Name),
                    ActivityTypePathCode = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "VM" : entityPoco.ActivityTypeCode,
                    IsOpen = entityPoco.IsOpen,
                    Field1 = entityPoco.Field1,
                    Field2 = entityPoco.Field2,
                    Field3 = entityPoco.Field3,
                    Field4 = entityPoco.Field4,
                    Field5 = entityPoco.Field5,
                    Field6 = entityPoco.Field6,
                    Field7 = entityPoco.Field7,
                    Field8 = entityPoco.Field8,
                    Field9 = entityPoco.Field9,
                    Field10 = entityPoco.Field10,
                    MeetingSummary = entityPoco.MeetingSummary,
                    CompleteDate = entityPoco.CompleteDate,
                    BusinessUnitId = entityPoco.BusinessUnitId,
                    BusinessUnitName = entityPoco.BusinessUnit == null ? null : entityPoco.BusinessUnit.Name,
                    CommunicationLogId = entityPoco.CommunicationLogId,
                    SenderEmail = entityPoco.SenderEmail,
                    CustomerId = entityPoco.CustomerId,
                    CustomerName = entityPoco.Customer == null ? "" : entityPoco.Customer.EnglishName,
                    OpportunityId = entityPoco.OpportunityId,
                    OwnerId = entityPoco.OwnerId,
                    CreatedByUserName = entityPoco.CreatedByUser == null ? "" : (entityPoco.CreatedByUser.Contact == null ? "" : entityPoco.CreatedByUser.Contact.EnglishName),
                    CreatedByUserId = entityPoco.CreatedByUserId,
                    UpcomingDate = entityPoco.StartDateTime != null ? entityPoco.StartDateTime : entityPoco.DueDate,
                    SortByDate = entityPoco.StartDateTime != null ? entityPoco.StartDateTime : (entityPoco.DueDate != null ? entityPoco.DueDate : date),
                    QuoteId = entityPoco.QuoteId,
                    SortingDate = entityPoco.SortingDate,
                    SortingBy = entityPoco.SortingBy,
                    SendReceiveDate = entityPoco.SendReceiveDate,
                    CallWithId = entityPoco.CallWithId,
                    From = entityPoco.From,
                    Cc = entityPoco.Cc,
                    To = entityPoco.To,
                };

                entityList.Add(list);
            }

            return entityList.OrderBy(a => a.SortByDate).Take(20).ToList();
        }

        public List<ActivityList> GetActivitiesByOpportunityId(string opportunityId,int tenant)
        {
            ActivityRepository entityRepository = new ActivityRepository(tenant);
            ActivityEmailRecipientRepository emailRecipientRepository = new ActivityEmailRecipientRepository(tenant);
 
            IQueryable<Activity> entities = entityRepository.GetActivitiesByOpportunityId(opportunityId, tenant);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            entities = filter.RunFilter(entities);

            List<ActivityList> result = (from entityPoco in entities.Include("Customer").Include("ActivityType").Include("ActivityStatus").Include("ActivityPriority").Include("Owner").Include("Owner.Contact").Include("UpdatedByUser").Include("Opportunity").Include("BusinessUnit").Include("CreatedByUser.Contact")
                                         select new ActivityList()
                                         {
                                             Id = entityPoco.Id,
                                             Subject = entityPoco.Subject,
                                             DueDate = entityPoco.DueDate,
                                             PriorityCode = entityPoco.PriorityCode,
                                             StartDateTime = entityPoco.StartDateTime,
                                             EndDateTime = entityPoco.EndDateTime,
                                             Description = entityPoco.Description,
                                             CallPurpose = entityPoco.CallPurpose,
                                             CallDetails = entityPoco.CallDetails,
                                             CallResult = entityPoco.CallResult,
                                             PhoneNumber = entityPoco.PhoneNumber,
                                             Duration = entityPoco.Duration,
                                             Notes = entityPoco.Notes,
                                             CreateDate = entityPoco.CreateDate,
                                             UpdateDate = entityPoco.UpdateDate,
                                             AllDayEvent = entityPoco.AllDayEvent,
                                             Location = entityPoco.Location,
                                             SearchFields = entityPoco.SearchFields,
                                             ActivityStatusCode = entityPoco.ActivityStatusCode,
                                             ActivityTypeCode = entityPoco.ActivityTypeCode,
                                             ActivityStatusName = entityPoco.ActivityStatus == null ? "" : entityPoco.ActivityStatus.Name,
                                             PriorityName = entityPoco.ActivityPriority == null ? "" : entityPoco.ActivityPriority.Name,
                                             OwnerName = entityPoco.Owner == null ? "" : (entityPoco.Owner.Contact == null ? "" : entityPoco.Owner.Contact.EnglishName),
                                             UpdatedByUserName = entityPoco.UpdatedByUser == null ? "" : (entityPoco.UpdatedByUser.Contact == null ? "" : entityPoco.UpdatedByUser.Contact.EnglishName),
                                             IsLeftVoiceMail = entityPoco.IsLeftVoiceMail,
                                             OutlookId = entityPoco.OutlookId,
                                             NeedSynchronization = entityPoco.NeedSynchronization,
                                             //Background = entityPoco.IsOpen ? "White" : "#7DEBEBEB",
                                             IsOpen = entityPoco.IsOpen,
                                             ActivityTypeName = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "Voice Mail" : (entityPoco.ActivityType == null ? "" : entityPoco.ActivityType.Name),
                                             ActivityTypePathCode = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "VM" : entityPoco.ActivityTypeCode,
                                             OpportunitySubject = entityPoco.Opportunity != null ? entityPoco.Opportunity.Subject : null,
                                             OpportunityId = entityPoco.OpportunityId,
                                             Field1 = entityPoco.Field1,
                                             Field2 = entityPoco.Field2,
                                             Field3 = entityPoco.Field3,
                                             Field4 = entityPoco.Field4,
                                             Field5 = entityPoco.Field5,
                                             Field6 = entityPoco.Field6,
                                             Field7 = entityPoco.Field7,
                                             Field8 = entityPoco.Field8,
                                             Field9 = entityPoco.Field9,
                                             Field10 = entityPoco.Field10,
                                             MeetingSummary = entityPoco.MeetingSummary,
                                             CompleteDate = entityPoco.CompleteDate,
                                             BusinessUnitId = entityPoco.BusinessUnitId,
                                             BusinessUnitName = entityPoco.BusinessUnit == null ? null : entityPoco.BusinessUnit.Name,
                                             CommunicationLogId = entityPoco.CommunicationLogId,
                                             SenderEmail = entityPoco.SenderEmail,
                                             CustomerName = entityPoco.Customer == null ? "" : entityPoco.Customer.EnglishName,
                                             OwnerId = entityPoco.OwnerId,
                                             CreatedByUserName = entityPoco.CreatedByUser == null ? "" : (entityPoco.CreatedByUser.Contact == null ? "" : entityPoco.CreatedByUser.Contact.EnglishName),
                                             CreatedByUserId = entityPoco.CreatedByUserId,
                                             QuoteId = entityPoco.QuoteId,
                                             CustomerId = entityPoco.CustomerId,
                                             SortingDate = entityPoco.SortingDate,
                                             SortingBy = entityPoco.SortingBy,
                                             SendReceiveDate = entityPoco.SendReceiveDate,
                                             CallWithId = entityPoco.CallWithId,
                                             From = entityPoco.From,
                                             Cc = entityPoco.Cc,
                                             To = entityPoco.To,
                                         }).ToList();

            foreach (ActivityList list in result)
            {
                list.RecipientsEmails = "";

                if (list.ActivityTypeCode == "EO")
                {
                    List<ActivityEmailRecipient> recipientsList = emailRecipientRepository.GetMulti(new ActivityKeys() { Id = list.Id });
                    foreach (ActivityEmailRecipient recp in recipientsList)
                    {
                        if (!string.IsNullOrEmpty(recp.Email) && !list.RecipientsEmails.Contains(recp.Email))
                        {
                            list.RecipientsEmails += recp.Email + ";";
                        }
                    }

                    list.RecipientsEmails = list.RecipientsEmails.TrimEnd(';');
                }
            }


            return result;
        }

        public List<ActivityList> GetActivitiesByQuoteId(string quoteId, int tenant)
        {
            ActivityRepository entityRepository = new ActivityRepository(tenant);
            ActivityEmailRecipientRepository emailRecipientRepository = new ActivityEmailRecipientRepository(tenant);
            IQueryable<Activity> entities = entityRepository.GetActivitiesByQuoteId(quoteId, tenant);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            entities = filter.RunFilter(entities);

            List<ActivityList> result = (from entityPoco in entities.Include("Customer").Include("ActivityType").Include("ActivityStatus").Include("ActivityPriority").Include("Owner").Include("Owner.Contact").Include("UpdatedByUser").Include("Opportunity").Include("BusinessUnit").Include("CreatedByUser.Contact")
                                         select new ActivityList()
                                         {
                                             Id = entityPoco.Id,
                                             Subject = entityPoco.Subject,
                                             DueDate = entityPoco.DueDate,
                                             PriorityCode = entityPoco.PriorityCode,
                                             StartDateTime = entityPoco.StartDateTime,
                                             EndDateTime = entityPoco.EndDateTime,
                                             Description = entityPoco.Description,
                                             CallPurpose = entityPoco.CallPurpose,
                                             CallDetails = entityPoco.CallDetails,
                                             CallResult = entityPoco.CallResult,
                                             PhoneNumber = entityPoco.PhoneNumber,
                                             Duration = entityPoco.Duration,
                                             Notes = entityPoco.Notes,
                                             CreateDate = entityPoco.CreateDate,
                                             UpdateDate = entityPoco.UpdateDate,
                                             AllDayEvent = entityPoco.AllDayEvent,
                                             Location = entityPoco.Location,
                                             SearchFields = entityPoco.SearchFields,
                                             ActivityStatusCode = entityPoco.ActivityStatusCode,
                                             ActivityTypeCode = entityPoco.ActivityTypeCode,
                                             ActivityStatusName = entityPoco.ActivityStatus == null ? "" : entityPoco.ActivityStatus.Name,
                                             PriorityName = entityPoco.ActivityPriority == null ? "" : entityPoco.ActivityPriority.Name,
                                             OwnerName = entityPoco.Owner == null ? "" : (entityPoco.Owner.Contact == null ? "" : entityPoco.Owner.Contact.EnglishName),
                                             UpdatedByUserName = entityPoco.UpdatedByUser == null ? "" : (entityPoco.UpdatedByUser.Contact == null ? "" : entityPoco.UpdatedByUser.Contact.EnglishName),
                                             IsLeftVoiceMail = entityPoco.IsLeftVoiceMail,
                                             OutlookId = entityPoco.OutlookId,
                                             NeedSynchronization = entityPoco.NeedSynchronization,
                                             //Background = entityPoco.IsOpen ? "White" : "#7DEBEBEB",
                                             IsOpen = entityPoco.IsOpen,
                                             ActivityTypeName = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "Voice Mail" : (entityPoco.ActivityType == null ? "" : entityPoco.ActivityType.Name),
                                             ActivityTypePathCode = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "VM" : entityPoco.ActivityTypeCode,
                                             OpportunitySubject = entityPoco.Opportunity != null ? entityPoco.Opportunity.Subject : null,
                                             OpportunityId = entityPoco.OpportunityId,
                                             Field1 = entityPoco.Field1,
                                             Field2 = entityPoco.Field2,
                                             Field3 = entityPoco.Field3,
                                             Field4 = entityPoco.Field4,
                                             Field5 = entityPoco.Field5,
                                             Field6 = entityPoco.Field6,
                                             Field7 = entityPoco.Field7,
                                             Field8 = entityPoco.Field8,
                                             Field9 = entityPoco.Field9,
                                             Field10 = entityPoco.Field10,
                                             MeetingSummary = entityPoco.MeetingSummary,
                                             CompleteDate = entityPoco.CompleteDate,
                                             BusinessUnitId = entityPoco.BusinessUnitId,
                                             BusinessUnitName = entityPoco.BusinessUnit == null ? null : entityPoco.BusinessUnit.Name,
                                             CommunicationLogId = entityPoco.CommunicationLogId,
                                             SenderEmail = entityPoco.SenderEmail,
                                             CustomerName = entityPoco.Customer == null ? "" : entityPoco.Customer.EnglishName,
                                             OwnerId = entityPoco.OwnerId,
                                             CreatedByUserName = entityPoco.CreatedByUser == null ? "" : (entityPoco.CreatedByUser.Contact == null ? "" : entityPoco.CreatedByUser.Contact.EnglishName),
                                             CreatedByUserId = entityPoco.CreatedByUserId,
                                             QuoteId = entityPoco.QuoteId,
                                             CustomerId = entityPoco.CustomerId,
                                             SortingDate = entityPoco.SortingDate,
                                             SortingBy = entityPoco.SortingBy,
                                             SendReceiveDate = entityPoco.SendReceiveDate,
                                             CallWithId = entityPoco.CallWithId,
                                             From = entityPoco.From,
                                             Cc = entityPoco.Cc,
                                             To = entityPoco.To,
                                         }).ToList();

            foreach (ActivityList list in result)
            {
                list.RecipientsEmails = "";

                if (list.ActivityTypeCode == "EO")
                {
                    List<ActivityEmailRecipient> recipientsList = emailRecipientRepository.GetMulti(new ActivityKeys() { Id = list.Id });
                    foreach (ActivityEmailRecipient recp in recipientsList)
                    {
                        if (!string.IsNullOrEmpty(recp.Email) && !list.RecipientsEmails.Contains(recp.Email))
                        {
                            list.RecipientsEmails += recp.Email + ";";
                        }
                    }

                    list.RecipientsEmails = list.RecipientsEmails.TrimEnd(';');
                }
            }


            return result;
        }

        private IQueryable<Activity> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Activity> iQueryable,int tenant)
        {
            return ActivityCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);
        }

        private IQueryable<Activity> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Activity> iQueryable, int tenant)
        {
            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            iQueryable = filter.RunFilter(iQueryable);

            return iQueryable;
        }

        public bool OpportunityHasOpenActivities(string opportunityId, int tenant)
        {
            ActivityRepository entityRepository = new ActivityRepository(tenant);
            IQueryable<Activity> entities = entityRepository.GetActivitiesByOpportunityId(opportunityId, tenant);

            return entities.Where(d => d.IsOpen).Any();
        }

        public List<ActivityList> GetActivitiesByTicketId(string ticketId, int tenant)
        {
            ActivityRepository entityRepository = new ActivityRepository(tenant);
            ActivityEmailRecipientRepository emailRecipientRepository = new ActivityEmailRecipientRepository(tenant);

            IQueryable<Activity> entities = entityRepository.GetActivitiesByTicketId(ticketId, tenant);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            entities = filter.RunFilter(entities);

            List<ActivityList> result = (from entityPoco in entities.Include("Customer").Include("ActivityType").Include("ActivityStatus").Include("ActivityPriority").Include("Owner").Include("Owner.Contact").Include("UpdatedByUser").Include("Opportunity").Include("BusinessUnit").Include("CreatedByUser.Contact").Include("Ticket")
                                         select new ActivityList()
                                         {
                                             Id = entityPoco.Id,
                                             Subject = entityPoco.Subject,
                                             DueDate = entityPoco.DueDate,
                                             PriorityCode = entityPoco.PriorityCode,
                                             StartDateTime = entityPoco.StartDateTime,
                                             EndDateTime = entityPoco.EndDateTime,
                                             Description = entityPoco.Description,
                                             CallPurpose = entityPoco.CallPurpose,
                                             CallDetails = entityPoco.CallDetails,
                                             CallResult = entityPoco.CallResult,
                                             PhoneNumber = entityPoco.PhoneNumber,
                                             Duration = entityPoco.Duration,
                                             Notes = entityPoco.Notes,
                                             CreateDate = entityPoco.CreateDate,
                                             UpdateDate = entityPoco.UpdateDate,
                                             AllDayEvent = entityPoco.AllDayEvent,
                                             Location = entityPoco.Location,
                                             SearchFields = entityPoco.SearchFields,
                                             ActivityStatusCode = entityPoco.ActivityStatusCode,
                                             ActivityTypeCode = entityPoco.ActivityTypeCode,
                                             ActivityStatusName = entityPoco.ActivityStatus == null ? "" : entityPoco.ActivityStatus.Name,
                                             PriorityName = entityPoco.ActivityPriority == null ? "" : entityPoco.ActivityPriority.Name,
                                             OwnerName = entityPoco.Owner == null ? "" : (entityPoco.Owner.Contact == null ? "" : entityPoco.Owner.Contact.EnglishName),
                                             UpdatedByUserName = entityPoco.UpdatedByUser == null ? "" : (entityPoco.UpdatedByUser.Contact == null ? "" : entityPoco.UpdatedByUser.Contact.EnglishName),
                                             IsLeftVoiceMail = entityPoco.IsLeftVoiceMail,
                                             OutlookId = entityPoco.OutlookId,
                                             NeedSynchronization = entityPoco.NeedSynchronization,
                                             //Background = entityPoco.IsOpen ? "White" : "#7DEBEBEB",
                                             IsOpen = entityPoco.IsOpen,
                                             ActivityTypeName = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "Voice Mail" : (entityPoco.ActivityType == null ? "" : entityPoco.ActivityType.Name),
                                             ActivityTypePathCode = (entityPoco.ActivityTypeCode == "CL" && entityPoco.IsLeftVoiceMail) ? "VM" : entityPoco.ActivityTypeCode,
                                             OpportunitySubject = entityPoco.Opportunity != null ? entityPoco.Opportunity.Subject : null,
                                             OpportunityId = entityPoco.OpportunityId,
                                             Field1 = entityPoco.Field1,
                                             Field2 = entityPoco.Field2,
                                             Field3 = entityPoco.Field3,
                                             Field4 = entityPoco.Field4,
                                             Field5 = entityPoco.Field5,
                                             Field6 = entityPoco.Field6,
                                             Field7 = entityPoco.Field7,
                                             Field8 = entityPoco.Field8,
                                             Field9 = entityPoco.Field9,
                                             Field10 = entityPoco.Field10,
                                             MeetingSummary = entityPoco.MeetingSummary,
                                             CompleteDate = entityPoco.CompleteDate,
                                             BusinessUnitId = entityPoco.BusinessUnitId,
                                             BusinessUnitName = entityPoco.BusinessUnit == null ? null : entityPoco.BusinessUnit.Name,
                                             CommunicationLogId = entityPoco.CommunicationLogId,
                                             SenderEmail = entityPoco.SenderEmail,
                                             CustomerName = entityPoco.Customer == null ? "" : entityPoco.Customer.EnglishName,
                                             OwnerId = entityPoco.OwnerId,
                                             CreatedByUserName = entityPoco.CreatedByUser == null ? "" : (entityPoco.CreatedByUser.Contact == null ? "" : entityPoco.CreatedByUser.Contact.EnglishName),
                                             CreatedByUserId = entityPoco.CreatedByUserId,
                                             QuoteId = entityPoco.QuoteId,
                                             CustomerId = entityPoco.CustomerId,
                                             TicketId = entityPoco.TicketId,
                                             SortingDate = entityPoco.SortingDate,
                                             SortingBy = entityPoco.SortingBy,
                                             SendReceiveDate = entityPoco.SendReceiveDate,
                                             CallWithId = entityPoco.CallWithId,
                                             Cc = entityPoco.Cc,
                                             To = entityPoco.To,
                                             From = entityPoco.From,
                                         }).ToList();

            foreach (ActivityList list in result)
            {
                list.RecipientsEmails = "";

                if (list.ActivityTypeCode == "EO")
                {
                    List<ActivityEmailRecipient> recipientsList = emailRecipientRepository.GetMulti(new ActivityKeys() { Id = list.Id });
                    foreach (ActivityEmailRecipient recp in recipientsList)
                    {
                        if (!string.IsNullOrEmpty(recp.Email) && !list.RecipientsEmails.Contains(recp.Email))
                        {
                            list.RecipientsEmails += recp.Email + ";";
                        }
                    }

                    list.RecipientsEmails = list.RecipientsEmails.TrimEnd(';');
                }
            }
            return result;
        }
    }
}
	