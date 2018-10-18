
namespace WebFreight.Web.CRMModel.DomainServices
{
    using Logitude.CRM.BL.EntityQueryServices;
    using Logitude.CRM.Data;
    using Logitude.CRM.Data.EntityPOCOs;
    using Logitude.CRM.Data.Repsitories;
    using Simplog.Data.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using Logitude.BL.CommonDataModel.EntityPMs;
    using Logitude.BL.CommonDataModel.EntityQueries;
    using WebFreight.Web.DataContracts;
    using WebFreight.Web.Helpers;
    using WebFreight.Web.Security;
    using Simplog.Data.CommonDataModel.Repositories;
    using Logitude.CRM.Data.BusinessUnitFilters;
    using Logitude.BL.DataContracts;
    using Logitude.CRM.BL.EntityPMs;
    using Logitude.CRM.Data.EntityLists;
    using Logitude.CRM.Data.EntityListQueryServices;
    using Simplog.Server.Infrastructure.DataContracts;
    using Logitude.Server.Tools.Helpers;
    using Simplog.Server.Infrastructure;
    using Logitude.CRM.BL.EntityUpdateServices;
    using Simplog.Data.InfrastructureModel.Repositories;
    using System.Web;
    using Simplog.Data.CommonDataModel.EntityPOCOs;
    using Simplog.Data.InfrastructureModel.EntityPOCOs;
    
    using System.Data.Entity.Core;
    using Logitude.BL.Helpers;

    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public partial class CRMDomainService : LogitudeDomainService
    {
        ICRMContext crmContext;
       
        // Repositories
        private ActivityRepository activityRepository;
        private OpportunityRepository opportunityRepository;
        private TicketRepository ticketRepository;

        // Queries
        private ActivityQueryService activityQuery;
        private ActivityTimeTypeQueryService activityTimeTypeQuery;
        private ActivityTypeQueryService activityTypeQuery;
        private CallTypeQueryService callTypeQuery;
        private ActivityPriorityQueryService priorityQuery;
        private ActivityStatusQueryService activityStatusQuery;
        private OpportunityClosingReasonQueryService closingReasonQuery;
        private RatingQueryService ratingQuery;
        private StageQueryService stageQuery;
        private OpportunityQueryService opportunityQuery;
        private OpportunityTypeQueryService opportunityTypeQuery;
        private OpportunityProductLocationQueryService opportunityProductLocationQuery;
        private OpportunityProductQueryService opportunityProductQuery;
        private OpportunityStageQueryService opportunityStageQuery;
        private CRMFilterSettingQueryService cRMFilterSettingQueryService;
        private TicketTypeQueryService ticketTypeQuery;
        private TicketSeverityQueryService ticketSeverityQuery;
        private TicketStageQueryService ticketStageQuery;
        private TicketClassificationQueryService ticketClassificationQuery;
        private TicketQueryService ticketQuery;
        private CorrespondenceQueryService correspondenceQuery;

        public CRMSummary GetActivitiesSummary(int tenant, string activityTypeCode, string ownerId, string businessUnitId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);           

            activityRepository = new ActivityRepository(tenant);
            IQueryable<Activity> dataSource = activityRepository.GetAll(tenant);

            if (!string.IsNullOrEmpty(activityTypeCode))
            {
                dataSource = dataSource.Where(d => d.ActivityTypeCode == activityTypeCode);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
            }

            dataSource = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Activity>(new QueryOperations(), dataSource, tenant);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            dataSource = filter.RunFilter(dataSource);

            string rr = ServiceContext.User.Identity.Name;

            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContact = contactQuery.GetContactByNameAndTenant(rr, tenant, true);

            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(rr, tenant);
            }

            CRMSummary summaryClass = new CRMSummary() { Id = tenant };
            summaryClass.MyOpenDataCount = dataSource.Where(d => d.OwnerId == loggedContact.Id && d.IsOpen).Count();
            summaryClass.AllOpenDataCount = dataSource.Where(d => d.IsOpen).Count();

