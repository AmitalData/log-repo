using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.WorkRoles;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class TicketUpdateService
    {
        protected override void OnCreating(EntityPMs.TicketPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (string.IsNullOrEmpty(entityPM.Id))
                {
                    entityPM.Id = IdCounter.GetNumber("Ticket", entityPM.Tenant);
                }

                if (string.IsNullOrEmpty(entityPM.TicketNumber))
                {
                    entityPM.TicketNumber = CodeCounter.GetNumber("Ticket", entityPM.Tenant).ToString();
                }

                TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
                Tenant tenantPoco = tenantRepository.GetSingleByTenant(entityPM.Tenant);
                string id = tenantPoco.DefaultSLAId;
                entityPM.SLAId = id;

                DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.CreateDate = myDate;
                entityPM.UpdateDate = myDate;
                entityPM.OpenDate = myDate;
                entityPM.EntityNumber = entityPM.ShipmentNumber != null ? entityPM.ShipmentNumber : entityPM.QuoteNumber;
                if (string.IsNullOrEmpty(entityPM.GuidId))
                {
                    // Guid 
                    var guid = Guid.NewGuid();
                    var base64string = Convert.ToBase64String(guid.ToByteArray()).ToLower();
                    base64string = base64string.Substring(0, 22);
                    base64string = base64string.Replace("/", "_");
                    base64string = base64string.Replace("+", "_"); // "-" must ask about that 
                    base64string = base64string.Replace("-", "_");
                    entityPM.GuidId = base64string;
                }

              


                #region Automation 
                ICRMContext context = CRMContext.GetContext(entityPM.Tenant);
                TicketClassificationPM classification = new TicketClassificationPM();
                TicketClassificationQueryService classificationQuery = new TicketClassificationQueryService(context);
                SLAHeaderQueryService sLAHeaderQuery = new SLAHeaderQueryService(context);
                EmployeeGroupQueryService employeeGroupQuery = new EmployeeGroupQueryService(context);
                TicketClassificationRepository repClassification = new TicketClassificationRepository(entityPM.Tenant);


                if (!string.IsNullOrEmpty(entityPM.SecondaryClassificationId))
                {
                    classification = classificationQuery.GetSingle(entityPM.SecondaryClassificationId, true, false);
                }
                else
                {
                    classification = classificationQuery.GetSingle(entityPM.MainClassificationId, true, false);
                }
                entityPM.ClassificationManager = classification.ManagerUserEmail;
                entityPM.ClassificationNotify = classification.EscalationNotify;

                EmployeeGroupPM groupManager = employeeGroupQuery.GetSingle(entityPM.EmployeeGroupId, true, false);
                if (groupManager != null)
                {
                    entityPM.GroupManager = groupManager.ManagerUserEmail;
                }

                var employeeGroupPM = employeeGroupQuery.GetSingle(entityPM.EmployeeGroupId, true, false);
                if (employeeGroupPM != null)
                {
                    entityPM.GroupNotify = employeeGroupPM.EscalationNotify;
                    entityPM.EmployeeGroupName = employeeGroupPM.Name;
                }
                #endregion

                if (!string.IsNullOrEmpty(entityPM.OwnerId) && !entityPM.IsCreatedFromOutSide)
                {
                    this.CheckOwnerFeature(entityPM.Tenant, entityPM.OwnerId, entityPM.OwnerName);
                }

                EntityChangeHelper entityChangeHelper = new EntityChangeHelper();
                entityChangeHelper.AddEntityChange(entityPM, this.OldEntityPM, "OnCreate", "", "Ticket");


                this.SetTimeIssues(entityPM, null);
                TicketEscalationAnalyzer ticketEscalationAnalyzer = new TicketEscalationAnalyzer(entityPM, true);

            }
        }

        protected override void OnUpdating(EntityPMs.TicketPM entityPM)
        {
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.EntityNumber = entityPM.ShipmentNumber != null ? entityPM.ShipmentNumber : entityPM.QuoteNumber;

            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists() && !entityPM.IsUpdateByAutomation)
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }

            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.UpdateDate = myDate;
            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = myDate;
                if (entityPM.CreatedByContactId == null)
                {
                    entityPM.CreatedByContactId = myLoggedUserId;
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void OnUpdating(EntityPMs.TicketPM entityPM, Ticket entityPOCO)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                #region Automation 
                ICRMContext context = CRMContext.GetContext(entityPM.Tenant);
                TicketClassificationPM classification = new TicketClassificationPM();
                TicketClassificationQueryService classificationQuery = new TicketClassificationQueryService(context);
                SLAHeaderQueryService sLAHeaderQuery = new SLAHeaderQueryService(context);
                EmployeeGroupQueryService employeeGroupQuery = new EmployeeGroupQueryService(context);
                TicketClassificationRepository repClassification = new TicketClassificationRepository(entityPM.Tenant);

                if (!string.IsNullOrEmpty(entityPM.SecondaryClassificationId))
                {
                    classification = classificationQuery.GetSingle(entityPM.SecondaryClassificationId, true, false);
                }
                else
                {
                    classification = classificationQuery.GetSingle(entityPM.MainClassificationId, true, false);
                }
                entityPM.ClassificationManager = classification.ManagerUserEmail;
                entityPM.ClassificationNotify = classification.EscalationNotify;

                EmployeeGroupPM groupManager = employeeGroupQuery.GetSingle(entityPM.EmployeeGroupId, true, false);
                if (groupManager != null)
                {
                    entityPM.GroupManager = groupManager.ManagerUserEmail;
                }

                var employeeGroupPM = employeeGroupQuery.GetSingle(entityPM.EmployeeGroupId, true, false);
                if (employeeGroupPM != null)
                {
                    entityPM.GroupNotify = employeeGroupPM.EscalationNotify;
                }

                #endregion

                bool IsChangeSLAViaAutomation = false;
                if (!entityPM.IsUpdateByAutomation)
                {
                    EntityChangeHelper entityChangeHelper = new EntityChangeHelper();
                    entityChangeHelper.AddEntityChange(entityPM, this.OldEntityPM, "OnUpdate", this.EntityChangeFieldXml, "Ticket");
                    IsChangeSLAViaAutomation = entityChangeHelper.IsChangeSLA;

                }

                if (entityPM.SeverityId != entityPOCO.SeverityId || entityPM.OwnerId != entityPOCO.OwnerId || entityPM.EmployeeGroupId != entityPOCO.EmployeeGroupId || IsChangeSLAViaAutomation)
                {
                    this.SetTimeIssues(entityPM, entityPOCO, IsChangeSLAViaAutomation);
                    TicketEscalationAnalyzer ticketEscalationAnalyzer = new TicketEscalationAnalyzer(entityPM, false);
                }

                // TotalOpenPeriod stages Update
                if (entityPM.StageId != entityPOCO.StageId)
                {
                    TicketStageQueryService myQuery = new TicketStageQueryService(entityPM.Tenant);
                    TicketStageRepository repository = new TicketStageRepository(entityPM.Tenant);
                    TicketStage resolveStage = repository.GetTicketStageByCode("RE", entityPM.Tenant);
                    TicketStage closedStage = repository.GetTicketStageByCode("CS", entityPM.Tenant);
                    TicketStage openedStage = repository.GetTicketStageByCode("OP", entityPM.Tenant);
                    SLALineRepository slaRep = new SLALineRepository(entityPM.Tenant);
                    BusinessHourRepository businessHourRep = new BusinessHourRepository(entityPM.Tenant);
                    SLALine slaLine = slaRep.GetSLALineBySeverityId(entityPM.Tenant, entityPM.SeverityId, entityPM.SLAId);
                    BusinessHour businessHour = new BusinessHour();
                    businessHour = businessHourRep.GetSingleBusinessHours(slaLine.BusinessHoursId, slaLine.Tenant);
                    BusinessHourCalcualtions businessCalculation = new BusinessHourCalcualtions(businessHour);
                   // DateTime? nextDate = new DateTime();

                    DateTime? nextDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                    if ((entityPM.StageId == openedStage.Id) && (entityPOCO.StageId == resolveStage.Id || entityPOCO.StageId == closedStage.Id))
                    {
                        entityPM.OpenDate = todayDate;
                    }

                    if ((entityPOCO.StageId == openedStage.Id) && (entityPM.StageId == resolveStage.Id || entityPM.StageId == closedStage.Id))
                    {
                        if (entityPM.OpenDate != null) {
                            if (entityPM.StageId == resolveStage.Id)
                            {
                                nextDate = entityPM.FullResolvedTime;
                            }

                            if (entityPM.StageId == closedStage.Id)
                            {
                                nextDate = entityPM.LastCloseDate;
                            }

                            int d = businessCalculation.CalculateBusinessHours(entityPM.OpenDate, nextDate, businessHour.Is247);

                            if (entityPM.OpenPeriodMinutes == null)
                            {
                                entityPM.OpenPeriodMinutes = d;
                            }
                            else
                            {
                                entityPM.OpenPeriodMinutes += d;
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(entityPM.OwnerId))
                {

                    this.CheckOwnerFeature(entityPM.Tenant, entityPM.OwnerId, entityPM.OwnerName);
                }

                this.UpdateDates(entityPM);
            }
        }

        protected override void UpdateComposition(TicketPM entityPM)
        {

        }

        protected override void Trace(TicketPM entityPM, Ticket entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);

            string loggedContactId = null;

            if (entityPM.IsCreatedFromOutSide || entityPM.IsUpdateByAutomation)
            {
                loggedContactId = entityPM.UpdatedByUserId;
            }

            else
            {
                ContactRepository contactRep = new ContactRepository(commonContext);
                Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
                if (contact != null)
                {
                    loggedContactId = contact.Id;
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                if (!string.IsNullOrEmpty(entityPM.ShipmentNumber) && !string.IsNullOrEmpty(entityPOCO.ShipmentNumber) && entityPM.ShipmentNumber != entityPOCO.ShipmentNumber)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TSDC",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Shipment " + entityPOCO.ShipmentNumber + " Disconnected"
                    });

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "STDC",
                        UserId = loggedContactId,
                        EntityId = entityPOCO.ShipmentId,
                        ObjectTableName = "Shipment",
                        Notes = "Shipment disconnected from Ticket " + entityPM.TicketNumber
                    });


                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TSCN",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Shipment " + entityPM.ShipmentNumber + " Connected"
                    });

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "STCN",
                        UserId = loggedContactId,
                        EntityId = entityPM.ShipmentId,
                        ObjectTableName = "Shipment",
                        Notes = "Shipment connected to Ticket " + entityPM.TicketNumber
                    });
                }

                if (string.IsNullOrEmpty(entityPM.ShipmentNumber) && !string.IsNullOrEmpty(entityPOCO.ShipmentNumber))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TSDC",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Shipment " + entityPOCO.ShipmentNumber + " Disconnected"
                    });

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "STDC",
                        UserId = loggedContactId,
                        EntityId = entityPOCO.ShipmentId,
                        ObjectTableName = "Shipment",
                        Notes = "Shipment disconnected from Ticket " + entityPM.TicketNumber
                    });
                }

                if (!string.IsNullOrEmpty(entityPM.ShipmentNumber) && string.IsNullOrEmpty(entityPOCO.ShipmentNumber))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TSCN",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Shipment " + entityPM.ShipmentNumber + " Connected"
                    });

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "STCN",
                        UserId = loggedContactId,
                        EntityId = entityPM.ShipmentId,
                        ObjectTableName = "Shipment",
                        Notes = "Shipment connected to Ticket " + entityPM.TicketNumber
                    });
                }

                if (!string.IsNullOrEmpty(entityPM.QuoteNumber) && !string.IsNullOrEmpty(entityPOCO.QuoteNumber) && entityPOCO.QuoteNumber != entityPM.QuoteNumber)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TQDC",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Quote " + entityPOCO.QuoteNumber + " Disconnected"
                    });

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "QTDC",
                        UserId = loggedContactId,
                        EntityId = entityPOCO.QuoteId,
                        ObjectTableName = "Quote",
                        Notes = "Quote disconnected from Ticket " + entityPOCO.TicketNumber
                    });


                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TQCN",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Quote " + entityPM.QuoteNumber + " Connected"
                    });

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "QTCN",
                        UserId = loggedContactId,
                        EntityId = entityPM.QuoteId,
                        ObjectTableName = "Quote",
                        Notes = "Quote connected to Ticket " + entityPM.TicketNumber
                    });

                }

                if (string.IsNullOrEmpty(entityPM.QuoteNumber) && !string.IsNullOrEmpty(entityPOCO.QuoteNumber))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TQDC",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Quote "+ entityPOCO.QuoteNumber + " Disconnected"
                    });

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "QTDC",
                        UserId = loggedContactId,
                        EntityId = entityPOCO.QuoteId,
                        ObjectTableName = "Quote",
                        Notes = "Quote disconnected from Ticket " + entityPM.TicketNumber
                    });
                }

                if (!string.IsNullOrEmpty(entityPM.QuoteNumber) && string.IsNullOrEmpty(entityPOCO.QuoteNumber))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TQCN",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Quote " + entityPM.QuoteNumber + " Connected"
                    });

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "QTCN",
                        UserId = loggedContactId,
                        EntityId = entityPM.QuoteId,
                        ObjectTableName = "Quote",
                        Notes = "Quote connected to Ticket " + entityPM.TicketNumber
                    });
                }


                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPTK",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Ticket",
                    Notes = changesXml
                });

                if (entityPM.IsCancelled && !entityPOCO.IsCancelled)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "CATK",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = changesXml
                    });
                }

                if (!entityPM.IsCancelled && entityPOCO.IsCancelled)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "RATK",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = changesXml
                    });
                }

                if (entityPM.IsClosed && !entityPOCO.IsClosed)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "CLTK",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = changesXml
                    });
                }

                if (entityPM.StageId != entityPOCO.StageId)
                {
                    TicketStageQueryService myQuery = new TicketStageQueryService(entityPM.Tenant);
                    TicketStagePM myStage = myQuery.GetSingle(entityPOCO.StageId, false, false);

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "SCTK",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Ticket",
                        Notes = "Ticket Stage Changed From " + myStage.Name + " To " + entityPM.StageName,
                    });
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityPM.IsCreatedFromOutSide)
                {
                    changesXml = "Ticket Created Via Email";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRTK",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Ticket",
                    Notes = changesXml
                });
            }
        }

        private void SetTicketFirstResponceTime(TicketPM entityPM)
        {
            ICRMContext context = CRMContext.GetContext(entityPM.Tenant);
            CorrespondenceQueryService correspondenceService = new CorrespondenceQueryService(context);
            List<CorrespondencePM> myCorrespondences = correspondenceService.GetCorrespondencesByEntityId(entityPM.Id, entityPM.Tenant);
            CorrespondencePM myCorrespondencePM = myCorrespondences.FirstOrDefault();
            if (myCorrespondences.Count == 2 && !myCorrespondencePM.IsInternal)
            {
                InboundEmailLineRepository emailLineRep = new InboundEmailLineRepository(entityPM.Tenant);
                InboundEmailLine myLine = emailLineRep.GetSingleByEntityLineId(myCorrespondencePM.Id, entityPM.Tenant);
                if (myLine != null)
                {
                    entityPM.FirstResponseTime = entityPM.CreateDate;
                }
            }
        }

        protected override void CheckConcurrency(TicketPM entityPM, Ticket entityPOCO)
        {
            if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            {

            }

            base.CheckConcurrency(entityPM, entityPOCO);
        }
           
        BusinessHour businessHour { get; set; }

        public void SetTimeIssues(TicketPM entityPM, Ticket entityPOCO, bool isChangeSLAViaAutomation = false)
        {
            bool isExecuting = false;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert )
            {
                isExecuting = true;
            }

            else
            {
                if (entityPM.SeverityId != entityPOCO.SeverityId || entityPM.OwnerId != entityPOCO.OwnerId || entityPM.EmployeeGroupId != entityPOCO.EmployeeGroupId || isChangeSLAViaAutomation)
                {
                   isExecuting = true;
                }
            }

            if (isExecuting)
            {
                SLALineRepository slaRep = new SLALineRepository(entityPM.Tenant);
                BusinessHourRepository businessHourRep = new BusinessHourRepository(entityPM.Tenant);
                SLALine slaLine = slaRep.GetSLALineBySeverityId(entityPM.Tenant, entityPM.SeverityId, entityPM.SLAId);
                businessHour = new BusinessHour();

                if (slaLine != null)
                {
                    businessHour = businessHourRep.GetSingleBusinessHours(slaLine.BusinessHoursId, slaLine.Tenant);
                    BusinessHourCalcualtions businessCalculation = new BusinessHourCalcualtions(businessHour);

                    if (entityPM.FirstResponseTime == null && slaLine.FirstResponseEscalate)
                    {
                        if (slaLine.FirstResponseTimeInMinute != null)
                        {
                            DateTime ticketCreateDate = entityPM.CreateDate.Value.AddMinutes((double)slaLine.FirstResponseTimeInMinute);
                            // Calender
                            if (businessHour.Is247)
                            {
                                bool isHolidayDay  = businessCalculation.isHolidayDay(ticketCreateDate);
                                if (isHolidayDay)
                                {
                                    ticketCreateDate = businessCalculation.NextDayAfterHoliday24Hour(ticketCreateDate);
                                    entityPM.FirstResponseDue = ticketCreateDate.AddMinutes((double)slaLine.FirstResponseTimeInMinute);
                                }
                                else
                                {
                                    entityPM.FirstResponseDue = ticketCreateDate;
                                }
                            }

                            // Business Hour 
                            else
                            {
                                entityPM.FirstResponseDue = businessCalculation.addResolveMinutes(entityPM.CreateDate.Value, (int)slaLine.FirstResponseTimeInMinute);
                            }

                            //TicketEscalationAnalyzer ticketEscalationAnalyzer = new TicketEscalationAnalyzer(entityPM, true);
                        }
                    }

                    if (entityPM.FullResolvedTime == null && slaLine.ResolveWithinEscalate)
                    {
                        if (slaLine.ResolveWithinTimeInMinute != null)
                        {
                            DateTime ticketCreateDate = entityPM.CreateDate.Value.AddMinutes((double)slaLine.ResolveWithinTimeInMinute);

                            // Calender
                            if (businessHour.Is247)
                            {
                               // entityPM.ResolveWithinDue = ticketCreateDate;
                                bool isHolidayDay = businessCalculation.isHolidayDay(ticketCreateDate);
                                if (isHolidayDay)
                                {
                                    ticketCreateDate = businessCalculation.NextDayAfterHoliday24Hour(ticketCreateDate);
                                    entityPM.ResolveWithinDue = ticketCreateDate.AddMinutes((double)slaLine.ResolveWithinTimeInMinute);
                                }
                                else
                                {
                                    entityPM.ResolveWithinDue = ticketCreateDate;
                                }
                            }

                            // Business Hour 
                            else
                            {
                                entityPM.ResolveWithinDue = businessCalculation.addResolveMinutes(entityPM.CreateDate.Value, (int)slaLine.ResolveWithinTimeInMinute);
                            }

                          //  TicketEscalationAnalyzer ticketEscalationAnalyzer = new TicketEscalationAnalyzer(entityPM, true);
                        }
                    }

                }
            }
        }

        //public void CheckOwnerFeature(int tenant, string ownerId, string ownerName)
        //{
        //    string msg = "Can't set " + ownerName + " as owner. The user is not licensed for tickets";

        //    ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
        //    ContactTenantRoleRepository contactTenantRole = new ContactTenantRoleRepository(commonContext);
        //    ContactTenantRepository contactTenantRepository = new ContactTenantRepository(commonContext);
        //    RoleRepository roleRepository = new RoleRepository(commonContext);
        //    FeatureRepository featureRepository = new FeatureRepository(commonContext);
        //    ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
        //    RoleFeatureRepository roleFeatureRepository = new RoleFeatureRepository(commonContext);

        //    ContactTenant contacttenant = contactTenantRepository.GetContactTenantForContactId(ownerId,tenant);
        //    if(contacttenant != null)
        //    {
        //        var contacttenantId = contacttenant.Id;
        //        IQueryable<ContactTenantRole> queryableRoles = contactTenantRole.GetContactTenantRolesListByContactAndTenant(contacttenantId, tenant);
        //        List<string> allRolesIds = queryableRoles.Select(s => s.RoleId).ToList();
        //        var ticketObjectTable = objectTableRepository.GetObjectTableByName("Ticket",tenant,false);
        //        Feature ticketUpdateFeature = featureRepository.GetSingleFeatureByCode(ticketObjectTable.Id, "UPDATE", tenant);

        //        if (ticketUpdateFeature != null)
        //        {
        //            var featureId = ticketUpdateFeature.Id;
        //            List<RoleFeature> allRolesFeatures = roleFeatureRepository.GetRoleFeatureByRoleIds(allRolesIds, tenant);
        //            RoleFeature updateRoleFeature = allRolesFeatures.Where(a => a.FeatureId == featureId).FirstOrDefault();

        //            if(updateRoleFeature == null)
        //            {
        //                throw new ApplicationException(msg);
        //            }
        //        }
        //        else
        //        {
        //            throw new ApplicationException(msg);
        //        }
        //    }
        //    else
        //    {
        //        throw new ApplicationException(msg);
        //    }
        //}
        public void CheckOwnerFeature(int tenant, string ownerId, string ownerName)
        {
            ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
            FeatureRepository myFeatureRepository = new FeatureRepository(myContext);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);

            FeatureQuery featureQuery = new FeatureQuery(myFeatureRepository);
            LoggedUserFeatures loggedUserFeatures = featureQuery.GetAllowedFeaturesForLoggedUser(ownerId, tenant);
            List<FeaturePM> allAllowedFeatures = loggedUserFeatures.Features;

            ObjectTable objectTable = objectTableRepository.GetObjectTableByName("Ticket", tenant, true);
            FeaturePM myFeature = allAllowedFeatures.Where(d => d.ObjectTableId == objectTable.Id && d.Code == "OwnerLicenseUpdate").FirstOrDefault();
            if(myFeature == null)
            {
                throw new ApplicationException("Can't set " + ownerName + " as owner. The user is not licensed for tickets");                
            }
        }
        private void UpdateDates(TicketPM entityPM)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            entityPM.IsResolveDue = (entityPM.FirstResolveDate != null) ? false : true;

            entityPM.ResolveColor = (entityPM.FirstResolveDate != null) ? "Green" : (entityPM.ResolveWithinDue > todayDateTime ? "Orange" : "Red");
            entityPM.IsResolveExamination = (entityPM.FirstResolveDate != null && entityPM.ResolveWithinDue < entityPM.FirstResolveDate) ? true : false;

            entityPM.IsResponseDue = (entityPM.FirstResponseTime != null) ? false : (true);
            entityPM.ResponseColor = (entityPM.FirstResponseTime != null) ? "Green" : (entityPM.FirstResponseDue > todayDateTime ? "Orange" : "Red");
            entityPM.IsResponseExamination = (entityPM.FirstResponseTime != null && entityPM.FirstResponseDue < entityPM.FirstResponseTime) ? true : false;

            entityPM.TicketFirstResponseTime = (entityPM.FirstResponseTime != null) ? entityPM.FirstResponseTime : entityPM.FirstResponseDue;
            entityPM.TicketFirstResolveTime = (entityPM.FirstResolveDate != null) ? entityPM.FirstResolveDate : entityPM.ResolveWithinDue;

            if (!string.IsNullOrEmpty(entityPM.SLAId))
            {
                SLAHeaderRepository mySLAHeaderRepository = new SLAHeaderRepository(entityPM.Tenant);
                SLAHeader mySLAHeader = mySLAHeaderRepository.GetSingle(entityPM.SLAId, entityPM.Tenant);
                entityPM.SLAName = mySLAHeader.Name;
            }
        }
    }
}
