using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EmailAlerts;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;

namespace Logitude.BL.QuoteModel.Tools.TraceEvents
{
    public partial class QuoteTracing
    {
        public static void Trace(QuotePM entityPM, Quote entityPoco, string loggedContactId, bool isNewEntity)
        {
            int tenant = entityPM.Tenant;
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode("Quote", entityPM.Tenant);
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            QuoteStageRepository myQuoteStageRepository = new QuoteStageRepository(tenant);
            IQueryable<QuoteStage> myStages = myQuoteStageRepository.GetQuoteStages(tenant);

            QuoteEmailAlert quoteEmailAlert = new QuoteEmailAlert();

            if (isNewEntity)
            {
                if (entityPM.SalesmanUserId != entityPM.UpdatedByUserId)
                {
                    quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "OQTA", true);
                }

                quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "GNQT", true);
              
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "CRQT",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Quote",
                });
               
                if (entityPM.IsCopy)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "CFAQ",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Quote",
                        Notes = "Copied from Quote number: " + entityPM.BaseShipmentNumber,
                    });
                }

                if (!string.IsNullOrEmpty(entityPM.OpportunityId))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QTOP",
                        UserId = loggedContactId,
                        EntityId = entityPM.OpportunityId,
                        ObjectTableName = "Opportunity",
                        Notes = "Quote: " + entityPM.QuoteNumber + " Added",
                    });
                }
            }

            else
            {
                if (entityPM.SalesmanUserId != entityPoco.SalesmanUserId && entityPM.SalesmanUserId != entityPM.UpdatedByUserId)
                {
                    quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "OQTA", false);
                }

                if (!entityPM.MarkFollowUpsAsDone)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "UPQT",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Quote",
                    });
                }

                if (!string.IsNullOrEmpty(entityPM.OpportunityId) && string.IsNullOrEmpty(entityPoco.OpportunityId))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QTOP",
                        UserId = loggedContactId,
                        EntityId = entityPM.OpportunityId,
                        ObjectTableName = "Opportunity",
                        Notes = "Quote: " + entityPM.QuoteNumber + " Added",
                    });
                }

                if (string.IsNullOrEmpty(entityPM.OpportunityId) && !string.IsNullOrEmpty(entityPoco.OpportunityId))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QTOP",
                        UserId = loggedContactId,
                        EntityId = entityPM.OpportunityId,
                        ObjectTableName = "Opportunity",
                        Notes = "Quote: " + entityPM.QuoteNumber + " Deleted",
                    });
                }

                if (entityPM.StageDueDate != entityPoco.StageDueDate)
                {
                    string myEventNotes = "";
                    myEventNotes += "Previous stage due date: " + (entityPoco.StageDueDate == null ? "" : entityPoco.StageDueDate.Value.ToShortDateString());
                    myEventNotes += "\n";
                    myEventNotes += "New stage due date: " + (entityPM.StageDueDate == null ? "" : entityPM.StageDueDate.Value.ToShortDateString());
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QUSG",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Quote",
                        Notes = myEventNotes,
                    });
                }
            }



            if (entityPM.ActionType == "SentToCustomerFromQuotation")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "SASC",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Quote",
                    Notes = entityPM.EventNote,
                });

                entityPM.SentDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                QuoteStage myCreateStage = myStages.Where(d => d.Code == "QTCR").FirstOrDefault();
                QuoteStage myDraftStage = myStages.Where(d => d.Code == "QTDR").FirstOrDefault();

                QuoteStage myStage = myStages.Where(d => d.Code == "QTST").FirstOrDefault();
                if (myStage != null)
                {
                    if (entityPM.StageId == myCreateStage.Id || entityPM.StageId == myDraftStage.Id)
                    {
                        entityPM.StageId = myStage.Id;
                        entityPM.StageName = myStage.Name;
                        entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                        if (myStage.MaxDays != null)
                        {
                            entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                        }
                    }

                }
            }

            if (entityPM.ActionType == "SetAsSentToCustomer")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "SASC",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Quote",
                    Notes= entityPM.EventNote,
                });

                entityPM.SentDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                QuoteStage myStage = myStages.Where(d => d.Code == "QTST").FirstOrDefault();
                if (myStage != null)
                {
                    entityPM.StageId = myStage.Id;
                    entityPM.StageName = myStage.Name;
                    entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                    if (myStage.MaxDays != null)
                    {
                        entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                    }
                }
            }

            if (entityPM.ActionType == "ReturnInProgress")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "RQTD",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Quote",
                    Notes = entityPM.EventNote,
                });

                QuoteStage myStage = myStages.Where(d => d.Code == "QTDR").FirstOrDefault();
                if (myStage != null)
                {
                    entityPM.StageId = myStage.Id;
                    entityPM.StageName = myStage.Name;
                    entityPM.AcceptedDate = null;
                    entityPM.DeclinedDate = null;
                    entityPM.IsClosed = false;
                    entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                    if (myStage.MaxDays != null)
                    {
                        entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                    }
                }

                entityPM.QuoteClosingReasonCode = null;
                entityPM.IsAutomaticallyClosed = false;
                entityPM.AutomaticallyCloseDate = null;
                entityPM.AutomaticallyCloseDays = null;
            }

            if (!entityPoco.IsClosed && entityPM.IsClosed)
            {
                if (entityPM.ActionType == "Accept")
                {
                    string traceEventNotes = entityPM.EventNote;
                    if (!string.IsNullOrEmpty(entityPM.QuoteClosingReasonCode))
                    {
                        QuoteClosingReasonRepository closingReasonRepository = new QuoteClosingReasonRepository(tenant);
                        QuoteClosingReason myQuoteClosingReason = closingReasonRepository.GetSingleQuoteClosingReason(entityPM.QuoteClosingReasonCode);
                        if (myQuoteClosingReason != null)
                        {
                            traceEventNotes = myQuoteClosingReason.Name;
                        }
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QTCP",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Quote",
                        Notes = traceEventNotes,
                    });

                    QuoteStage myStage = myStages.Where(d => d.Code == "QTAC").FirstOrDefault();
                    if (myStage != null)
                    {
                        entityPM.StageId = myStage.Id;
                        entityPM.StageName = myStage.Name;
                        entityPM.AcceptedDate = todayDate;
                        entityPM.IsClosed = true;
                        entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                        if (myStage.MaxDays != null)
                        {
                            entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                        }

                        quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "GQTA", false);
                    }
                }

                else if (entityPM.ActionType == "Decline")
                {
                    string traceEventNotes = entityPM.EventNote;
                    if (!string.IsNullOrEmpty(entityPM.QuoteClosingReasonCode))
                    {
                        QuoteClosingReasonRepository closingReasonRepository = new QuoteClosingReasonRepository(tenant);
                        QuoteClosingReason myQuoteClosingReason = closingReasonRepository.GetSingleQuoteClosingReason(entityPM.QuoteClosingReasonCode);
                        if (myQuoteClosingReason != null)
                        {
                            traceEventNotes = myQuoteClosingReason.Name;
                        }
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QTDL",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Quote",
                        Notes = traceEventNotes,
                    });

                    QuoteStage myStage = myStages.Where(d => d.Code == "QTDC").FirstOrDefault();
                    if (myStage != null)
                    {
                        entityPM.StageId = myStage.Id;
                        entityPM.StageName = myStage.Name;
                        entityPM.DeclinedDate = todayDate;
                        entityPM.IsClosed = true;
                        entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                        if (myStage.MaxDays != null)
                        {
                            entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                        }

                        quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "GQTD", false);
                    }
                }
            }

            if (entityPoco.IsCancelled && !entityPM.IsCancelled)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "RAQT",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Quote",
                    Notes = entityPM.EventNote,
                });
            }

            if (!entityPoco.IsCancelled && entityPM.IsCancelled)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "CLQT",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Quote",
                    Notes = entityPM.EventNote,
                });
            }

            if (entityPM.ConvertToLCL)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "oLCL",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Quote",
                    Notes = entityPM.EventNote,
                });
            }

            if (entityPM.ConvertToFCL)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "oFCL",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Quote",
                    Notes = entityPM.EventNote,
                });
            }
        }

        public static void DeleteQuoteTraceEvent(QuotePM quotePM, string traceEventId, int tenant, bool external)
        {
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            traceEvent.Deleted = true;
            traceEventRep.Update(traceEvent);
            traceEventRep.SubmitChanges();
        }
    }
}