            return summaryClass;
        }

        public CRMSummary GetOpportunitiesSummary(string ownerId, string businessUnitId, int tenant,string contactName=null)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         
            opportunityRepository = new OpportunityRepository(tenant);
            IQueryable<Opportunity> dataSource = opportunityRepository.GetAll(tenant).Where(d => d.IsCancelled == false);

            if(!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
            }

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            dataSource = filter.RunFilter(dataSource);

            string rr = "";
            if (contactName == null)
                 rr = ServiceContext.User.Identity.Name;
            else
                rr = contactName;

            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContact = contactQuery.GetContactByNameAndTenant(rr, tenant, true);

            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(rr, tenant);
            }

            CRMSummary summaryClass = new CRMSummary() { Id = tenant };
            summaryClass.MyOpenDataCount = dataSource.Where(d => d.OwnerId == loggedContact.Id && !d.IsClosed).Count();
            summaryClass.AllOpenDataCount = dataSource.Where(d => !d.IsClosed).Count();
            summaryClass.OpenByStageCount = dataSource.Where(d => !d.IsClosed && d.LastStageDate != null).Count();

            return summaryClass;
        }

        public CRMSummary GetTicketsSummary(string ownerId, string employeeGroupId, int tenant, string serviceContextUser = null)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ticketRepository = new TicketRepository(tenant);
            IQueryable<Ticket> dataSource = ticketRepository.GetAll(tenant);

            //TicketBusinessUnitFilter filter = new TicketBusinessUnitFilter(tenant);
            //dataSource = filter.RunFilter(dataSource);

            string rr = "";

            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContact;
            if (serviceContextUser != null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(serviceContextUser, tenant);
            }
            else
            {
                rr = ServiceContext.User.Identity.Name;
                loggedContact = contactQuery.GetContactByNameAndTenant(rr, tenant, true);
            }

            if (loggedContact == null)
            {
                rr = ServiceContext.User.Identity.Name;
                loggedContact = contactQuery.GetContactByEmailOnly(rr, tenant);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSource = dataSource.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

            CRMSummary summaryClass = new CRMSummary() { Id = tenant };
            summaryClass.MyOpenDataCount = dataSource.Where(d => !d.IsClosed && !d.IsCancelled && d.StageId != resolvedStage.Id && d.StageId != closedStage.Id).Count();
            summaryClass.Unassigned_Tickets = dataSource.Where(d => d.OwnerId == null && (d.StageId != resolvedStage.Id && d.StageId != closedStage.Id)).Count();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            summaryClass.SLA_Failures = dataSource.Where(d => d.IsCancelled == false && d.IsClosed == false
                                                             && (d.StageId != resolvedStage.Id && d.StageId != closedStage.Id)
                                                             && ((d.FirstResponseTime == null && d.FirstResponseDue != null && d.FirstResponseDue < todayDate)
                                                             || (d.FirstResponseTime != null && d.FirstResponseDue != null && d.FirstResponseDue < d.FirstResponseTime)
                                                             || (d.FirstResolveDate == null && d.ResolveWithinDue != null && d.ResolveWithinDue < todayDate)
                                                             || (d.FirstResolveDate != null && d.ResolveWithinDue != null && d.ResolveWithinDue < d.FirstResolveDate))
                                                     ).Count();

            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            UserRepository userRepository = new UserRepository(tenant);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);

            summaryClass.RecentlyUpdated_Tickets = dataSource.Where(d => !d.IsClosed && !d.IsCancelled && d.StageId != resolvedStage.Id && d.StageId != closedStage.Id && d.UpdatedByUserId != loggedUser.Id).Count();

            return summaryClass;
        }

        public void UpdateChartingDataClass(ChartingDataClass entity)
        {
            // Ayman: Don't remove this please
        }

        protected override bool PersistChangeSet()
        {
            try
            {
                crmContext.SaveChanges();
            }

            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception("Sorry you can't update this record right now it's being updated by another user");
            }

            return base.PersistChangeSet();
        }

        #region Time Unit  
        public TimeUnitPM GetSingleTimeUnitPM(string code, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TimeUnitQueryService timeUnitQuery = new TimeUnitQueryService(crmContext);
            TimeUnitPM timeUnitPM = timeUnitQuery.GetSingle(code, false, false);
            return timeUnitPM;

        }

        public TimeUnitList GetSingleTimeUnitList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }


            TimeUnitListQueryService listService = new TimeUnitListQueryService(crmContext);
            return listService.GetSingle(code);
        }

        public List<TimeUnitList> GetTimeUnitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            TimeUnitListQueryService listService = new TimeUnitListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<TimeUnitList> GetTimeUnitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            TimeUnitListQueryService listService = new TimeUnitListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTimeUnitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            TimeUnitListQueryService queryService = new TimeUnitListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
        #endregion 

        #region SLA Header 
        public SLAHeaderPM GetSingleSLAHeaderPM(string id, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            SLAHeaderQueryService sLAHeaderQuery = new SLAHeaderQueryService(crmContext);
            SLAHeaderPM sLAHeaderPM = sLAHeaderQuery.GetSingle(id, false, false);
            return sLAHeaderPM;

        }

        public SLAHeaderList GetSingleSLAHeaderList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }


            SLAHeaderListQueryService listService = new SLAHeaderListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<SLAHeaderList> GetSLAHeaderLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            SLAHeaderListQueryService listService = new SLAHeaderListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<SLAHeaderList> GetSLAHeadersFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            SLAHeaderListQueryService listService = new SLAHeaderListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetSLAHeaderFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            SLAHeaderListQueryService queryService = new SLAHeaderListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertSLAHeader(SLAHeaderPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("SLAHeader", "NEW", entityPM.Tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            };
            SLAHeaderUpdateService service = new SLAHeaderUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);

           
            //List<SLALinePM> SLALinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLALines).Cast<SLALinePM>().ToList();
            //foreach (SLALinePM SLALine in SLALinesChangeSet)
            //{
            //    entityPM.SLALines.Where(d => d.Id == SLALine.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //}

            //List<SLAEscalationPM> SLAEscalationsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLAEscalations).Cast<SLAEscalationPM>().ToList();
            //foreach (SLAEscalationPM SLAEscalation in SLAEscalationsChangeSet)
            //{
            //    entityPM.SLAEscalations.Where(d => d.Id == SLAEscalation.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //}

            SetSLALineChangeSet(entityPM);
            SetSLAEscalationChangeSet(entityPM);

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            //List<SLAEscalationRecepientPM> SLAEscalationRecepientsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLAEscalations.SLAEscalationRecepients).Cast<SLAEscalationRecepientPM>().ToList();
            //foreach (SLAEscalationRecepientPM SLAEscalationRecepient in SLAEscalationRecepientsChangeSet)
            //{
            //    entityPM.SLAEscalationRecepients.Where(d => d.Id == SLAEscalationRecepient.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //}

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("SLAHeader", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }

        public void UpdateSLAHeader(SLAHeaderPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("SLAHeader", "UPDATE", entityPM.Tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            };
            SLAHeaderUpdateService service = new SLAHeaderUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            SetSLALineChangeSet(entityPM);
            SetSLAEscalationChangeSet(entityPM);

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("SLAHeader", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }

        private void SetSLALineChangeSet(SLAHeaderPM entityPM)
        {
            List<SLALinePM> SLALinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLALines).Cast<SLALinePM>().ToList();

            foreach (SLALinePM itemPM in SLALinesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SLALinePM currentItemPM = entityPM.SLALines.Where(d => d.SLAHeaderId == itemPM.SLAHeaderId && d.ChangeSetOp != ChangeSetOperation.Insert && d.Id == null).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            SLALinePM currentItemPM = entityPM.SLALines.Where(d => d.SLAHeaderId == itemPM.SLAHeaderId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            SLALinePM currentItemPM = new SLALinePM() { ChangeSetOp = ChangeSetOperation.Delete, SLAHeaderId = itemPM.SLAHeaderId, Id = itemPM.Id };
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            entityPM.DeletedSLALines.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            SLALinePM currentItemPM = entityPM.SLALines.Where(d => d.SLAHeaderId == itemPM.SLAHeaderId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetSLAEscalationChangeSet(SLAHeaderPM entityPM)
        {
            List<SLAEscalationPM> SLAEscalationsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLAEscalations).Cast<SLAEscalationPM>().ToList();

            foreach (SLAEscalationPM itemPM in SLAEscalationsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SLAEscalationPM currentItemPM = entityPM.SLAEscalations.Where(d => d.SLAHeaderId == itemPM.SLAHeaderId && d.ChangeSetOp != ChangeSetOperation.Insert && d.Id == null).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            SetSLAEscalationRecepientChangeSet(currentItemPM); 
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            SLAEscalationPM currentItemPM = entityPM.SLAEscalations.Where(d => d.SLAHeaderId == itemPM.SLAHeaderId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            SetSLAEscalationRecepientChangeSet(currentItemPM); 
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            SLAEscalationPM currentItemPM = new SLAEscalationPM() { ChangeSetOp = ChangeSetOperation.Delete, SLAHeaderId = itemPM.SLAHeaderId, Id = itemPM.Id };
                         
                            #region composition handling
                            List<SLAEscalationRecepientPM> recipientsModificationsChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SLAEscalationRecepients).Cast<SLAEscalationRecepientPM>().ToList();
                            foreach (SLAEscalationRecepientPM item in recipientsModificationsChangeset)
                            {
                                SLAEscalationRecepientPM deletedItem = new SLAEscalationRecepientPM()
                                {
                                    Id= item.Id,
                                    SLAEscalationId = item.SLAEscalationId,
                                    Tenant = item.Tenant,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };
                                currentItemPM.SLAEscalationRecepients.Add(deletedItem);
                            }
                            #endregion
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            entityPM.DeletedSLAEscalations.Add(currentItemPM);

                            break;
                        }

                    default:
                        {
                            SLAEscalationPM currentItemPM = entityPM.SLAEscalations.Where(d => d.SLAHeaderId == itemPM.SLAHeaderId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetSLAEscalationRecepientChangeSet(SLAEscalationPM entityPM)
        {
            List<SLAEscalationRecepientPM> SLAEscalationRecepientsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLAEscalationRecepients).Cast<SLAEscalationRecepientPM>().ToList();

            foreach (SLAEscalationRecepientPM itemPM in SLAEscalationRecepientsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SLAEscalationRecepientPM currentItemPM = entityPM.SLAEscalationRecepients.Where(d => d.SLAEscalationId == itemPM.SLAEscalationId && d.ChangeSetOp != ChangeSetOperation.Insert && d.Id == null).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            SLAEscalationRecepientPM currentItemPM = entityPM.SLAEscalationRecepients.Where(d =>d.SLAEscalationId == itemPM.SLAEscalationId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            SLAEscalationRecepientPM currentItemPM = new SLAEscalationRecepientPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                SLAEscalationId = itemPM.SLAEscalationId,
                                Id = itemPM.Id,
                            };

                            entityPM.DeletedSLAEscalationRecepients.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            SLAEscalationRecepientPM currentItemPM = entityPM.SLAEscalationRecepients.Where( d => d.SLAEscalationId == itemPM.SLAEscalationId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        } 

        public SLALineList GetSingleSLALineList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLALine", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }


            SLALineListQueryService listService = new SLALineListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public SLAEscalationList GetSingleSLAEscalationList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLAEscalation", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }


            SLAEscalationListQueryService listService = new SLAEscalationListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public SLAEscalationRecepientList GetSingleSLAEscalationRecepientList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLAEscalationRecepient", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            SLAEscalationRecepientListQueryService listService = new SLAEscalationRecepientListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public SLAEscalationPM GetSingleSLAEscalationPM(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLAEscalation", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };

            SLAEscalationQueryService SLAEscalationQuery = new SLAEscalationQueryService(tenant);
            SLAEscalationPM entityPM = SLAEscalationQuery.GetSinglePMByTenant(tenant);

            return entityPM;
        }

        public SLAHeaderPM GetSingleSLAHeaderPMByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };

            SLAHeaderQueryService SLAHeaderQuery = new SLAHeaderQueryService(tenant);
            SLAHeaderPM entityPM = SLAHeaderQuery.GetSinglePMByTenant(tenant);

            return entityPM;
        }

        #endregion 

        #region EmployeeGroup

        public EmployeeGroupPM GetSingleEmployeeGroupPM(string id,int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            EmployeeGroupQueryService employeeGroupQuery = new EmployeeGroupQueryService(crmContext);
            EmployeeGroupPM employeeGroupPM = employeeGroupQuery.GetSingle(id, true, false);
            return employeeGroupPM;
           
        }

		public EmployeeGroupList GetSingleEmployeeGroupList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

          
            EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(crmContext);
            return listService.GetSingle(id);
        }

		public List<EmployeeGroupList> GetEmployeeGroupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(crmContext);
            return listService.GetList(tenant);
        }
       
	    public List<EmployeeGroupList> GetEmployeeGroupsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetEmployeeGroupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            EmployeeGroupListQueryService queryService = new EmployeeGroupListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertEmployeeGroup(EmployeeGroupPM entityPM)
        {
            int tenant = entityPM.Tenant;
			
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("EmployeeGroup", "NEW", tenant);

            if (crmContext == null)
            {  
                crmContext = CRMContext.GetContext(tenant);
            }
        
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.CreateDate = todayDate;
            entityPM.UpdateDate = todayDate;

			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            if (loggedContact != null)
            {
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;
        }

            SetEmployeeGroupLineChangeSet(entityPM);

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            EmployeeGroupUpdateService service = new EmployeeGroupUpdateService(crmContext, new Dictionary<string, IContext>(), tenant);
            service.Update(entityPM, true);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "User");
        }

		public void UpdateEmployeeGroup(EmployeeGroupPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("EmployeeGroup", "UPDATE", entityPM.Tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            EmployeeGroupUpdateService service = new EmployeeGroupUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
			
			SetEmployeeGroupLineChangeSet(entityPM); 			 
            service.Update(entityPM, true);

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("EmployeeGroup", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            }

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "User");
        } 

		private void SetEmployeeGroupLineChangeSet(EmployeeGroupPM entityPM)
        {
            List<EmployeeGroupLinePM> entityChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.EmployeeGroupLines).Cast<EmployeeGroupLinePM>().ToList();

            foreach (EmployeeGroupLinePM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        { 
                            EmployeeGroupLinePM currentItemPM = entityPM.EmployeeGroupLines.Where(d => d.EmployeeGroupId == itemPM.EmployeeGroupId && d.ChangeSetOp != ChangeSetOperation.Insert && d.Id == null).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;                            
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            EmployeeGroupLinePM currentItemPM = entityPM.EmployeeGroupLines.Where(d => d.EmployeeGroupId == itemPM.EmployeeGroupId && d.Id == itemPM.Id).FirstOrDefault();
						    currentItemPM.ChangeSetOp = ChangeSetOperation.Update; 
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            EmployeeGroupLinePM currentItemPM = new EmployeeGroupLinePM() { ChangeSetOp = ChangeSetOperation.Delete, EmployeeGroupId = itemPM.EmployeeGroupId, Id = itemPM.Id };
                            entityPM.DeletedEmployeeGroupLines.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            EmployeeGroupLinePM currentItemPM = entityPM.EmployeeGroupLines.Where(d => d.EmployeeGroupId == itemPM.EmployeeGroupId && d.Id == itemPM.Id).FirstOrDefault();
						   currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                           break;
                        }
                }
            }
        }

        public List<EmployeeGroupPM> GetEmployeeGroupsPMList(int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            EmployeeGroupQueryService listService = new EmployeeGroupQueryService(crmContext);
            List<EmployeeGroupPM> myResult = listService.GetAllEmployeeGroupsByTenant(tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("EmployeeGroup", tenant, myResult.Cast<object>().ToList());
            return myResult.ToList();
        }

        #endregion

        #region EscalationPreDefinition
        public EscalationPreDefinitionPM GetSingleEscalationPreDefinitionPM(string code,int tenant)
        {
            if (crmContext == null)
            {
                  crmContext = CRMContext.GetContext(tenant);
            }

            EscalationPreDefinitionQueryService escalationPreDefinitionQuery = new EscalationPreDefinitionQueryService(crmContext);
            EscalationPreDefinitionPM escalationPreDefinitionPM = escalationPreDefinitionQuery.GetSingle(code,false,false);
            return escalationPreDefinitionPM;
           
        }

		public EscalationPreDefinitionList GetSingleEscalationPreDefinitionList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if ( crmContext == null)
            {
                 crmContext = CRMContext.GetContext(tenant);
            }

          
            EscalationPreDefinitionListQueryService listService = new EscalationPreDefinitionListQueryService(crmContext);
            return listService.GetSingle(code);
        }

		public List<EscalationPreDefinitionList> GetEscalationPreDefinitionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if ( crmContext == null)
            {
                  crmContext = CRMContext.GetContext(tenant);
            }
            EscalationPreDefinitionListQueryService listService = new EscalationPreDefinitionListQueryService(crmContext);
            return listService.GetList(tenant);
        }
       
	    public List<EscalationPreDefinitionList> GetEscalationPreDefinitionsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if ( crmContext == null)
            {
                  crmContext = CRMContext.GetContext(tenant);
            };
            EscalationPreDefinitionListQueryService listService = new EscalationPreDefinitionListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetEscalationPreDefinitionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if ( crmContext == null)
            {
                  crmContext = CRMContext.GetContext(tenant);
            };
            EscalationPreDefinitionListQueryService queryService = new EscalationPreDefinitionListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				

        #endregion 

        #region TicketEscalationPM
        public TicketEscalationPM GetSingleTicketEscalationPM(string id, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketEscalationQueryService ticketEscalationQuery = new TicketEscalationQueryService(crmContext);
            TicketEscalationPM ticketEscalationPM = ticketEscalationQuery.GetSingle(id, false, false);
            return ticketEscalationPM;

        }

        public TicketEscalationList GetSingleTicketEscalationList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TicketEscalation", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }


            TicketEscalationListQueryService listService = new TicketEscalationListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<TicketEscalationList> GetTicketEscalationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TicketEscalation", "READ", tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            TicketEscalationListQueryService listService = new TicketEscalationListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<TicketEscalationList> GetTicketEscalationsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TicketEscalation", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            TicketEscalationListQueryService listService = new TicketEscalationListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTicketEscalationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TicketEscalation", "READ", tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            TicketEscalationListQueryService queryService = new TicketEscalationListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertTicketEscalation(TicketEscalationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("TicketEscalation", "NEW", entityPM.Tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            };
            TicketEscalationUpdateService service = new TicketEscalationUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("TicketEscalation", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }

        public void UpdateTicketEscalation(TicketEscalationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("TicketEscalation", "UPDATE", entityPM.Tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            };
            TicketEscalationUpdateService service = new TicketEscalationUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("TicketEscalation", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }

        public List<TicketEscalationList> GetTicketEscalationListsByTicketId( string ticketId,int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketEscalationListQueryService listService = new TicketEscalationListQueryService(crmContext);
            List<TicketEscalationList> myResult = listService.GetTicketEscalationListByTicketId(ticketId, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("TicketEscalation", tenant, myResult.Cast<object>().ToList());
            return myResult.ToList();
        }

        #endregion 

        #region EscalationActionTimeIndicator

        public EscalationActionTimeIndicatorPM GetSingleEscalationActionTimeIndicatorPM(string code,int tenant)
        {
            if (crmContext == null)
            {
                  crmContext = CRMContext.GetContext(tenant);
            }

            EscalationActionTimeIndicatorQueryService escalationActionTimeIndicatorQuery = new EscalationActionTimeIndicatorQueryService(crmContext);
            EscalationActionTimeIndicatorPM escalationActionTimeIndicatorPM = escalationActionTimeIndicatorQuery.GetSingle(code,false,false);
            return escalationActionTimeIndicatorPM;
           
        }

		public EscalationActionTimeIndicatorList GetSingleEscalationActionTimeIndicatorList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if ( crmContext == null)
            {
                 crmContext = CRMContext.GetContext(tenant);
            }

          
            EscalationActionTimeIndicatorListQueryService listService = new EscalationActionTimeIndicatorListQueryService(crmContext);
            return listService.GetSingle(code);
        }

		public List<EscalationActionTimeIndicatorList> GetEscalationActionTimeIndicatorLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if ( crmContext == null)
            {
                  crmContext = CRMContext.GetContext(tenant);
            }
            EscalationActionTimeIndicatorListQueryService listService = new EscalationActionTimeIndicatorListQueryService(crmContext);
            return listService.GetList(tenant);
        }
       
	    public List<EscalationActionTimeIndicatorList> GetEscalationActionTimeIndicatorsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if ( crmContext == null)
            {
                  crmContext = CRMContext.GetContext(tenant);
            };
            EscalationActionTimeIndicatorListQueryService listService = new EscalationActionTimeIndicatorListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetEscalationActionTimeIndicatorFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if ( crmContext == null)
            {
                  crmContext = CRMContext.GetContext(tenant);
            };
            EscalationActionTimeIndicatorListQueryService queryService = new EscalationActionTimeIndicatorListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }

        #endregion 

        #region CorrespondencesAttachment

        public CorrespondencesAttachmentPM GetSingleCorrespondencesAttachmentPM(string id, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CorrespondencesAttachmentQueryService correspondencesAttachmentQuery = new CorrespondencesAttachmentQueryService(crmContext);
            CorrespondencesAttachmentPM correspondencesAttachmentPM = correspondencesAttachmentQuery.GetSingle(id, false, false);
            return correspondencesAttachmentPM;

        }

        public CorrespondencesAttachmentList GetSingleCorrespondencesAttachmentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }


            CorrespondencesAttachmentListQueryService listService = new CorrespondencesAttachmentListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<CorrespondencesAttachmentList> GetCorrespondencesAttachmentLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            CorrespondencesAttachmentListQueryService listService = new CorrespondencesAttachmentListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<CorrespondencesAttachmentList> GetCorrespondencesAttachmentsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            CorrespondencesAttachmentListQueryService listService = new CorrespondencesAttachmentListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCorrespondencesAttachmentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            CorrespondencesAttachmentListQueryService queryService = new CorrespondencesAttachmentListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertCorrespondencesAttachment(CorrespondencesAttachmentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            };
            CorrespondencesAttachmentUpdateService service = new CorrespondencesAttachmentUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CorrespondencesAttachment", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }

        public void UpdateCorrespondencesAttachment(CorrespondencesAttachmentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            };
            CorrespondencesAttachmentUpdateService service = new CorrespondencesAttachmentUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CorrespondencesAttachment", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        } 

        #endregion 

        #region Ticket Types
        public TicketCreatedByTypePM GetSingleTicketCreatedByTypePM(string code, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketCreatedByTypeQueryService ticketCreatedByTypeQuery = new TicketCreatedByTypeQueryService(crmContext);
            TicketCreatedByTypePM ticketCreatedByTypePM = ticketCreatedByTypeQuery.GetSingle(code, false, false);
            return ticketCreatedByTypePM;

        }


        public TicketCreatedByTypeList GetSingleTicketCreatedByTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }


            TicketCreatedByTypeListQueryService listService = new TicketCreatedByTypeListQueryService(crmContext);
            return listService.GetSingle(code);
        }

        public List<TicketCreatedByTypeList> GetTicketCreatedByTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            TicketCreatedByTypeListQueryService listService = new TicketCreatedByTypeListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<TicketCreatedByTypeList> GetTicketCreatedByTypesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            TicketCreatedByTypeListQueryService listService = new TicketCreatedByTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTicketCreatedByTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            TicketCreatedByTypeListQueryService queryService = new TicketCreatedByTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
        #endregion 

        #region Ticket Sources 
        
        public TicketSourcePM GetSingleTicketSourcePM(string code, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            TicketSourceQueryService ticketSourceQuery = new TicketSourceQueryService(crmContext);
            TicketSourcePM ticketSourcePM = ticketSourceQuery.GetSingle(code, false, false);
            return ticketSourcePM;

        }


        public TicketSourceList GetSingleTicketSourceList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }


            TicketSourceListQueryService listService = new TicketSourceListQueryService(crmContext);
            return listService.GetSingle(code);
        }

        public List<TicketSourceList> GetTicketSourceLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            TicketSourceListQueryService listService = new TicketSourceListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<TicketSourceList> GetTicketSourcesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            TicketSourceListQueryService listService = new TicketSourceListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTicketSourceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            };
            TicketSourceListQueryService queryService = new TicketSourceListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

        #endregion 

    }
}


