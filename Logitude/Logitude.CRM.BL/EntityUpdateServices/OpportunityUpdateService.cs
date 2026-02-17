using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.BL.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Social.BL.Helpers;
using Logitude.CRM.Data;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using System.Data;
using Logitude.CRM.BL.Validators;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Azure;
using Logitude.CRM.BL.Helpers;
using System.Web;
using System.Data.Entity.Core;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class OpportunityUpdateService
    {
        protected override void OnCreating(EntityPMs.OpportunityPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                StageRepository stageRepository = new StageRepository(entityPM.Tenant);

                entityPM.Id = IdCounter.GetNumber("Opportunity", entityPM.Tenant);

                DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                entityPM.CreateDate = todayDateTime;
                entityPM.UpdateDate = todayDateTime;
                entityPM.LastStageDate = todayDateTime;

                if (string.IsNullOrEmpty(entityPM.RatingCode))
                {
                    entityPM.RatingCode = "N";
                }

                if (string.IsNullOrEmpty(entityPM.StageId))
                {
                    if (entityPM.StageDueDate == null)
                    {
                        DateTime? stageDueDate = null;                        
                        Stage stage = stageRepository.GetStageByCode("QUA", entityPM.Tenant);
                        if (stage != null)
                        {
                            entityPM.Probability = stage.Probability;

                            if (stage.MaxDays != null)
                            {
                                stageDueDate = todayDateTime.Value.Date.AddDays(Convert.ToDouble(stage.MaxDays));
                            }
                        }

                        entityPM.StageDueDate = stageDueDate;
                    }
                }

                else
                {
                    Stage stage = stageRepository.GetSingle(entityPM.StageId, entityPM.Tenant);
                    if (stage != null)
                    {
                        entityPM.Probability = stage.Probability;
                    }
                }
                
                decimal? field1 = entityPM.Probability == null ? 0 : Convert.ToDecimal(entityPM.Probability);
                decimal? field2 = entityPM.NumberOfShipments == null ? 0 : Convert.ToDecimal(entityPM.NumberOfShipments);
                entityPM.ValueField = field1 * field2 / 100;

                this.SetNextActivityData(entityPM);
                this.InitializeBusinessUnit(entityPM);
                this.SetCustomerDateFields(entityPM, null);

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Opportunity", 0, true);
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                if (loggedContact != null)
                {
                    ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", loggedContact.Id);
                       }
            }
        }

        protected override void OnUpdating(EntityPMs.OpportunityPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                decimal? field1 = entityPM.Probability == null ? 0 : Convert.ToDecimal(entityPM.Probability);
                decimal? field2 = entityPM.NumberOfShipments == null ? 0 : Convert.ToDecimal(entityPM.NumberOfShipments);
                entityPM.ValueField = field1 * field2 / 100;

                this.SetNextActivityData(entityPM);
                this.InitializeBusinessUnit(entityPM);

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Opportunity", 0, true);
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                if (loggedContact != null)
                {
                    ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);                   
                   entityPM.UpdatedByUserId = loggedContact.Id;                   
                }

            }
        }

        protected override void OnUpdating(EntityPMs.OpportunityPM entityPM, Opportunity entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {               
                if (entityPM.StageId != entityPOCO.StageId)
                {
                    DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                    if (entityPM.LastStageDate == null)
                    {
                        entityPM.LastStageDate = entityPM.CreateDate;
                    }                    

                    OpportunityStage opportunityStage = new OpportunityStage()
                    {
                        Id = IdCounter.GetNumber("OpportunityStage", entityPM.Tenant),
                        OpportunityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        FromStageId = entityPOCO.StageId,
                        ToStageId = entityPM.StageId,
                        StartDate = entityPM.LastStageDate,
                        EndDate = todayDateTime,
                    };

                    entityPM.LastStageDate = todayDateTime;

                    OpportunityStageRepository opportunityStageRepository = new OpportunityStageRepository(entityPM.Tenant);
                    opportunityStageRepository.Add(opportunityStage);
                    opportunityStageRepository.SubmitChanges();
                }

                if (entityPM.CustomerId != entityPOCO.CustomerId)
                {
                    ActivityRepository activityRepository = new ActivityRepository(entityPM.Tenant);
                    List<Activity> activities = activityRepository.GetActivitiesByOpportunityId(entityPM.Id, entityPM.Tenant).ToList();

                    foreach (Activity item in activities)
                    {
                        item.CustomerId = entityPM.CustomerId;
                        activityRepository.Update(item);
                    }

                    activityRepository.SubmitChanges();
                    
                }

                this.SetCustomerDateFields(entityPM, entityPOCO);
               
            }
        }

        protected override void UpdateComposition(OpportunityPM entityPM)
        {
            OpportunityProductUpdateService productUpdateService = new OpportunityProductUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            productUpdateService.UpdateMulti(entityPM.OpportunityProducts, entityPM.DeletedOpportunityProducts, entityPM, false);

            OpportunityCompetitorUpdateService competitorUpdateService = new OpportunityCompetitorUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            competitorUpdateService.UpdateMulti(entityPM.OpportunityCompetitors, entityPM.DeletedOpportunityCompetitors, entityPM, false);

            OpportunityAdditionalServiceUpdateService additionalServiceUpdateService = new OpportunityAdditionalServiceUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            additionalServiceUpdateService.UpdateMulti(entityPM.OpportunityAdditionalServices, entityPM.DeletedOpportunityAdditionalServices, entityPM, false);
        }

        protected override void Trace(OpportunityPM entityPM, Opportunity entityPOCO, string changesXml)
        {            
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
            
            ICRMContext crmContext = this.MainContext as ICRMContext;
            CRMEmailAlertsHelper helper = new CRMEmailAlertsHelper();
            
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                if (entityPM.Subject != entityPOCO.Subject)
                {
                    AutomaticPosting.CreatePost(entityPM.Id, "Opportunity", contact.Id, entityPM.Subject, "changed Opportunity Subject from " + entityPOCO.Subject + " to " + entityPM.Subject, true, entityPM.Tenant);
                }

                if (entityPM.StageDueDate != entityPOCO.StageDueDate)
                {
                    string myEventNotes = "";
                    myEventNotes += "Previous stage due date: " + (entityPOCO.StageDueDate == null ? "" : entityPOCO.StageDueDate.Value.ToShortDateString());
                    myEventNotes += "\n";
                    myEventNotes += "New stage due date: " + (entityPM.StageDueDate == null ? "" : entityPM.StageDueDate.Value.ToShortDateString());

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "OPSG",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Opportunity",
                        Notes = myEventNotes,
                    });
                }

                if (entityPM.StageId != entityPOCO.StageId)
                {
                    StageQueryService stageQueryService = new StageQueryService(crmContext);
                    StagePM oldStage = stageQueryService.GetSingle(entityPOCO.StageId, false, false);
                    StagePM newStage = stageQueryService.GetSingle(entityPM.StageId, false, false);

                    AutomaticPosting.CreatePost(entityPM.Id, "Opportunity", contact.Id, entityPM.Subject, "changed Opportunity Stage from " + oldStage.Name + " to " + newStage.Name, true, entityPM.Tenant);

                    if (entityPM.PostToFollowersAsWon)
                    {
                        string message = (newStage.Code == "CLS") ? "Opportunity closed as lost" : "Opportunity closed as won";
                        message += ":" + Environment.NewLine + entityPM.ClosingDescription;
                        entityPM.PostToFollowersAsWon = false;
                        AutomaticPosting.CreatePost(entityPM.Id, "Opportunity", contact.Id, entityPM.Subject, message, false, entityPM.Tenant);
                    }

                    if (entityPM.OwnerId != entityPM.UpdatedByUserId)
                    {
                        helper.SendEmailAlert(entityPM, entityPOCO, "Opportunity", entityPM.Tenant, "OOPS", false);
                    }

                    helper.SendEmailAlert(entityPM, entityPOCO, "Opportunity", entityPM.Tenant, "GOPS", false);

                    if (newStage.Code == "CLS" || newStage.Code == "CLC")
                    {
                        helper.SendEmailAlert(entityPM, entityPOCO, "Opportunity", entityPM.Tenant, "GOCL", false);
                    }

                    else if (newStage.Code == "CWN")
                    {
                        helper.SendEmailAlert(entityPM, entityPOCO, "Opportunity", entityPM.Tenant, "GOCW", false);
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "OPST",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Opportunity",
                        Notes = "Stage changed from " + oldStage.Name + " to " + newStage.Name
                    });
                }

                if (entityPM.OwnerId != entityPOCO.OwnerId && entityPM.OwnerId != entityPM.UpdatedByUserId)
                {
                    helper.SendEmailAlert(entityPM, entityPOCO, "Opportunity", entityPM.Tenant, "OOPA", false);
                }                

                if (entityPM.RatingCode != entityPOCO.RatingCode)
                {
                    RatingQueryService ratingService = new RatingQueryService(crmContext);
                    RatingPM oldRating = ratingService.GetSingle(entityPOCO.RatingCode, false, false);
                    RatingPM newRating = ratingService.GetSingle(entityPM.RatingCode, false, false);
                    AutomaticPosting.CreatePost(entityPM.Id, "Opportunity", contact.Id, entityPM.Subject, "changed Opportunity Rating from " + oldRating.Name + " to " + newRating.Name, true, entityPM.Tenant);
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityPM.OwnerId != entityPM.UpdatedByUserId)
                {
                    helper.SendEmailAlert(entityPM, entityPOCO, "Opportunity", entityPM.Tenant,  "OOPA", true);                   
                }

                helper.SendEmailAlert(entityPM, entityPOCO, "Opportunity", entityPM.Tenant, "GNOP", true);

                CardRepository cardRepository = new CardRepository(commonContext);
                Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                AutomaticPosting.CreatePost(entityPM.CustomerId, "Customer", contact.Id, customer.EnglishName, "Opportunity of Subject (" + entityPM.Subject + ") has been created for this customer", true, entityPM.Tenant);

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CROP",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Opportunity",
                    Notes = changesXml
                });

                this.TraceProducts(entityPM, contact.Id, true);
                this.TraceCompetitors(entityPM, contact.Id, true);
                this.TraceAdditionalServices(entityPM, contact.Id, true);
            }

            else
            {                
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPOP",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Opportunity",
                    Notes = changesXml
                });

                if (entityPM.ClosingReasonId != entityPOCO.ClosingReasonId && entityPM.ClosingReasonId != null)
                {
                    OpportunityClosingReasonRepository closingRep = new OpportunityClosingReasonRepository(crmContext);
                    OpportunityClosingReason reason = closingRep.GetSingle(entityPM.ClosingReasonId, entityPM.Tenant);
                    if (reason != null)
                    {
                        string myReasonCode = reason.Code;
                        string myReasonName = reason.Name;
                        string myResounNotes = myReasonName;
                        string myClosingDescription = "";

                        if (!string.IsNullOrEmpty(entityPM.ClosingDescription))
                        {
                            myResounNotes += " - " + entityPM.ClosingDescription;
                            myClosingDescription = " - " + entityPM.ClosingDescription;
                        }

                        if (myReasonCode == "WN")
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "CWOP",
                                UserId = contact.Id,
                                EntityId = entityPM.Id,
                                ObjectTableName = "Opportunity",
                                Notes = myResounNotes,
                                EventDateTime = entityPM.ActualClosingDate,
                            });
                        }

                        else if (myReasonCode == "LO")
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "CLOP",
                                UserId = contact.Id,
                                EntityId = entityPM.Id,
                                ObjectTableName = "Opportunity",
                                Notes = myResounNotes,
                                EventDateTime = entityPM.ActualClosingDate,
                            });
                        }

                        else if (myReasonCode == "LC")
                        {
                            CompetitorRepository rep = new CompetitorRepository(entityPM.Tenant);
                            Competitor comp = rep.GetSingleCompetitor(entityPM.ClosedToCompetitorId, entityPM.Tenant);

                            if (comp != null)
                            {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = entityPM.Tenant,
                                    EventTypeCode = "CCOP",
                                    UserId = contact.Id,
                                    EntityId = entityPM.Id,
                                    ObjectTableName = "Opportunity",
                                    Notes = "Lost To " + comp.Name + myClosingDescription,
                                    EventDateTime = entityPM.ActualClosingDate,
                                });
                            }

                            else
                            {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = entityPM.Tenant,
                                    EventTypeCode = "CCOP",
                                    UserId = contact.Id,
                                    EntityId = entityPM.Id,
                                    ObjectTableName = "Opportunity",
                                    Notes = myResounNotes,
                                    EventDateTime = entityPM.ActualClosingDate,
                                });
                            }
                        }

                        else if (myReasonCode == "CA")
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "CAOP",
                                UserId = contact.Id,
                                EntityId = entityPM.Id,
                                ObjectTableName = "Opportunity",
                                Notes = myResounNotes,
                            });
                        }
                    }
                }

                if (entityPM.IsCopy)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "COOP",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Opportunity",
                    });
                }

                if (string.IsNullOrEmpty(entityPM.ClosingReasonId) && !string.IsNullOrEmpty(entityPOCO.ClosingReasonId))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ROOP",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Opportunity",
                    });
                }

                this.TraceProducts(entityPM, contact.Id, false);
                this.TraceCompetitors(entityPM, contact.Id, false);
                this.TraceAdditionalServices(entityPM, contact.Id, false);
            }            
        }

        private void TraceProducts(OpportunityPM entityPM, string loggedContactId, bool isNewEntity)
        {
            List<string> myAddedItemsTexts = new List<string>();
            List<string> myUpdatedItemsTexts = new List<string>();
            List<string> myDeletedItemsTexts = new List<string>();

            foreach (OpportunityProductPM itemPM in entityPM.DeletedOpportunityProducts)
            {
                myDeletedItemsTexts.Add(itemPM.OpportunityProductTypeName);
            }

            foreach (OpportunityProductPM itemPM in entityPM.OpportunityProducts)
            {
                if (isNewEntity)
                {
                    myAddedItemsTexts.Add(itemPM.OpportunityProductTypeName);
                }

                else
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                myAddedItemsTexts.Add(itemPM.OpportunityProductTypeName);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                myUpdatedItemsTexts.Add(itemPM.OpportunityProductTypeName);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                myDeletedItemsTexts.Add(itemPM.OpportunityProductTypeName);
                                break;
                            }
                    }
                }
            }

            if (myAddedItemsTexts.Count + myUpdatedItemsTexts.Count + myDeletedItemsTexts.Count > 0)
            {
                this.CreateEntityEvent("PROP", "Opportunity", entityPM.Id, loggedContactId, entityPM.Tenant, myAddedItemsTexts, myUpdatedItemsTexts, myDeletedItemsTexts);
            }
        }

        private void TraceCompetitors(OpportunityPM entityPM, string loggedContactId, bool isNewEntity)
        {
            List<string> myAddedItemsTexts = new List<string>();
            List<string> myUpdatedItemsTexts = new List<string>();
            List<string> myDeletedItemsTexts = new List<string>();

            foreach (OpportunityCompetitorPM itemPM in entityPM.DeletedOpportunityCompetitors)
            {
                myDeletedItemsTexts.Add(itemPM.EnglishName);
            }

            foreach (OpportunityCompetitorPM itemPM in entityPM.OpportunityCompetitors)
            {
                if (isNewEntity)
                {
                    myAddedItemsTexts.Add(itemPM.EnglishName);
                }

                else
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                myAddedItemsTexts.Add(itemPM.EnglishName);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                myUpdatedItemsTexts.Add(itemPM.EnglishName);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                myDeletedItemsTexts.Add(itemPM.EnglishName);
                                break;
                            }
                    }
                }
            }

            if (myAddedItemsTexts.Count + myUpdatedItemsTexts.Count + myDeletedItemsTexts.Count > 0)
            {
                this.CreateEntityEvent("CMOP", "Opportunity", entityPM.Id, loggedContactId, entityPM.Tenant, myAddedItemsTexts, myUpdatedItemsTexts, myDeletedItemsTexts);
            }
        }

        private void TraceAdditionalServices(OpportunityPM entityPM, string loggedContactId, bool isNewEntity)
        {
            List<string> myAddedItemsTexts = new List<string>();
            List<string> myUpdatedItemsTexts = new List<string>();
            List<string> myDeletedItemsTexts = new List<string>();

            foreach (OpportunityAdditionalServicePM itemPM in entityPM.DeletedOpportunityAdditionalServices)
            {
                myDeletedItemsTexts.Add(itemPM.EnglishName);
            }

            foreach (OpportunityAdditionalServicePM itemPM in entityPM.OpportunityAdditionalServices)
            {
                if (isNewEntity)
                {
                    myAddedItemsTexts.Add(itemPM.EnglishName);
                }

                else
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                myAddedItemsTexts.Add(itemPM.EnglishName);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                myUpdatedItemsTexts.Add(itemPM.EnglishName);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                myDeletedItemsTexts.Add(itemPM.EnglishName);
                                break;
                            }
                    }
                }
            }

            if (myAddedItemsTexts.Count + myUpdatedItemsTexts.Count + myDeletedItemsTexts.Count > 0)
            {
                this.CreateEntityEvent("ADOP", "Opportunity", entityPM.Id, loggedContactId, entityPM.Tenant, myAddedItemsTexts, myUpdatedItemsTexts, myDeletedItemsTexts);
            }
        }

        private void CreateEntityEvent(string myEventCode, string myTableName, string myEntityId, string myLoggedContactId, int myTenant, List<string> myAddedItemsTexts, List<string> myUpdatedItemsTexts, List<string> myDeletedItemsTexts)
        {
            string myEventNotes = null;

            if (myAddedItemsTexts.Count > 0)
            {
                if (!string.IsNullOrEmpty(myEventNotes))
                {
                    myEventNotes += "\n";
                }

                myEventNotes += "Added:";

                foreach (string item in myAddedItemsTexts)
                {
                    myEventNotes += "\n" + item;
                }
            }

            if (myUpdatedItemsTexts.Count > 0)
            {
                if (!string.IsNullOrEmpty(myEventNotes))
                {
                    myEventNotes += "\n";
                }

                myEventNotes += "Updated:";

                foreach (string item in myUpdatedItemsTexts)
                {
                    myEventNotes += "\n" + item;
                }
            }

            if (myDeletedItemsTexts.Count > 0)
            {
                if (!string.IsNullOrEmpty(myEventNotes))
                {
                    myEventNotes += "\n";
                }

                myEventNotes += "Deleted:";

                foreach (string item in myDeletedItemsTexts)
                {
                    myEventNotes += "\n" + item;
                }
            }

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = myTenant,
                EventTypeCode = myEventCode,
                UserId = myLoggedContactId,
                EntityId = myEntityId,
                ObjectTableName = myTableName,
                Notes = myEventNotes,
            });
        }

        private void SetCustomerDateFields(OpportunityPM entityPM, Opportunity entityPOCO)
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
                        customer.LastOpportunityDate = myDate;
                        customer.LastInteractionDate = myDate;
                        customerRepository.Update(customer);
                        customerRepository.SubmitChanges();
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
                            customer.LastOpportunityDate = myDate;
                            customer.LastInteractionDate = myDate;
                            customerRepository.Update(customer);
                            customerRepository.SubmitChanges();
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.CustomerId))
                    {
                        OpportunityRepository myOpportunityRepository = new OpportunityRepository(myTenant);
                        IQueryable<Opportunity> oldCustomerEntities = myOpportunityRepository.GetOppListByCustomerIdAndTenant(entityPOCO.CustomerId, myTenant);

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
                                        customer.LastOpportunityDate = oldestDate;

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

        private void SetNextActivityData(OpportunityPM entityPM)
        {
            string nextSubject = null;
            string nextTypeCode = null;
            DateTime? nextDateTime = null;

            ActivityRepository activityRepository = new ActivityRepository(entityPM.Tenant);
            IQueryable<Activity> iQueryableActivities = activityRepository.GetActivitiesByOpportunityId(entityPM.Id, entityPM.Tenant);
            iQueryableActivities = iQueryableActivities.Where(d => d.StartDateTime != null && d.IsOpen);
            Activity nextDBActivity = iQueryableActivities.OrderBy(d => d.StartDateTime).FirstOrDefault();
            
            if (nextDBActivity != null)
            {
                nextSubject = nextDBActivity.Subject;
                nextTypeCode = nextDBActivity.ActivityTypeCode;
                nextDateTime = nextDBActivity.StartDateTime;
            }

            entityPM.NextActivitySubject = nextSubject;
            entityPM.NextActivityTypeCode = nextTypeCode;
            entityPM.NextActivityDate = nextDateTime; 
        }

        private void InitializeBusinessUnit(OpportunityPM entityPM)
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

        protected override void CheckConcurrency(OpportunityPM entityPM, Opportunity entityPOCO)
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
    }
}
