using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System.Data;
using Logitude.CRM.BL.Validators;
using Logitude.CRM.BL.Helpers;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.Core;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class ActivityUpdateService
    {
        protected override void OnCreating(ActivityPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Activity", entityPM.Tenant);

                if (!string.IsNullOrEmpty(entityPM.TicketId))
                {
                    AddCorrespondenceLine(entityPM, "LineCreated"); // Created New Activity Line
                }

                DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.CreateDate = myDate;
                entityPM.UpdateDate = myDate;

                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(entityPM.Tenant), entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }

                if (entityPM.IsMarkedCompleted)
                {
                    entityPM.CompleteDate = myDate;
                    entityPM.IsOpen = false;
                    entityPM.ActivityStatusCode = "C";

                    this.SetTheLastActivityFields(entityPM);
                }

                this.InitializeData(entityPM);
                this.SetCustomerDateFields(entityPM, null);
                this.InitializeSortingFields(entityPM);
                this.InitializeActivityWithField(entityPM);
            }
        }

        private void AddCorrespondenceLine(ActivityPM entityPM, string isCompleted)
        {
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
                        meetingSummary = "\n" + "Metting Summary : " + entityPM.MeetingSummary ;
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
                };

                correspondenceRepository.Add(line);
                correspondenceRepository.SubmitChanges();
            }
        }

        protected override void OnUpdating(ActivityPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                this.InitializeData(entityPM);
            }
        }

        protected override void OnUpdating(ActivityPM entityPM, Activity entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(entityPM.Tenant), entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }

                if (entityPM.ActivityStatusCode == "C" && entityPOCO.ActivityStatusCode != "C")
                {
                    entityPM.CompleteDate = TenantServerConfigration.GetCurrentDateTime(EntityPM.Tenant);

                    this.SetTheLastActivityFields(entityPM);
                }

                else if (entityPM.ActivityStatusCode != "C" && entityPOCO.ActivityStatusCode == "C")
                {
                    entityPM.CompleteDate = null;
                    // Add Corresponding line for open Activity case
                    AddCorrespondenceLine(entityPM, "LineOpened");
                }

                this.InitializeSortingFields(entityPM);
                this.InitializeActivityWithField(entityPM);

                if (entityPM.OwnerId != entityPOCO.OwnerId)
                {
                    DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                    ActivityOwnerHistory activityOwnerHistory = new ActivityOwnerHistory()
                    {
                        Id = IdCounter.GetNumber("ActivityOwnerHistory", entityPM.Tenant),
                        ActivityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        OwnerId = entityPOCO.OwnerId,
                        ModifiedDate = todayDate,
                        NeedSynchronization = (entityPM.ActivityTypeCode != "CL" && entityPM.ActivityTypeCode != "EI" && entityPM.ActivityTypeCode != "EO") ? true : false,
                    };

                    ActivityOwnerHistoryRepository activityOwnerHistoryRepository = new ActivityOwnerHistoryRepository(entityPM.Tenant);
                    activityOwnerHistoryRepository.Add(activityOwnerHistory);
                    activityOwnerHistoryRepository.SubmitChanges();
                }

                if (entityPM.CustomerId != entityPOCO.CustomerId)
                {
                    this.SetCustomerDateFields(entityPM, entityPOCO);
                }
            }
        }

        protected override void Trace(ActivityPM entityPM, Activity entityPOCO, string changesXml)
        {
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            var resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

            CRMEmailAlertsHelper helper = new CRMEmailAlertsHelper();
            string activityTypeCode = null;
            switch (entityPM.ActivityTypeCode)
            {
                case "AP":
                    activityTypeCode = "A";
                    break;
                case "CL":
                    activityTypeCode = "P";
                    break;
                case "TS":
                    activityTypeCode = "T";
                    break;
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityPM.OwnerId != entityPOCO.OwnerId && entityPM.OwnerId != entityPM.UpdatedByUserId)
                {
                    if (activityTypeCode != null)
                    {
                        helper.SendEmailAlert(entityPM, entityPOCO, "Activity", entityPM.Tenant, "OAC" + activityTypeCode, true);
                    }
                }

                if (activityTypeCode != null)
                {
                    helper.SendEmailAlert(entityPM, entityPOCO, "Activity", entityPM.Tenant, "GAN" + activityTypeCode, true);
                    if (entityPM.ActivityStatusCode == "C")
                        helper.SendEmailAlert(entityPM, entityPOCO, "Activity", entityPM.Tenant, "GA" + activityTypeCode + "C", false);
                }

                if (entityPM.IsCopy)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "COAC",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Activity",
                        Notes = "Copied from: '" + entityPM.OriginalActivitySubject + "'",
                    });
                }

                if (string.IsNullOrEmpty(entityPM.ActivityTypeName))
                {
                    ActivityTypeRepository typeRepository = new ActivityTypeRepository(entityPM.Tenant);
                    ActivityType type = typeRepository.GetSingle(entityPM.ActivityTypeCode);
                    if (type != null)
                    {
                        entityPM.ActivityTypeName = type.Name;
                    }
                }

                this.CreateEntityEvent(contact.Id, entityPM, "CR");
            }

            else
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Activity",
                });

                if (entityPM.OwnerId != entityPOCO.OwnerId && entityPM.OwnerId != entityPM.UpdatedByUserId)
                {
                    if (activityTypeCode != null)
                    {
                        helper.SendEmailAlert(entityPM, entityPOCO, "Activity", entityPM.Tenant, "OAC" + activityTypeCode, false);
                    }
                }
                
                if (entityPM.ActivityStatusCode == "X" && entityPOCO.ActivityStatusCode != "X")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "CAAV",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Activity",
                    });
                }

                else if (entityPM.ActivityStatusCode == "C" && entityPOCO.ActivityStatusCode != "C")
                {
                    if (activityTypeCode != null)
                    {
                        helper.SendEmailAlert(entityPM, entityPOCO, "Activity", entityPM.Tenant, "GA" + activityTypeCode + "C", false);
                    }

                    this.CreateEntityEvent(contact.Id, entityPM, "CM");
                }

                if (entityPM.IsOpen && !entityPOCO.IsOpen)
                {
                    this.CreateEntityEvent(contact.Id, entityPM, "RP");
                }                
            }
        }

        private void CreateEntityEvent(string loggedUserId, ActivityPM entityPM, string eventTypeCode)
        {
            string myEventCode = null;
            string myEventEntityId = null;
            string myEventTableName = null;
            string myEventNotes = entityPM.ActivityTypeName + ": " + entityPM.Subject;

            switch (eventTypeCode)
            {
                case "CR": { myEventCode = "CRAV"; break; }
                case "CM": { myEventCode = "COAV"; break; }
                case "RP": { myEventCode = "ROAV"; break; }
            }

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = entityPM.Tenant,
                EventTypeCode = myEventCode,
                UserId = loggedUserId,
                EntityId = entityPM.Id,
                ObjectTableName = "Activity",
                Notes = myEventNotes,
            });

            if (!string.IsNullOrEmpty(entityPM.OpportunityId))
            {
                myEventEntityId = entityPM.OpportunityId;
                myEventTableName = "Opportunity";

                switch (eventTypeCode)
                {
                    case "CR": { myEventCode = "OACR"; break; }
                    case "CM": { myEventCode = "OACM"; break; }
                    case "RP": { myEventCode = "OARP"; break; }
                }
            }

            else if (!string.IsNullOrEmpty(entityPM.QuoteId))
            {
                myEventEntityId = entityPM.QuoteId;
                myEventTableName = "Quote";

                switch (eventTypeCode)
                {
                    case "CR": { myEventCode = "QACR"; break; }
                    case "CM": { myEventCode = "QACM"; break; }
                    case "RP": { myEventCode = "QARP"; break; }
                }
            }

            else if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                myEventEntityId = entityPM.CustomerId;
                myEventTableName = "Customer";

                switch (eventTypeCode)
                {
                    case "CR": { myEventCode = "CACR"; break; }
                    case "CM": { myEventCode = "CACM"; break; }
                    case "RP": { myEventCode = "CARP"; break; }
                }
            }

            if (!string.IsNullOrEmpty(myEventEntityId) && !string.IsNullOrEmpty(myEventTableName))
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = myEventCode,
                    UserId = loggedUserId,
                    EntityId = myEventEntityId,
                    ObjectTableName = myEventTableName,
                    Notes = myEventNotes,
                });
            }
        }

        private void InitializeData(ActivityPM entityPM)
        {
            this.InitializeBusinessUnit(entityPM);
            this.UpdateParentEntities(entityPM);

            if (entityPM.ActivityTypeCode == "AP")
            {
                entityPM.DueDate = entityPM.StartDateTime;
            }

            if (string.IsNullOrEmpty(entityPM.ActivityStatusCode))
            {
                entityPM.ActivityStatusCode = "N";
            }
            
            if (!entityPM.DontSetNeedSynchronization && entityPM.ActivityTypeCode != "CL" && entityPM.ActivityTypeCode != "EI" && entityPM.ActivityTypeCode != "EO")
            {
                entityPM.NeedSynchronization = true;

                string activity = null;
                switch (entityPM.ActivityTypeCode)
                {
                    case "TS":
                        activity = "Task- New to Outlook";
                        break;
                    case "AP":
                        activity = "Appointment - New to Outlook";
                        break;
                }

                if (!string.IsNullOrEmpty(activity))
                {
                    //ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    //Contact contact = contactRep.GetSingleContact(entityPM.CreatedByUserId, entityPM.Tenant);
                    //ActivityLogger.SendTotangoContactActivity(contact.Email, "OUTLOOK COONECTION", activity, entityPM.Tenant, false, null);
                }
            }

            if (entityPM.ActivityStatusCode == "X")
            {
                entityPM.OpportunityId = null;
                entityPM.OpportunitySubject = null;
            }

            entityPM.IsOpen = !(entityPM.ActivityStatusCode == "X" || entityPM.ActivityStatusCode == "C");

            this.SetTheNextActivityFields(entityPM);
        }

        private void UpdateParentEntities(ActivityPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.OpportunityId))
            {
                OpportunityRepository opportunityRepository = new OpportunityRepository(entityPM.Tenant);
                Opportunity opportunity = opportunityRepository.GetSingle(entityPM.OpportunityId, entityPM.Tenant);

                if (opportunity != null)
                {
                    opportunity.UpdateDate = entityPM.UpdateDate;
                    opportunity.UpdatedByUserId = entityPM.UpdatedByUserId;
                    opportunityRepository.Update(opportunity);
                    opportunityRepository.SubmitChanges();

                    entityPM.CustomerId = opportunity.CustomerId;
                }

                if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                {
                    if (entityPM.ActivityTypeCode == "EO" || entityPM.ActivityTypeCode == "EI")
                    {
                        var SendReceiveDate = entityPM.SendReceiveDate;
                        var by = entityPM.SenderEmail;
                        var RecordDate = entityPM.CreateDate;
                        var RecorderBy = entityPM.CreatedByUserName;
                        if (string.IsNullOrEmpty(RecorderBy))
                        {
                            if (entityPM.CreatedByUserId != null)
                            {
                                UserRepository userRepository = new UserRepository(entityPM.Tenant);
                                User user = userRepository.GetSingleUser(entityPM.CreatedByUserId, entityPM.Tenant);
                                if (user != null)
                                {
                                    RecorderBy = user.Contact.EnglishName;
                                }

                                else
                                {
                                    user = userRepository.GetSingleUser(entityPM.CreatedByUserId, 0);
                                    if (user != null)
                                    {
                                        RecorderBy = user.Contact.EnglishName;
                                    }
                                }
                            }
                        }

                        if (entityPM.ActivityTypeCode == "EO")
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "OPMO",
                                UserId = entityPM.CreatedByUserId,
                                EntityId = entityPM.OpportunityId,
                                ObjectTableName = "Opportunity",
                                Notes = "Sent date: " + SendReceiveDate + "\nby: " + by + "\nRecorder by: " + RecorderBy + "\nRecord date: " + RecordDate,
                            });
                        }

                        else if (entityPM.ActivityTypeCode == "EI")
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "OPMI",
                                UserId = entityPM.CreatedByUserId,
                                EntityId = entityPM.OpportunityId,
                                ObjectTableName = "Opportunity",
                                Notes = "Receive date: " + SendReceiveDate + "\nby: " + by + "\nRecorder by: " + RecorderBy + "\nRecord date: " + RecordDate,
                            });
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.QuoteId))
            {
                QuoteRepository quoteRepository = new QuoteRepository(entityPM.Tenant);
                Quote quote = quoteRepository.GetSingleQuote(entityPM.QuoteId, entityPM.Tenant);

                if (quote != null)
                {
                    quote.UpdateDate = entityPM.UpdateDate;
                    quote.UpdatedByUserId = entityPM.UpdatedByUserId;
                    quoteRepository.Update(quote);
                    quoteRepository.SubmitChanges();
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                Card card = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);

                if (card != null)
                {
                    card.UpdateDate = entityPM.UpdateDate;
                    card.UpdatedByUserId = entityPM.UpdatedByUserId;
                    cardRepository.Update(card);
                    cardRepository.SubmitChanges();
                }
            }

            if (!string.IsNullOrEmpty(entityPM.TicketId))
            {
                TicketRepository ticketRepository = new TicketRepository(entityPM.Tenant);
                Ticket ticket = ticketRepository.GetSingle(entityPM.TicketId, entityPM.Tenant);
                if (ticket != null)
                {
                    ticket.UpdateDate = entityPM.UpdateDate;
                    ticket.UpdatedByUserId = entityPM.UpdatedByUserId;
                    ticketRepository.Update(ticket);
                    ticketRepository.SubmitChanges();
                }
            }
        }

        private void SetTheLastActivityFields(ActivityPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.OpportunityId))
            {
                OpportunityRepository opportunityRepository = new OpportunityRepository(entityPM.Tenant);
                Opportunity opportunity = opportunityRepository.GetSingle(entityPM.OpportunityId, entityPM.Tenant);

                if (opportunity != null)
                {
                    opportunity.LastActivitySubject = entityPM.Subject;
                    opportunity.LastCompletedActivityTypeCode = entityPM.ActivityTypeCode;
                    opportunity.LastCompletedActivityDate = entityPM.CompleteDate;
                    opportunityRepository.Update(opportunity);
                    opportunityRepository.SubmitChanges();
                }
            }

            if (!string.IsNullOrEmpty(entityPM.QuoteId))
            {
                QuoteRepository quoteRepository = new QuoteRepository(entityPM.Tenant);
                Quote quote = quoteRepository.GetSingleQuote(entityPM.QuoteId, entityPM.Tenant);

                if (quote != null)
                {
                    quote.LastActivitySubject = entityPM.Subject;
                    quote.LastActivityTypeCode = entityPM.ActivityTypeCode;
                    quote.LastActivityDate = entityPM.CompleteDate;
                    quoteRepository.Update(quote);
                    quoteRepository.SubmitChanges();
                }
            }

            if (!string.IsNullOrEmpty(entityPM.TicketId))
            {
                TicketRepository ticketRepository = new TicketRepository(entityPM.Tenant);
                Ticket ticket = ticketRepository.GetSingle(entityPM.TicketId, entityPM.Tenant);
                if (ticket != null)
                {
                    ticket.LastCompletedActivitySubject = entityPM.Subject;
                    ticket.LastCompletedActivityTypeCode = entityPM.ActivityTypeCode;
                    ticket.LastCompletedActivityDate = entityPM.CompleteDate;
                    ticketRepository.Update(ticket);
                    ticketRepository.SubmitChanges();

                    // Add Correspondence Line as Activity Completed 

                    AddCorrespondenceLine(entityPM, "LineCompleted");
                }
            }
        }

        private void SetTheNextActivityFields(ActivityPM entityPM)
        {
            ActivityRepository activityRepository = new ActivityRepository(entityPM.Tenant);

            if (!string.IsNullOrEmpty(entityPM.OpportunityId))
            {
                string nextSubject = null;
                string nextTypeCode = null;
                DateTime? nextDateTime = null;

                if (entityPM.IsOpen && entityPM.StartDateTime != null)
                {
                    nextSubject = entityPM.Subject;
                    nextTypeCode = entityPM.ActivityTypeCode;
                    nextDateTime = entityPM.StartDateTime;
                }

                IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByOpportunityId(entityPM.OpportunityId, entityPM.Tenant);               
                iQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);

                if (!string.IsNullOrEmpty(entityPM.Id))
                {
                    iQueryableActivities = iQueryableActivities.Where(d => d.Id != entityPM.Id);
                }

                Activity nextDBActivity = iQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();

                if (nextDBActivity != null)
                {
                    if (nextDateTime == null)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }

                    else if (nextDBActivity.StartDateTime < nextDateTime)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }
                }

                OpportunityRepository opportunityRepository = new OpportunityRepository(entityPM.Tenant);
                Opportunity opportunity = opportunityRepository.GetSingle(entityPM.OpportunityId, entityPM.Tenant);
                
                if (opportunity != null)
                {
                    opportunity.NextActivitySubject = nextSubject;
                    opportunity.NextActivityTypeCode = nextTypeCode;
                    opportunity.NextActivityDate = nextDateTime;

                    opportunityRepository.Update(opportunity);
                    opportunityRepository.SubmitChanges();
                }
            }

            if (!string.IsNullOrEmpty(entityPM.QuoteId))
            {
                string nextSubject = null;
                string nextTypeCode = null;
                DateTime? nextDateTime = null;

                if (entityPM.IsOpen && entityPM.StartDateTime != null)
                {
                    nextSubject = entityPM.Subject;
                    nextTypeCode = entityPM.ActivityTypeCode;
                    nextDateTime = entityPM.StartDateTime;
                }

                IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByQuoteId(entityPM.QuoteId, entityPM.Tenant);
                iQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);

                if (!string.IsNullOrEmpty(entityPM.Id))
                {
                    iQueryableActivities = iQueryableActivities.Where(d => d.Id != entityPM.Id);
                }

                Activity nextDBActivity = iQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();

                if (nextDBActivity != null)
                {
                    if (nextDateTime == null)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }

                    else if (nextDBActivity.StartDateTime < nextDateTime)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }
                }

                QuoteRepository quoteRepository = new QuoteRepository(entityPM.Tenant);
                Quote quote = quoteRepository.GetSingleQuote(entityPM.QuoteId, entityPM.Tenant);

                if (quote != null)
                {
                    quote.NextActivitySubject = nextSubject;
                    quote.NextActivityTypeCode = nextTypeCode;
                    quote.NextActivityDate = nextDateTime;

                    quoteRepository.Update(quote);
                    quoteRepository.SubmitChanges();
                }
            }

            if (!string.IsNullOrEmpty(entityPM.TicketId))
            {
                string nextSubject = null;
                string nextTypeCode = null;
                DateTime? nextDateTime = null;

                if (entityPM.IsOpen && entityPM.StartDateTime != null)
                {
                    nextSubject = entityPM.Subject;
                    nextTypeCode = entityPM.ActivityTypeCode;
                    nextDateTime = entityPM.StartDateTime;
                }

                IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByTicketId(entityPM.TicketId, entityPM.Tenant);
                iQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);

                if (!string.IsNullOrEmpty(entityPM.Id))
                {
                    iQueryableActivities = iQueryableActivities.Where(d => d.Id != entityPM.Id);
                }

                Activity nextDBActivity = iQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();

                if (nextDBActivity != null)
                {
                    if (nextDateTime == null)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }

                    else if (nextDBActivity.StartDateTime < nextDateTime)
                    {
                        nextSubject = nextDBActivity.Subject;
                        nextTypeCode = nextDBActivity.ActivityTypeCode;
                        nextDateTime = nextDBActivity.StartDateTime;
                    }
                }

                TicketRepository ticketRepository = new TicketRepository(entityPM.Tenant);
                Ticket ticket = ticketRepository.GetSingle(entityPM.TicketId, entityPM.Tenant);

                if (ticket != null)
                {
                    ticket.NextActivitySubject = nextSubject;
                    ticket.NextActivityTypeCode = nextTypeCode;
                    ticket.NextActivityDate = nextDateTime;

                    ticketRepository.Update(ticket);
                    ticketRepository.SubmitChanges();
                }
            }
        }

        private void InitializeBusinessUnit(ActivityPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.OwnerId))
            {
                UserRepository userRepository = new UserRepository(entityPM.Tenant);
                User user = userRepository.GetSingleUser(entityPM.OwnerId, entityPM.Tenant, false);
                if (user != null)
                {
                    if (entityPM.BusinessUnitId != user.BusinessUnitId)
                    {
                        entityPM.BusinessUnitId = user.BusinessUnitId;
                    }
                }
            }
        }

        protected override void UpdateComposition(ActivityPM entityPM)
        {
            ActivityInviteeUpdateService inviteesUpdateService = new ActivityInviteeUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            inviteesUpdateService.UpdateMulti(entityPM.ActivityInvitees, entityPM.DeletedActivityInvitees, entityPM, false);

            ActivityEmailRecipientUpdateService emailRecipientUpdateService = new ActivityEmailRecipientUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            emailRecipientUpdateService.UpdateMulti(entityPM.ActivityEmailRecipients, entityPM.DeletedActivityEmailRecipients, entityPM, false);

            ActivityNoteUpdateService noteUpdateService = new ActivityNoteUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            noteUpdateService.UpdateMulti(entityPM.ActivityNotes, entityPM.DeletedActivityNotes, entityPM, false);
        }

        protected override void CheckConcurrency(ActivityPM entityPM, Activity entityPOCO)
        {
            if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            {
                if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
                {
                    string msg = CRMTranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                    throw new OptimisticConcurrencyException(msg);
                }
            }

            base.CheckConcurrency(entityPM, entityPOCO);
        }

        private void SetCustomerDateFields(ActivityPM entityPM, Activity entityPOCO)
        {
            if (entityPM.ActivityTypeCode == "CL" || entityPM.ActivityTypeCode == "AP")
            {
                int myTenant = entityPM.Tenant;
                DateTime myDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

                if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                {
                    if (!string.IsNullOrEmpty(entityPM.CustomerId))
                    {
                        CustomerRepository customerRepository = new CustomerRepository(myTenant);
                        Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, myTenant, false);

                        if (customer != null)
                        {
                            if (entityPM.ActivityTypeCode == "CL")
                            {
                                customer.LastCallDate = myDate;
                                customer.LastInteractionDate = myDate;
                                customerRepository.Update(customer);
                                customerRepository.SubmitChanges();
                            }

                            else if (entityPM.ActivityTypeCode == "AP")
                            {
                                customer.LastMeetingDate = myDate;
                                customer.LastInteractionDate = myDate;
                                customerRepository.Update(customer);
                                customerRepository.SubmitChanges();
                            }
                        }
                    }
                }

                else if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
                {
                    if (entityPM.CustomerId != entityPOCO.CustomerId)
                    {
                        CustomerRepository customerRepository = new CustomerRepository(myTenant);

                        if (!string.IsNullOrEmpty(entityPM.CustomerId))
                        {
                            Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, myTenant, false);

                            if (customer != null)
                            {
                                if (entityPM.ActivityTypeCode == "CL")
                                {
                                    customer.LastCallDate = myDate;
                                    customer.LastInteractionDate = myDate;
                                    customerRepository.Update(customer);
                                    customerRepository.SubmitChanges();
                                }

                                else if (entityPM.ActivityTypeCode == "AP")
                                {
                                    customer.LastMeetingDate = myDate;
                                    customer.LastInteractionDate = myDate;
                                    customerRepository.Update(customer);
                                    customerRepository.SubmitChanges();
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(entityPOCO.CustomerId))
                        {
                            ActivityRepository myActivityRepository = new ActivityRepository(myTenant);
                            IQueryable<Activity> oldCustomerEntities = myActivityRepository.GetActivitiesByCustomerId(entityPOCO.CustomerId, myTenant);

                            if (oldCustomerEntities != null)
                            {
                                if (oldCustomerEntities.Count() > 0)
                                {
                                    DateTime? oldestDate = oldCustomerEntities.OrderByDescending(d => d.CreateDate).FirstOrDefault().CreateDate;
                                    if (oldestDate != null)
                                    {
                                        Customer customer = customerRepository.GetSingleCustomer(entityPOCO.CustomerId, myTenant, false);
                                        if (customer != null)
                                        {
                                            if (entityPM.ActivityTypeCode == "CL")
                                            {
                                                customer.LastCallDate = oldestDate;
                                            }

                                            else if (entityPM.ActivityTypeCode == "AP")
                                            {
                                                customer.LastMeetingDate = oldestDate;
                                            }

                                            customer.LastInteractionDate = customerRepository.ComputeLastInteractionDate(customer, oldestDate);

                                            customerRepository.Update(customer);
                                            customerRepository.SubmitChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
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
                case "EO":
                    {
                        entityPM.SortingBy = "Send/Receive Date";

                        if (entityPM.SendReceiveDate != null)
                        {
                            entityPM.SortingDate = entityPM.SendReceiveDate;
                        }
                        break;
                    }
            }
        }

        private void InitializeActivityWithField(ActivityPM entityPM)
        {
            switch (entityPM.ActivityTypeCode)
            {
                case "TS":
                    {
                        if (!string.IsNullOrEmpty(entityPM.OwnerId))
                        {
                            UserRepository rep = new UserRepository(entityPM.Tenant);
                            User owner = rep.GetSingleUser(entityPM.OwnerId, entityPM.Tenant);

                            if (owner != null)
                            {
                                entityPM.ActivityWith = owner.Contact.EnglishName;
                            }
                        }

                        break;
                    }

                    case "CL":
                    {
                       
                        if(!string.IsNullOrEmpty(entityPM.CallWithId))
                        {
                            ContactRepository rep = new ContactRepository(entityPM.Tenant);
                            Contact callWith = rep.GetSingleContact(entityPM.CallWithId, entityPM.Tenant);

                            if (callWith != null)
                            {
                                entityPM.ActivityWith = callWith.EnglishName;
                            }
                        }

                        break;
                    }

                case "AP":
                    {
                        if (entityPM.ActivityInvitees != null)
                        {
                            string str = "";
                            foreach (ActivityInviteePM item in entityPM.ActivityInvitees)
                            {
                                if (item.IsRequired)
                                {
                                    if (string.IsNullOrEmpty(str))
                                    {
                                        str = item.ContactName;
                                    }
                                    else
                                    {
                                        str = str + ", " + item.ContactName;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(str))
                            {
                                entityPM.ActivityWith = str;
                            }
                        }

                        break;
                    }                  

                case "EO":
                case "EI":
                    {
                        if (entityPM.ActivityEmailRecipients != null)
                        {
                            string str = "";
                            ContactRepository rep = new ContactRepository(entityPM.Tenant);
                            foreach (ActivityEmailRecipientPM item in entityPM.ActivityEmailRecipients)
                            {
                                if (!string.IsNullOrEmpty(item.Email) && !string.IsNullOrEmpty(entityPM.To) && !entityPM.To.Contains(item.Email) && item.RecipientTypeCode == "TO")
                                {
                                    entityPM.To += item.Email + ";";
                                }

                                if (!string.IsNullOrEmpty(item.Email) && !string.IsNullOrEmpty(entityPM.Cc) && !entityPM.Cc.Contains(item.Email) && item.RecipientTypeCode == "CC")
                                {
                                    entityPM.Cc += item.Email + ";";
                                }

                                Contact contact = rep.GetSingleContact(item.ContactId, entityPM.Tenant);

                                if (contact != null)
                                {
                                    if (string.IsNullOrEmpty(str))
                                    {
                                        str = contact.EnglishName;
                                    }
                                    else
                                    {
                                        str = str + ", " + contact.EnglishName;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(str))
                            {
                                entityPM.ActivityWith = str;
                            }
                        }

                        break;
                    }    
            }
        }
    }
}
