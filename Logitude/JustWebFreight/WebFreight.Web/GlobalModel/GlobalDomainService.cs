using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.CustomFilters;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.GlobalModel.Tools.EntityService;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.Helpers;
using Logitude.BL.GlobalModel;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Simplog.Data.Helpers;
using WebFreight.Web.SystemLogsModel.Queries;

namespace WebFreight.Web.GlobalModel
{
    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public partial class GlobalDomainService : LogitudeDomainService
    {
        private IGlobalContext objectContext;
        private GlobalContactRepository globalContactsRepository;
        private GlobalTenantRepository globalTenantsRepository;
        private GlobalDBRepository globalDBsRepository;
        private PerformanceLogRepository performanceLogsRepository;
        private ConvertProgramInfoRepository convertProgramInfoRepository;
        private LogitudeLeadRepository leadRepository;
        private AnalyzeQueueRepository analyzeQueueRepository;
        private RecurringPeriodRepository recurringPeriodRepository;
        private PaymentChannelRepository paymentChannelRepository;
        private PaymentMethodRepository paymentMethodRepository;
        private PaymentCurrencyRepository paymentCurrencyRepository;
        private LogitudeLeadRepository logitudeLeadRepository;
        private SettingRepository settingRepository;
        private BatchServicesDefinitionRepository batchServicesDefinitionRepository;
        private TenantTypeRepository tenantTypeRepository;

        private RecurringPeriodQuery recurringPeriodQuery;
        private PaymentChannelQuery paymentChannelQuery;
        private PaymentMethodQuery paymentMethodQuery;
        private PaymentCurrencyQuery paymentCurrencyQuery;
        private LogitudeLeadQuery logitudeLeadQurey;
        private SettingQuery settingQuery;
        private BatchServicesDefinitionQuery batchServicesDefinitionQuery;
        private TenantTypeQuery tenantTypeQuery;

        private UserData currentUser;
        public UserData CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        public GlobalDomainService(UserData theCurrentUser)
        {
            CurrentUser = theCurrentUser;
            objectContext = GlobalContext.GetContext();

            globalTenantsRepository = new GlobalTenantRepository(objectContext);
            globalContactsRepository = new GlobalContactRepository(objectContext);
            globalDBsRepository = new GlobalDBRepository(objectContext);
            performanceLogsRepository = new PerformanceLogRepository(objectContext);
            convertProgramInfoRepository = new ConvertProgramInfoRepository(objectContext);
            tenantManagementRepository = new TenantManagementRepository(objectContext);
            analyzeQueueRepository = new AnalyzeQueueRepository(objectContext);
            logitudeLeadRepository = new LogitudeLeadRepository(objectContext);
            settingRepository = new SettingRepository(objectContext);
            batchServicesDefinitionRepository = new BatchServicesDefinitionRepository(objectContext);
            tenantTypeRepository = new TenantTypeRepository(objectContext);
        }

        public GlobalDomainService()
        {
            objectContext = GlobalContext.GetContext();

            globalTenantsRepository = new GlobalTenantRepository(objectContext);
            globalContactsRepository = new GlobalContactRepository(objectContext);
            globalDBsRepository = new GlobalDBRepository(objectContext);
            performanceLogsRepository = new PerformanceLogRepository(objectContext);
            convertProgramInfoRepository = new ConvertProgramInfoRepository(objectContext);
            leadRepository = new LogitudeLeadRepository(objectContext);
            tenantManagementRepository = new TenantManagementRepository(objectContext);
            analyzeQueueRepository = new AnalyzeQueueRepository(objectContext);
            logitudeLeadRepository = new LogitudeLeadRepository(objectContext);
            settingRepository = new SettingRepository(objectContext);
            batchServicesDefinitionRepository = new BatchServicesDefinitionRepository(objectContext);
            tenantTypeRepository = new TenantTypeRepository(objectContext);
        }

        public GlobalDomainService(IGlobalContext context)
        {
            globalTenantsRepository = new GlobalTenantRepository(context);
            globalContactsRepository = new GlobalContactRepository(context);
            globalDBsRepository = new GlobalDBRepository(context);
            performanceLogsRepository = new PerformanceLogRepository(context);
            convertProgramInfoRepository = new ConvertProgramInfoRepository(context);
            leadRepository = new LogitudeLeadRepository(context);
            tenantManagementRepository = new TenantManagementRepository(context);
            analyzeQueueRepository = new AnalyzeQueueRepository(context);
            logitudeLeadRepository = new LogitudeLeadRepository(context);
            settingRepository = new SettingRepository(context);
            batchServicesDefinitionRepository = new BatchServicesDefinitionRepository(context);
            tenantTypeRepository = new TenantTypeRepository(context);
        }

        #region GlobalContacts
        public GlobalContact GetGlobalContactByEmailAndTenant2(string email, int tenant)
        {
            return globalContactsRepository.GetGlobalContactByEmailAndTenant(email, tenant);
        }

        public IQueryable<GlobalContact> GetGlobalContactsByEmail(string email)
        {
            return globalContactsRepository.GetContactByEmail(email);
        }

        public IQueryable<GlobalContact> GetGlobalContacts()
        {
            return globalContactsRepository.GetGlobalContactByTenant(0);
        }

        public IQueryable<GlobalContact> GetGlobalContactsByTenant(int tenant)
        {
            return globalContactsRepository.GetGlobalContactByTenant(tenant);
        }

        public GlobalContact GetContact(string email)
        {
            GlobalContact contact = this.objectContext.GlobalContacts.Where(u => u.Email == email).FirstOrDefault();
            return contact;
        }

        public List<GlobalTenant> GetTenantsForContactsByEmail(string email)
        {
            List<GlobalContact> contacts = globalContactsRepository.GetContactByEmail(email).ToList();
            List<ContactTenant> contactTenants = new List<ContactTenant>();
            List<GlobalTenant> tenants = new List<GlobalTenant>();

            foreach (GlobalContact contact in contacts)
            {
                tenants.Add(contact.GlobalTenant);
            }

            return tenants;
        }

        public List<GlobalTenant> GetTenantsForContactsByEmailForCustomerCare(int tenant)
        {
            //List<GlobalContact> contacts = globalContactsRepository.GetContactByEmail(email).ToList();
            //List<ContactTenant> contactTenants = new List<ContactTenant>();
            List<GlobalTenant> tenants = new List<GlobalTenant>();

            //foreach (GlobalContact contact in contacts)
            //{
            //    tenants.Add(contact.GlobalTenant);
            //}
            globalTenantsRepository = new GlobalTenantRepository();
            GlobalTenant globaltenant = globalTenantsRepository.GetGlobalTenantsByTenant(tenant);
            tenants.Add(globaltenant);
            return tenants;
        }

        public void InsertGlobalContact(GlobalContact entity)
        {
            entity.Id = IdCounter.GetNumber("Contact", entity.GlobalTenantId).ToString();
            globalContactsRepository.Add(entity);
        }

        public void UpdateGlobalContact(GlobalContact currentEntity)
        {
            globalContactsRepository.Update(currentEntity);
        }

        public void DeleteGlobalContact(GlobalContact entity)
        {
            globalContactsRepository.Remove(entity);
        }
        #endregion 

        #region GlobalTenants
        public GlobalTenant GetGlobalTenants()
        {
            return globalTenantsRepository.GetGlobalTenantsByTenant(0);
        }

        public GlobalTenant GetGlobalTenantsByTenant(int tenant)
        {
            return globalTenantsRepository.GetGlobalTenantsByTenant(tenant);
        }

        public void InsertGlobalTenant(GlobalTenant entity)
        {
            globalTenantsRepository.Add(entity);
        }

        public void UpdateGlobalTenant(GlobalTenant currentEntity)
        {
            globalTenantsRepository.Update(currentEntity);
        }

        public void DeleteGlobalTenant(GlobalTenant entity)
        {
            globalTenantsRepository.Remove(entity);
        }
        #endregion 

        #region GlobalDBs
        public IQueryable<GlobalDB> GetGlobalDBs()
        {
            return globalDBsRepository.GetGlobalDBs();
        }

        public void InsertGlobalTenant(GlobalDB entity)
        {
            globalDBsRepository.Add(entity);
        }

        public void UpdateGlobalTenant(GlobalDB currentEntity)
        {
            globalDBsRepository.Update(currentEntity);
        }

        public void DeleteGlobalTenant(GlobalDB entity)
        {
            globalDBsRepository.Remove(entity);
        }
        #endregion 

        #region ConvertProgramInfos
        public IQueryable<ConvertProgramInfo> GetConvertProgramInfos()
        {
            return convertProgramInfoRepository.GetConvertProgramInfoes();
        }

        public void InsertGlobalTenant(ConvertProgramInfo entity)
        {
            convertProgramInfoRepository.Add(entity);
        }

        public void UpdateGlobalTenant(ConvertProgramInfo currentEntity)
        {
            convertProgramInfoRepository.Update(currentEntity);
        }

        public void DeleteGlobalTenant(ConvertProgramInfo entity)
        {
            convertProgramInfoRepository.Remove(entity);
        }
        #endregion 

        #region PerformanceLogs
        public IQueryable<PerformanceLog> GetPerformanceLogs()
        {
            return performanceLogsRepository.GetPerformanceLogs();
        }

        public void InsertPerformanceLog(PerformanceLog entity)
        {
            if (Environment.CommandLine.ToLower().Contains("iisexpress.exe"))
            {
                return;
            }

            try
            {
                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                if (string.IsNullOrEmpty(currentIP))
                {
                    currentIP = HttpContext.Current.Request.UserHostAddress;
                } 
                entity.UserIP = currentIP;
                PerformanceLog log = performanceLogsRepository.GetSinglePerformanceLog(entity.Id);
                if (log == null)
                {
                    if (String.IsNullOrWhiteSpace(entity.Email))
                    {
                        entity.Email = AuthenticationUtil.ResolveUserIdentityName(entity.Tenant);
                    }

                    try
                    {
                        entity.LogDateTimeGMT = DateTime.UtcNow;
                        performanceLogsRepository.Add(entity);
                        performanceLogsRepository.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.Message.Contains("Violation of PRIMARY KEY constraint") || ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                            {
                                entity.Id = Guid.NewGuid().ToString();
                                performanceLogsRepository.SubmitChanges();
                            }
                        }
                        else
                            throw ex;
                        //Cannot insert duplicate key in object 

                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("Violation of PRIMARY KEY constraint") || ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                    {
                        entity.Id = Guid.NewGuid().ToString();
                        performanceLogsRepository.SubmitChanges();
                    }
                }
                else
                    throw ex;
                //Cannot insert duplicate key in object 

            }
        }

        public void UpdatePerformanceLog(PerformanceLog currentEntity)
        {
            performanceLogsRepository.Update(currentEntity);
        }

        public void DeletePerformanceLog(PerformanceLog entity)
        {
            performanceLogsRepository.Remove(entity);
        }
        #endregion 

        #region AnalyzeQueue
        [RequiresAuthentication]
        [Query(IsDefault = true)]

        public IQueryable<AnalyzeQueue> GetAnalyzeQueues()
        {
            SecurityUtility.CheckContactFeature("AnalyzeQueue", "READ", 0);
            return analyzeQueueRepository.GetAllAnalyzeQueues();
        }

        AnalyzeQueueQuery analyzeQueueQuery;
        public AnalyzeQueuePM GetSingleAnalyzeQueuePM(string id, int tenant)
        {
            analyzeQueueQuery = new AnalyzeQueueQuery();
            return analyzeQueueQuery.GetSinglePM(id, tenant);
        }

        public void UpdateAnalyzeQueueList(AnalyzeQueueList currentEntity)
        {
        }

        public void MapAnalyzeQueueAnalyzeQueuePM(AnalyzeQueuePM analyzeQueuePm, AnalyzeQueue analyzeQueue)
        {
            analyzeQueue.From = analyzeQueuePm.From;
            analyzeQueue.FileSize = analyzeQueuePm.FileSize;
            analyzeQueue.ErrorMessage = analyzeQueuePm.ErrorMessage;
            analyzeQueue.CreateDate = analyzeQueuePm.CreateDate;
            analyzeQueue.ConnectedToTenant = analyzeQueuePm.ConnectedToTenant;
            analyzeQueue.ConnectedToTenant = analyzeQueuePm.ConnectedToEntity;
            analyzeQueue.CommunicationLogId = analyzeQueuePm.CommunicationLogId;
            analyzeQueue.Retries = analyzeQueuePm.Retries;
            analyzeQueue.Status = analyzeQueuePm.Status;
            analyzeQueue.Subject = analyzeQueuePm.Subject;
            analyzeQueue.AWBNumber = analyzeQueuePm.AWBNumber;
            analyzeQueue.SearchFields = analyzeQueuePm.Status + "," + analyzeQueuePm.Subject + "," + analyzeQueuePm.Tenant + "," + analyzeQueuePm.From + "," + analyzeQueuePm.CreateDate + "," + analyzeQueuePm.EntityReference + "," + analyzeQueuePm.ObjectTableName;
            analyzeQueue.AckReason = analyzeQueuePm.AckReason;
            analyzeQueue.DoneDate = analyzeQueuePm.DoneDate;
        }

        public void InsertAnalyzeQueuePM(AnalyzeQueuePM analyzeQueue)
        {
        }

        public void UpdateAnalyzeQueuePM(AnalyzeQueuePM currentAnalyzeQueue)
        {
            string entityName = "AnalyzeQueue" + currentAnalyzeQueue.Id;
            string entityPmName = "AnalyzeQueuePM" + currentAnalyzeQueue.Id;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            AnalyzeQueue entity = analyzeQueueRepository.GetSingleAnalyzeQueue(currentAnalyzeQueue.Id, currentAnalyzeQueue.Tenant);
            MapAnalyzeQueueAnalyzeQueuePM(currentAnalyzeQueue, entity);
            analyzeQueueRepository.Update(entity);

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ContactRepository contactsRepository = new ContactRepository(currentAnalyzeQueue.Tenant);
                ContactQuery contactQuery = new ContactQuery(contactsRepository);
                ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), 0, true);

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = 0,
                    EventTypeCode = "UPAQ",
                    UserId = contact.Id,
                    EntityId = entity.Id.ToString(),
                    ObjectTableName = "AnalyzeQueue",
                });

                TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "AnalyzeQueue");
                scope.Complete();
            }
        }

        public void DeleteAnalyzeQueuePM(AnalyzeQueuePM analyzeQueue)
        {
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AnalyzeQueueList> GetAnalyzeQueueFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AnalyzeQueue", "READ", tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AnalyzeQueue> iQueryable = analyzeQueueRepository.GetAllAnalyzeQueues();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AnalyzeQueue>(nonListQueryOperation, iQueryable);
            int skippedPorts = queryOperations.PageIndex;

            var query2 = from a in iQueryable.Include("AnalyzeQueueStatus").Include("TenantManagement")
                         select new AnalyzeQueueList()
                         {
                             Id = a.Id,
                             From = a.From,
                             CreateDate = a.CreateDate,
                             ErrorMessage = a.ErrorMessage,
                             Retries = a.Retries,
                             Status = a.AnalyzeQueueStatus.Name,
                             Subject = a.Subject,
                             FileSize = a.FileSize,
                             ConnectedToTenant = a.ConnectedToTenant,
                             ConnectedToEntity = a.ConnectedToEntity,
                             CommunicationLogId = a.CommunicationLogId,
                             SearchFields = a.SearchFields,
                             EntityReference = a.EntityReference,
                             ObjectTableName = a.ObjectTableName,
                             TenantName = a.TenantManagement == null ? null : a.TenantManagement.Name,
                             Tenant = a.Tenant,
                             AWBNumber = a.AWBNumber,
                             AckReason = a.AckReason,
                             DoneDate = a.DoneDate,
                         };

            query2 = filter.GetFilteredQuery<AnalyzeQueueList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AnalyzeQueueList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> objectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AnalyzeQueue", tenant).ToList();

                ObjectField objectField = (from a in objectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyzeQueueList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyzeQueueList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyzeQueueList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyzeQueueList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyzeQueueList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAnalyzeQueueFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AnalyzeQueue", "READ", tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AnalyzeQueue> iQueryable = analyzeQueueRepository.GetAllAnalyzeQueues();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AnalyzeQueue>(nonListQueryOperation, iQueryable);
            CommunicationLogRepository commLogRep = new CommunicationLogRepository(tenant);
            var query2 = from a in iQueryable.Include("AnalyzeQueueStatus").Include("TenantManagement")
                         select new AnalyzeQueueList()
                         {
                             Id = a.Id,
                             From = a.From,
                             CreateDate = a.CreateDate,
                             ErrorMessage = a.ErrorMessage,
                             Retries = a.Retries,
                             Status = a.AnalyzeQueueStatus.Name,
                             Subject = a.Subject,
                             FileSize = a.FileSize,
                             ConnectedToTenant = a.ConnectedToTenant,
                             ConnectedToEntity = a.ConnectedToEntity,
                             CommunicationLogId = a.CommunicationLogId,
                             SearchFields = a.SearchFields,
                             EntityReference = a.EntityReference,
                             ObjectTableName = a.ObjectTableName,
                             Tenant = a.Tenant,
                             TenantName = a.TenantManagement == null ? null : a.TenantManagement.Name,
                             AWBNumber = a.AWBNumber,
                             AckReason = a.AckReason,
                             DoneDate = a.DoneDate,

                         };


            query2 = filter.GetFilteredQuery<AnalyzeQueueList>(listQueryOperation, query2);

            int count = query2.Take(1001).Count();
            return count;
        }

        public IQueryable<AnalyzeQueueList> GetAnalyzeQueueLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AnalyzeQueue", "READ", tenant);
            IQueryable<AnalyzeQueue> tenants = analyzeQueueRepository.GetAllAnalyzeQueues();

            var query2 = from a in tenants
                         select new AnalyzeQueueList()
                         {
                             Id = a.Id,
                             From = a.From,
                             CreateDate = a.CreateDate,
                             ErrorMessage = a.ErrorMessage,
                             Retries = a.Retries,
                             Status = a.AnalyzeQueueStatus.Name,
                             Subject = a.Subject,
                             FileSize = a.FileSize,
                             ConnectedToTenant = a.ConnectedToTenant,
                             ConnectedToEntity = a.ConnectedToEntity,
                             CommunicationLogId = a.CommunicationLogId,
                             SearchFields = a.SearchFields,
                             Tenant = a.Tenant,
                             TenantName = a.TenantManagement == null ? null : a.TenantManagement.Name,
                             AWBNumber = a.AWBNumber,
                             AckReason = a.AckReason,
                             DoneDate = a.DoneDate,
                             EntityReference = a.EntityReference,
                         };

            return query2;
        }

        public IQueryable<AnalyzeQueuePM> GetAnalyzeQueuePMs(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AnalyzeQueue", "READ", tenant);
            analyzeQueueQuery = new AnalyzeQueueQuery();
            return analyzeQueueQuery.GetAnalyzeQueuePMsByTenant(tenant);
        }

        public AnalyzeQueueList GetSingleAnalyzeQueueList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            analyzeQueueRepository = new AnalyzeQueueRepository();

            AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetSingleAnalyzeQueue(id, tenant);

            if (analyzeQueue != null)
            {
                AnalyzeQueueList analyzeQueueList = new AnalyzeQueueList()
                {
                    Id = analyzeQueue.Id,
                    From = analyzeQueue.From,
                    CreateDate = analyzeQueue.CreateDate,
                    ErrorMessage = analyzeQueue.ErrorMessage,
                    Retries = analyzeQueue.Retries,
                    Status = analyzeQueue.AnalyzeQueueStatus.Name,
                    Subject = analyzeQueue.Subject,
                    FileSize = analyzeQueue.FileSize,
                    ConnectedToTenant = analyzeQueue.ConnectedToTenant,
                    ConnectedToEntity = analyzeQueue.ConnectedToEntity,
                    CommunicationLogId = analyzeQueue.CommunicationLogId,
                    SearchFields = analyzeQueue.SearchFields,
                    Tenant = analyzeQueue.Tenant,
                    TenantName = analyzeQueue.TenantManagement == null ? null : analyzeQueue.TenantManagement.Name,
                    AWBNumber = analyzeQueue.AWBNumber,
                    AckReason = analyzeQueue.AckReason,
                    DoneDate = analyzeQueue.DoneDate,
                    EntityReference = analyzeQueue.EntityReference
                };
                return analyzeQueueList;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region RecurringPeriod
        public IQueryable<RecurringPeriod> GetRecurringPeriods(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            recurringPeriodRepository = new RecurringPeriodRepository(tenant);
            return recurringPeriodRepository.GetRecurringPeriods();
        }

        public RecurringPeriodPM GetSingleRecurringPeriodPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            recurringPeriodQuery = new RecurringPeriodQuery(tenant);
            return recurringPeriodQuery.GetSingleRecurringPeriodPM(code);
        }

        public RecurringPeriodList GetSingleRecurringPeriodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            recurringPeriodRepository = new RecurringPeriodRepository(tenant);
            recurringPeriodQuery = new RecurringPeriodQuery(recurringPeriodRepository);

            RecurringPeriodList recurringPeriodList = null;
            RecurringPeriod recurringPeriod = recurringPeriodRepository.GetSingleRecurringPeriod(code);

            if (recurringPeriod != null)
            {
                List<RecurringPeriod> singleEntityList = new List<RecurringPeriod>();
                singleEntityList.Add(recurringPeriod);

                IQueryable<RecurringPeriod> iQueryable = singleEntityList.AsQueryable();
                IQueryable<RecurringPeriodList> iQueryableEntityList = recurringPeriodQuery.GetIQueryableEntityList(iQueryable);
                recurringPeriodList = iQueryableEntityList.FirstOrDefault();
            }
            return recurringPeriodList;
        }

        public IQueryable<RecurringPeriodList> GetRecurringPeriodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            recurringPeriodRepository = new RecurringPeriodRepository(tenant);
            recurringPeriodQuery = new RecurringPeriodQuery(recurringPeriodRepository);

            IQueryable<RecurringPeriod> iQueryable = recurringPeriodRepository.GetRecurringPeriods();
            IQueryable<RecurringPeriodList> query2 = recurringPeriodQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<RecurringPeriodList> GetRecurringPeriodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            recurringPeriodRepository = new RecurringPeriodRepository(tenant);
            recurringPeriodQuery = new RecurringPeriodQuery(recurringPeriodRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<RecurringPeriod> iQueryable = recurringPeriodRepository.GetRecurringPeriods();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RecurringPeriod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<RecurringPeriodList> query2 = recurringPeriodQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<RecurringPeriodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(RecurringPeriodList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("RecurringPeriod", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<RecurringPeriodList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RecurringPeriodList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RecurringPeriodList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RecurringPeriodList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RecurringPeriodList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetRecurringPeriodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            recurringPeriodRepository = new RecurringPeriodRepository(tenant);
            recurringPeriodQuery = new RecurringPeriodQuery(recurringPeriodRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<RecurringPeriod> iQueryable = recurringPeriodRepository.GetRecurringPeriods();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RecurringPeriod>(nonListQueryOperation, iQueryable);

            IQueryable<RecurringPeriodList> query2 = recurringPeriodQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<RecurringPeriodList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertRecurringPeriod(RecurringPeriod entity)
        {
            recurringPeriodRepository.Add(entity);
        }

        public void UpdateRecurringPeriod(RecurringPeriod currentEntity)
        {
            recurringPeriodRepository.Update(currentEntity);
        }

        public void DeleteRecurringPeriod(RecurringPeriod entity)
        {
            recurringPeriodRepository.Remove(entity);
        }
        #endregion

        #region PaymentChannel
        public IQueryable<PaymentChannel> GetPaymentChannels(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentChannelRepository = new PaymentChannelRepository(tenant);
            return paymentChannelRepository.GetPaymentChannels();
        }

        public PaymentChannelPM GetSinglePaymentChannelPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentChannelQuery = new PaymentChannelQuery(tenant);
            return paymentChannelQuery.GetSinglePaymentChannelPM(code);
        }

        public PaymentChannelList GetSinglePaymentChannelList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentChannelRepository = new PaymentChannelRepository(tenant);
            paymentChannelQuery = new PaymentChannelQuery(paymentChannelRepository);

            PaymentChannelList paymentChannelList = null;
            PaymentChannel paymentChannel = paymentChannelRepository.GetSinglePaymentChannel(code);

            if (paymentChannel != null)
            {
                List<PaymentChannel> singleEntityList = new List<PaymentChannel>();
                singleEntityList.Add(paymentChannel);

                IQueryable<PaymentChannel> iQueryable = singleEntityList.AsQueryable();
                IQueryable<PaymentChannelList> iQueryableEntityList = paymentChannelQuery.GetIQueryableEntityList(iQueryable);
                paymentChannelList = iQueryableEntityList.FirstOrDefault();
            }
            return paymentChannelList;
        }

        public IQueryable<PaymentChannelList> GetPaymentChannelLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentChannelRepository = new PaymentChannelRepository(tenant);
            paymentChannelQuery = new PaymentChannelQuery(paymentChannelRepository);

            IQueryable<PaymentChannel> iQueryable = paymentChannelRepository.GetPaymentChannels();
            IQueryable<PaymentChannelList> query2 = paymentChannelQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PaymentChannelList> GetPaymentChannelFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentChannelRepository = new PaymentChannelRepository(tenant);
            paymentChannelQuery = new PaymentChannelQuery(paymentChannelRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PaymentChannel> iQueryable = paymentChannelRepository.GetPaymentChannels();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentChannel>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PaymentChannelList> query2 = paymentChannelQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PaymentChannelList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PaymentChannelList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PaymentChannel", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentChannelList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentChannelList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentChannelList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentChannelList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentChannelList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetPaymentChannelFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentChannelRepository = new PaymentChannelRepository(tenant);
            paymentChannelQuery = new PaymentChannelQuery(paymentChannelRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PaymentChannel> iQueryable = paymentChannelRepository.GetPaymentChannels();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentChannel>(nonListQueryOperation, iQueryable);

            IQueryable<PaymentChannelList> query2 = paymentChannelQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PaymentChannelList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPaymentChannel(PaymentChannel entity)
        {
            paymentChannelRepository.Add(entity);
        }

        public void UpdatePaymentChannel(PaymentChannel currentEntity)
        {
            paymentChannelRepository.Update(currentEntity);
        }

        public void DeletePaymentChannel(PaymentChannel entity)
        {
            paymentChannelRepository.Remove(entity);
        }
        #endregion

        #region PaymentMethod
        public IQueryable<PaymentMethod> GetPaymentMethods(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new PaymentMethodRepository(tenant);
            return paymentMethodRepository.GetPaymentMethods();
        }

        public PaymentMethodPM GetSinglePaymentMethodPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodQuery = new PaymentMethodQuery(tenant);
            return paymentMethodQuery.GetSinglePaymentMethodPM(code);
        }

        public PaymentMethodList GetSinglePaymentMethodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new PaymentMethodRepository(tenant);
            paymentMethodQuery = new PaymentMethodQuery(paymentMethodRepository);

            PaymentMethodList paymentMethodList = null;
            PaymentMethod paymentMethod = paymentMethodRepository.GetSinglePaymentMethod(code);

            if (paymentMethod != null)
            {
                List<PaymentMethod> singleEntityList = new List<PaymentMethod>();
                singleEntityList.Add(paymentMethod);

                IQueryable<PaymentMethod> iQueryable = singleEntityList.AsQueryable();
                IQueryable<PaymentMethodList> iQueryableEntityList = paymentMethodQuery.GetIQueryableEntityList(iQueryable);
                paymentMethodList = iQueryableEntityList.FirstOrDefault();
            }
            return paymentMethodList;
        }

        public IQueryable<PaymentMethodList> GetPaymentMethodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new PaymentMethodRepository(tenant);
            paymentMethodQuery = new PaymentMethodQuery(paymentMethodRepository);

            IQueryable<PaymentMethod> iQueryable = paymentMethodRepository.GetPaymentMethods();
            IQueryable<PaymentMethodList> query2 = paymentMethodQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PaymentMethodList> GetPaymentMethodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new PaymentMethodRepository(tenant);
            paymentMethodQuery = new PaymentMethodQuery(paymentMethodRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PaymentMethod> iQueryable = paymentMethodRepository.GetPaymentMethods();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentMethod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PaymentMethodList> query2 = paymentMethodQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PaymentMethodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PaymentMethodList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PaymentMethod", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentMethodList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentMethodList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentMethodList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentMethodList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentMethodList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetPaymentMethodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new PaymentMethodRepository(tenant);
            paymentMethodQuery = new PaymentMethodQuery(paymentMethodRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PaymentMethod> iQueryable = paymentMethodRepository.GetPaymentMethods();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentMethod>(nonListQueryOperation, iQueryable);

            IQueryable<PaymentMethodList> query2 = paymentMethodQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PaymentMethodList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPaymentMethod(PaymentMethod entity)
        {
            paymentMethodRepository.Add(entity);
        }

        public void UpdatePaymentMethod(PaymentMethod currentEntity)
        {
            paymentMethodRepository.Update(currentEntity);
        }

        public void DeletePaymentMethod(PaymentMethod entity)
        {
            paymentMethodRepository.Remove(entity);
        }
        #endregion

        #region LogitudeLead

        public IQueryable<LogitudeLead> GetLogitudeLeads(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeLead", "READ", tenant);
            LogitudeLeadRepository logitudeLeadRepository;
            logitudeLeadRepository = new LogitudeLeadRepository();
            return logitudeLeadRepository.GetAllLogitudeLeads();
        }

        public LogitudeLeadPM GetLogitudeLeadById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeLead", "READ", tenant);
            DepartmentRepository departmentRepository;
            departmentRepository = new DepartmentRepository(tenant);
            LogitudeLeadQuery logitudeLeadQuery = new LogitudeLeadQuery();
            LogitudeLeadPM logitudeLead = logitudeLeadQuery.GetSinglePM(id);
            return logitudeLead;
        }

        public LogitudeLeadList GetSingleLogitudeLeadList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeLead", "READ", tenant);
            LogitudeLeadRepository LogitudeLeadRepository;
            LogitudeLeadRepository = new LogitudeLeadRepository();
            LogitudeLeadQuery LogitudeLeadQuery = new LogitudeLeadQuery(LogitudeLeadRepository);
            LogitudeLeadList LogitudeLeadList = null;
            LogitudeLead LogitudeLead = LogitudeLeadRepository.GetSingleLogitudeLead(id);

            if (LogitudeLead != null)
            {
                List<LogitudeLead> singleEntityList = new List<LogitudeLead>();
                singleEntityList.Add(LogitudeLead);

                IQueryable<LogitudeLead> iQueryable = singleEntityList.AsQueryable();
                IQueryable<LogitudeLeadList> iQueryableEntityList = LogitudeLeadQuery.GetIQueryableEntityList(iQueryable);
                LogitudeLeadList = iQueryableEntityList.FirstOrDefault();
            }
            return LogitudeLeadList;
        }

        public void InsertLogitudeLead(LogitudeLeadPM currentLogitudeLead)
        {
            SecurityUtility.CheckContactFeature("LogitudeLead", "NEW", currentLogitudeLead.TenantNumber);

            LogitudeLeadService service = new LogitudeLeadService(objectContext);
            service.Create(currentLogitudeLead);

            TableLastUpdateClass.UpdateTableHistory(currentLogitudeLead.TenantNumber, "LogitudeLead");



        }

        public void UpdateLogitudeLead(LogitudeLeadPM currentLogitudeLead)
        {
            SecurityUtility.CheckContactFeature("LogitudeLead", "UPDATE", currentLogitudeLead.TenantNumber);

            if (objectContext == null)
            {
                ///  objectContext = GlobalContext.GetContext(currentLogitudeLead.TenantNumber);
                ///  
                objectContext = GlobalContext.GetContext();
            }

            string entityName = "LogitudeLead" + currentLogitudeLead.Id + currentLogitudeLead.TenantNumber;
            string entityPmName = "LogitudeLeadPM" + currentLogitudeLead.Id + currentLogitudeLead.TenantNumber;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            LogitudeLeadService service = new LogitudeLeadService(objectContext);
            service.Update(currentLogitudeLead);
            TableLastUpdateClass.UpdateTableHistory(currentLogitudeLead.TenantNumber, "LogitudeLead");

        }

        public void DeleteLogitudeLead(LogitudeLeadPM LogitudeLead)
        {
            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }
            LogitudeLeadRepository LogitudeLeadRepository;
            LogitudeLeadRepository = new LogitudeLeadRepository(objectContext);
            //
            LogitudeLead entity = LogitudeLeadRepository.GetSingleLogitudeLead(LogitudeLead.Id);
            LogitudeLeadRepository.Remove(entity);
        }

        public LogitudeLeadPM GetSingleLogitudeLead(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            LogitudeLeadRepository LogitudeLeadsRepository = new LogitudeLeadRepository(tenant);
            LogitudeLeadQuery LogitudeLeadQuery = new LogitudeLeadQuery(LogitudeLeadsRepository);
            return LogitudeLeadQuery.GetSingleLogitudeLeadPM(id, tenant);
        }

        /// <summary>
        public IQueryable<LogitudeLeadList> GetLogitudeLeadLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            LogitudeLeadRepository LogitudeLeadsRepository = new LogitudeLeadRepository(tenant);
            IQueryable<LogitudeLead> LogitudeLeads = LogitudeLeadsRepository.GetLogitudeLeadsByTenant(tenant);
            LogitudeLeadQuery LogitudeLeadQuery = new LogitudeLeadQuery(LogitudeLeadsRepository);
            IQueryable<LogitudeLeadList> query2 = LogitudeLeadQuery.GetIQueryableEntityList(LogitudeLeads);
            return query2;

        }

        [Query(HasSideEffects = true)]
        public IQueryable<LogitudeLeadList> GetLogitudeLeadFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            LogitudeLeadRepository LogitudeLeadsRepository = new LogitudeLeadRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<LogitudeLead> LogitudeLeads = LogitudeLeadsRepository.GetLogitudeLeadsByTenant(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            LogitudeLeads = filter.GetFilteredQuery<LogitudeLead>(nonListQueryOperation, LogitudeLeads);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            LogitudeLeadQuery LogitudeLeadQuery = new LogitudeLeadQuery(LogitudeLeadsRepository);
            IQueryable<LogitudeLeadList> query2 = LogitudeLeadQuery.GetIQueryableEntityList(LogitudeLeads);
            query2 = filter.GetFilteredQuery<LogitudeLeadList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LogitudeLeadList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LogitudeLead", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeLeadList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeLeadList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeLeadList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeLeadList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeLeadList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.ContactName);
            }
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetLogitudeLeadFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            LogitudeLeadRepository LogitudeLeadsRepository = new LogitudeLeadRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<LogitudeLead> LogitudeLeads = LogitudeLeadsRepository.GetLogitudeLeadsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            LogitudeLeads = filter.GetFilteredQuery<LogitudeLead>(nonListQueryOperation, LogitudeLeads);
            LogitudeLeadQuery LogitudeLeadQuery = new LogitudeLeadQuery(LogitudeLeadsRepository);
            IQueryable<LogitudeLeadList> query2 = LogitudeLeadQuery.GetIQueryableEntityList(LogitudeLeads);
            query2 = filter.GetFilteredQuery<LogitudeLeadList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        /// </summary>
        #endregion

        #region PaymentCurrency 
        public IQueryable<PaymentCurrency> GetPaymentCurrencies(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentCurrencyRepository = new PaymentCurrencyRepository(tenant);
            return paymentCurrencyRepository.GetPaymentCurrencies();
        }

        public PaymentCurrencyPM GetSinglePaymentCurrencyPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentCurrencyQuery = new PaymentCurrencyQuery(tenant);
            return paymentCurrencyQuery.GetSinglePaymentCurrencyPM(code);
        }

        public PaymentCurrencyList GetSinglePaymentCurrencyList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentCurrencyRepository = new PaymentCurrencyRepository(tenant);
            paymentCurrencyQuery = new PaymentCurrencyQuery(paymentCurrencyRepository);

            PaymentCurrencyList paymentCurrencyList = null;
            PaymentCurrency paymentCurrency = paymentCurrencyRepository.GetSinglePaymentCurrency(code);

            if (paymentCurrency != null)
            {
                List<PaymentCurrency> singleEntityList = new List<PaymentCurrency>();
                singleEntityList.Add(paymentCurrency);

                IQueryable<PaymentCurrency> iQueryable = singleEntityList.AsQueryable();
                IQueryable<PaymentCurrencyList> iQueryableEntityList = paymentCurrencyQuery.GetIQueryableEntityList(iQueryable);
                paymentCurrencyList = iQueryableEntityList.FirstOrDefault();
            }
            return paymentCurrencyList;
        }

        public IQueryable<PaymentCurrencyList> GetPaymentCurrencyLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentCurrencyRepository = new PaymentCurrencyRepository(tenant);
            paymentCurrencyQuery = new PaymentCurrencyQuery(paymentCurrencyRepository);

            IQueryable<PaymentCurrency> iQueryable = paymentCurrencyRepository.GetPaymentCurrencies();
            IQueryable<PaymentCurrencyList> query2 = paymentCurrencyQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PaymentCurrencyList> GetPaymentCurrencyFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentCurrencyRepository = new PaymentCurrencyRepository(tenant);
            paymentCurrencyQuery = new PaymentCurrencyQuery(paymentCurrencyRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PaymentCurrency> iQueryable = paymentCurrencyRepository.GetPaymentCurrencies();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentCurrency>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PaymentCurrencyList> query2 = paymentCurrencyQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PaymentCurrencyList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PaymentCurrencyList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PaymentCurrency", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentCurrencyList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentCurrencyList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentCurrencyList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentCurrencyList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentCurrencyList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetPaymentCurrencyFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentCurrencyRepository = new PaymentCurrencyRepository(tenant);
            paymentCurrencyQuery = new PaymentCurrencyQuery(paymentCurrencyRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PaymentCurrency> iQueryable = paymentCurrencyRepository.GetPaymentCurrencies();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentCurrency>(nonListQueryOperation, iQueryable);

            IQueryable<PaymentCurrencyList> query2 = paymentCurrencyQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PaymentCurrencyList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPaymentCurrency(PaymentCurrency entity)
        {
            paymentCurrencyRepository.Add(entity);
        }

        public void UpdatePaymentCurrency(PaymentCurrency currentEntity)
        {
            paymentCurrencyRepository.Update(currentEntity);
        }

        public void DeletePaymentCurrency(PaymentCurrency entity)
        {
            paymentCurrencyRepository.Remove(entity);
        }
        #endregion

        #region Setting

        public SettingPM GetSingleSettingPM()
        {
            settingRepository = new SettingRepository(objectContext);
            settingQuery = new SettingQuery(settingRepository);
            return settingQuery.GetSinglePM();
        }

        #endregion

        #region BluesnapContract
        public IQueryable<BluesnapContract> GetBluesnapContracts(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BluesnapContract", "READ", tenant);

            BluesnapContractRepository bluesnapContractRepository = new BluesnapContractRepository();
            return bluesnapContractRepository.GetBluesnapContracts(0);
        }

        public BluesnapContractPM GetBluesnapContractByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BluesnapContract", "READ", tenant);

            BluesnapContractQuery bluesnapContractQuery = new BluesnapContractQuery();
            BluesnapContractPM bluesnapContract = bluesnapContractQuery.GetSinglePM(code);
            return bluesnapContract;
        }

        public BluesnapContractList GetSingleBluesnapContractList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BluesnapContract", "READ", tenant);

            BluesnapContractRepository bluesnapContractRepository = new BluesnapContractRepository();
            BluesnapContractQuery bluesnapContractQuery = new BluesnapContractQuery(bluesnapContractRepository);
            BluesnapContractList bluesnapContractList = null;
            BluesnapContract bluesnapContract = bluesnapContractRepository.GetSingleBluesnapContract(code, 0);

            if (bluesnapContract != null)
            {
                List<BluesnapContract> singleEntityList = new List<BluesnapContract>();
                singleEntityList.Add(bluesnapContract);

                IQueryable<BluesnapContract> iQueryable = singleEntityList.AsQueryable();
                IQueryable<BluesnapContractList> iQueryableEntityList = bluesnapContractQuery.GetIQueryableEntityList(iQueryable);
                bluesnapContractList = iQueryableEntityList.FirstOrDefault();
            }
            return bluesnapContractList;
        }

        public void InsertBluesnapContract(BluesnapContractPM entityPm)
        {
            SecurityUtility.CheckContactFeature("BluesnapContract", "NEW", 0);

            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BluesnapContractService service = new BluesnapContractService(objectContext, 0);
            service.Create(entityPm);

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TableLastUpdateClass.UpdateTableHistory(0, "BluesnapContract");

                scope.Complete();
            }
        }

        public void UpdateBluesnapContract(BluesnapContractPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("BluesnapContract", "UPDATE", 0);

            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BluesnapContractService service = new BluesnapContractService(objectContext, 0);
            service.Update(currententityPm);

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TableLastUpdateClass.UpdateTableHistory(0, "BluesnapContract");

                scope.Complete();
            }
        }

        public void DeleteBluesnapContract(BluesnapContractPM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BluesnapContractRepository bluesnapContractRepository = new BluesnapContractRepository(objectContext);
            BluesnapContract removedEntity = bluesnapContractRepository.GetSingleBluesnapContract(entityPm.Code, 0);
            bluesnapContractRepository.Remove(removedEntity);
        }

        public IQueryable<BluesnapContractList> GetBluesnapContractLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BluesnapContractRepository bluesnapContractRepository = new BluesnapContractRepository();
            IQueryable<BluesnapContract> bluesnapContracts = bluesnapContractRepository.GetBluesnapContracts(0);
            BluesnapContractQuery bluesnapContractQuery = new BluesnapContractQuery(bluesnapContractRepository);
            IQueryable<BluesnapContractList> query2 = bluesnapContractQuery.GetIQueryableEntityList(bluesnapContracts);
            return query2;

        }

        [Query(HasSideEffects = true)]
        public IQueryable<BluesnapContractList> GetBluesnapContractFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BluesnapContractRepository bluesnapContractRepository = new BluesnapContractRepository();
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BluesnapContract> bluesnapContracts = bluesnapContractRepository.GetBluesnapContracts(0);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            bluesnapContracts = filter.GetFilteredQuery<BluesnapContract>(nonListQueryOperation, bluesnapContracts);

            int skippedPorts = queryOperations.PageIndex;
            BluesnapContractQuery bluesnapContractQuery = new BluesnapContractQuery(bluesnapContractRepository);
            IQueryable<BluesnapContractList> query2 = bluesnapContractQuery.GetIQueryableEntityList(bluesnapContracts);
            query2 = filter.GetFilteredQuery<BluesnapContractList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BluesnapContractList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BluesnapContract", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetBluesnapContractFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BluesnapContractRepository bluesnapContractRepository = new BluesnapContractRepository();
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BluesnapContract> bluesnapContracts = bluesnapContractRepository.GetBluesnapContracts(0);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            bluesnapContracts = filter.GetFilteredQuery<BluesnapContract>(nonListQueryOperation, bluesnapContracts);
            BluesnapContractQuery bluesnapContractQuery = new BluesnapContractQuery(bluesnapContractRepository);
            IQueryable<BluesnapContractList> query2 = bluesnapContractQuery.GetIQueryableEntityList(bluesnapContracts);
            query2 = filter.GetFilteredQuery<BluesnapContractList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        #endregion

        #region BatchServicesDefinition
        public IQueryable<BatchServicesDefinition> GetBatchServicesDefinitions()
        {
            BatchServicesDefinitionRepository batchServicesDefinitionRepository = new BatchServicesDefinitionRepository();
            return batchServicesDefinitionRepository.GetAllBatchServicesDefinitions();
        }
        public IQueryable<BatchServicesDefinitionList> GetAllBatchServicesDefinitionsListWithLogs(DateTime? LastActivity)
        {
            BatchServicesDefinitionQuery Query = new BatchServicesDefinitionQuery();
            return Query.GetAllBatchServicesDefinitionsList(LastActivity);
        }
        public IQueryable<BatchServicesDefinitionPM> GetAllBatchServicesDefinitionsPMsWithLogs(DateTime? LastActivity, string status)
        {
            BatchServicesDefinitionQuery Query = new BatchServicesDefinitionQuery();
            return Query.GetAllBatchServicesDefinitionsPMs(LastActivity, status);
        }
        public BatchServicesDefinitionPM GetBatchServicesDefinitionByCode(string code)
        {


            BatchServicesDefinitionQuery batchServicesDefinitionQuery = new BatchServicesDefinitionQuery();
            BatchServicesDefinitionPM batchServicesDefinition = batchServicesDefinitionQuery.GetSingleBatchServicesDefinitionPM(code);
            return batchServicesDefinition;
        }

        public BatchServicesDefinitionList GetSingleBatchServicesDefinitionList(string code, int tenant)
        {


            BatchServicesDefinitionRepository batchServicesDefinitionRepository = new BatchServicesDefinitionRepository();
            BatchServicesDefinitionQuery BatchServicesDefinitionQuery = new BatchServicesDefinitionQuery(batchServicesDefinitionRepository);
            BatchServicesDefinitionList BatchServicesDefinitionList = null;
            BatchServicesDefinition BatchServicesDefinition = batchServicesDefinitionRepository.GetSingleBatchServicesDefinition(code);

            if (BatchServicesDefinition != null)
            {
                List<BatchServicesDefinition> singleEntityList = new List<BatchServicesDefinition>();
                singleEntityList.Add(BatchServicesDefinition);

                IQueryable<BatchServicesDefinition> iQueryable = singleEntityList.AsQueryable();
                IQueryable<BatchServicesDefinitionList> iQueryableEntityList = BatchServicesDefinitionQuery.GetIQueryableEntityList(iQueryable);
                BatchServicesDefinitionList = iQueryableEntityList.FirstOrDefault();
            }
            return BatchServicesDefinitionList;
        }

        public void InsertBatchServicesDefinition(BatchServicesDefinitionPM entityPm)
        {


            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BatchServicesDefinitionService service = new BatchServicesDefinitionService(objectContext);
            service.Create(entityPm);


        }

        public void UpdateBatchServicesDefinition(BatchServicesDefinitionPM currententityPm)
        {


            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BatchServicesDefinitionService service = new BatchServicesDefinitionService(objectContext);
            service.Update(currententityPm);


        }

        public void DeleteBatchServicesDefinition(BatchServicesDefinitionPM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BatchServicesDefinitionRepository BatchServicesDefinitionRepository = new BatchServicesDefinitionRepository(objectContext);
            BatchServicesDefinition removedEntity = BatchServicesDefinitionRepository.GetSingleBatchServicesDefinition(entityPm.Code);
            BatchServicesDefinitionRepository.Remove(removedEntity);
        }

        public IQueryable<BatchServicesDefinitionList> GetBatchServicesDefinitionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BatchServicesDefinitionRepository BatchServicesDefinitionRepository = new BatchServicesDefinitionRepository();
            IQueryable<BatchServicesDefinition> BatchServicesDefinitions = BatchServicesDefinitionRepository.GetAllBatchServicesDefinitions();
            BatchServicesDefinitionQuery BatchServicesDefinitionQuery = new BatchServicesDefinitionQuery(BatchServicesDefinitionRepository);
            IQueryable<BatchServicesDefinitionList> query2 = BatchServicesDefinitionQuery.GetIQueryableEntityList(BatchServicesDefinitions);
            return query2;

        }

        [Query(HasSideEffects = true)]
        public IQueryable<BatchServicesDefinitionList> GetBatchServicesDefinitionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BatchServicesDefinitionRepository BatchServicesDefinitionRepository = new BatchServicesDefinitionRepository();
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BatchServicesDefinition> BatchServicesDefinitions = BatchServicesDefinitionRepository.GetAllBatchServicesDefinitions();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            BatchServicesDefinitions = filter.GetFilteredQuery<BatchServicesDefinition>(nonListQueryOperation, BatchServicesDefinitions);

            int skippedPorts = queryOperations.PageIndex;
            BatchServicesDefinitionQuery BatchServicesDefinitionQuery = new BatchServicesDefinitionQuery(BatchServicesDefinitionRepository);
            IQueryable<BatchServicesDefinitionList> query2 = BatchServicesDefinitionQuery.GetIQueryableEntityList(BatchServicesDefinitions);
            query2 = filter.GetFilteredQuery<BatchServicesDefinitionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BatchServicesDefinitionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BatchServicesDefinition", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetBatchServicesDefinitionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BatchServicesDefinitionRepository BatchServicesDefinitionRepository = new BatchServicesDefinitionRepository();
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BatchServicesDefinition> BatchServicesDefinitions = BatchServicesDefinitionRepository.GetAllBatchServicesDefinitions();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            BatchServicesDefinitions = filter.GetFilteredQuery<BatchServicesDefinition>(nonListQueryOperation, BatchServicesDefinitions);
            BatchServicesDefinitionQuery BatchServicesDefinitionQuery = new BatchServicesDefinitionQuery(BatchServicesDefinitionRepository);
            IQueryable<BatchServicesDefinitionList> query2 = BatchServicesDefinitionQuery.GetIQueryableEntityList(BatchServicesDefinitions);
            query2 = filter.GetFilteredQuery<BatchServicesDefinitionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        #endregion

        #region HelpResource
        public IQueryable<HelpResource> GetAllHelpResources(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("HelpResource", "READ", tenant);

            HelpResourceRepository rep = new HelpResourceRepository();
            return rep.GetAllHelpResources();
        }
        public IQueryable<HelpResource> GetReleaseHelpResources(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("HelpResource", "READ", tenant);

            HelpResourceRepository rep = new HelpResourceRepository();
            return rep.GetReleaseHelpResources();
        }
        #endregion

        #region TenantType
        public IQueryable<TenantType> GetTenantTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tenantTypeRepository = new TenantTypeRepository(tenant);
            return tenantTypeRepository.GetTenantTypes();
        }

        public TenantTypePM GetSingleTenantTypePM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tenantTypeQuery = new TenantTypeQuery(tenant);
            return tenantTypeQuery.GetSingleTenantTypePM(code);
        }

        public TenantTypeList GetSingleTenantTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tenantTypeRepository = new TenantTypeRepository(tenant);
            tenantTypeQuery = new TenantTypeQuery(tenantTypeRepository);

            TenantTypeList tenantTypeList = null;
            TenantType tenantType = tenantTypeRepository.GetSingleTenantType(code);

            if (tenantType != null)
            {
                List<TenantType> singleEntityList = new List<TenantType>();
                singleEntityList.Add(tenantType);

                IQueryable<TenantType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<TenantTypeList> iQueryableEntityList = tenantTypeQuery.GetIQueryableEntityList(iQueryable);
                tenantTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return tenantTypeList;
        }

        public IQueryable<TenantTypeList> GetTenantTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tenantTypeRepository = new TenantTypeRepository(tenant);
            tenantTypeQuery = new TenantTypeQuery(tenantTypeRepository);

            IQueryable<TenantType> iQueryable = tenantTypeRepository.GetTenantTypes();
            IQueryable<TenantTypeList> query2 = tenantTypeQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TenantTypeList> GetTenantTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tenantTypeRepository = new TenantTypeRepository(tenant);
            tenantTypeQuery = new TenantTypeQuery(tenantTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TenantType> iQueryable = tenantTypeRepository.GetTenantTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TenantType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TenantTypeList> query2 = tenantTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TenantTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TenantTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TenantType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TenantTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TenantTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TenantTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TenantTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TenantTypeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTenantTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tenantTypeRepository = new TenantTypeRepository(tenant);
            tenantTypeQuery = new TenantTypeQuery(tenantTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TenantType> iQueryable = tenantTypeRepository.GetTenantTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TenantType>(nonListQueryOperation, iQueryable);

            IQueryable<TenantTypeList> query2 = tenantTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TenantTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertTenantType(TenantType entity)
        {
            tenantTypeRepository.Add(entity);
        }

        public void UpdateTenantType(TenantType currentEntity)
        {
            tenantTypeRepository.Update(currentEntity);
        }

        public void DeleteTenantType(TenantType entity)
        {
            tenantTypeRepository.Remove(entity);
        }
        #endregion

        #region BatchServicesDefinitionMods
        public IQueryable<BatchServicesDefinitionMods> GetBatchServicesDefinitionMods()
        {


            BatchServicesDefinitionModsRepository batchServicesDefinitionModsRepository = new BatchServicesDefinitionModsRepository();
            return batchServicesDefinitionModsRepository.GetAllBatchServicesDefinitionMods();
        }

        public BatchServicesDefinitionModsPM GetBatchServicesDefinitionModByCode(string code)
        {


            BatchServicesDefinitionModsQuery batchServicesDefinitionModsQuery = new BatchServicesDefinitionModsQuery();
            BatchServicesDefinitionModsPM batchServicesDefinitionMods = batchServicesDefinitionModsQuery.GetSingleBatchServicesDefinitionModsPM(code);
            return batchServicesDefinitionMods;
        }

        public BatchServicesDefinitionModsList GetSingleBatchServicesDefinitionModList(string code, int tenant)
        {


            BatchServicesDefinitionModsRepository batchServicesDefinitionModsRepository = new BatchServicesDefinitionModsRepository();
            BatchServicesDefinitionModsQuery BatchServicesDefinitionModsQuery = new BatchServicesDefinitionModsQuery(batchServicesDefinitionModsRepository);
            BatchServicesDefinitionModsList BatchServicesDefinitionModsList = null;
            BatchServicesDefinitionMods BatchServicesDefinitionMods = batchServicesDefinitionModsRepository.GetSingleBatchServicesDefinitionMod(code);

            if (BatchServicesDefinitionMods != null)
            {
                List<BatchServicesDefinitionMods> singleEntityList = new List<BatchServicesDefinitionMods>();
                singleEntityList.Add(BatchServicesDefinitionMods);

                IQueryable<BatchServicesDefinitionMods> iQueryable = singleEntityList.AsQueryable();
                IQueryable<BatchServicesDefinitionModsList> iQueryableEntityList = BatchServicesDefinitionModsQuery.GetIQueryableEntityList(iQueryable);
                BatchServicesDefinitionModsList = iQueryableEntityList.FirstOrDefault();
            }
            return BatchServicesDefinitionModsList;
        }

        public void InsertBatchServicesDefinitionMod(BatchServicesDefinitionModsPM entityPm)
        {


            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BatchServicesDefinitionModsService service = new BatchServicesDefinitionModsService(objectContext);
            service.Create(entityPm);


        }

        public void UpdateBatchServicesDefinitionMod(BatchServicesDefinitionModsPM currententityPm)
        {


            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BatchServicesDefinitionModsService service = new BatchServicesDefinitionModsService(objectContext);
            service.Update(currententityPm);


        }

        public void DeleteBatchServicesDefinitionMod(BatchServicesDefinitionModsPM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            BatchServicesDefinitionModsRepository BatchServicesDefinitionModsRepository = new BatchServicesDefinitionModsRepository(objectContext);
            BatchServicesDefinitionMods removedEntity = BatchServicesDefinitionModsRepository.GetSingleBatchServicesDefinitionMod(entityPm.Code);
            BatchServicesDefinitionModsRepository.Remove(removedEntity);
        }

        public IQueryable<BatchServicesDefinitionModsList> GetBatchServicesDefinitionModsLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BatchServicesDefinitionModsRepository BatchServicesDefinitionModsRepository = new BatchServicesDefinitionModsRepository();
            IQueryable<BatchServicesDefinitionMods> BatchServicesDefinitionsMods = BatchServicesDefinitionModsRepository.GetAllBatchServicesDefinitionMods();
            BatchServicesDefinitionModsQuery BatchServicesDefinitionModsQuery = new BatchServicesDefinitionModsQuery(BatchServicesDefinitionModsRepository);
            IQueryable<BatchServicesDefinitionModsList> query2 = BatchServicesDefinitionModsQuery.GetIQueryableEntityList(BatchServicesDefinitionsMods);
            return query2;

        }

        [Query(HasSideEffects = true)]
        public IQueryable<BatchServicesDefinitionModsList> GetBatchServicesDefinitionModsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BatchServicesDefinitionModsRepository BatchServicesDefinitionModsRepository = new BatchServicesDefinitionModsRepository();
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BatchServicesDefinitionMods> BatchServicesDefinitionsMods = BatchServicesDefinitionModsRepository.GetAllBatchServicesDefinitionMods();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            BatchServicesDefinitionsMods = filter.GetFilteredQuery<BatchServicesDefinitionMods>(nonListQueryOperation, BatchServicesDefinitionsMods);

            int skippedPorts = queryOperations.PageIndex;
            BatchServicesDefinitionModsQuery BatchServicesDefinitionModsQuery = new BatchServicesDefinitionModsQuery(BatchServicesDefinitionModsRepository);
            IQueryable<BatchServicesDefinitionModsList> query2 = BatchServicesDefinitionModsQuery.GetIQueryableEntityList(BatchServicesDefinitionsMods);
            query2 = filter.GetFilteredQuery<BatchServicesDefinitionModsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BatchServicesDefinitionModsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BatchServicesDefinitionMods", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionModsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionModsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionModsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionModsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesDefinitionModsList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.NumberOfThreads);
            }
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetBatchServicesDefinitionModsFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BatchServicesDefinitionModsRepository BatchServicesDefinitionModsRepository = new BatchServicesDefinitionModsRepository();
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BatchServicesDefinitionMods> BatchServicesDefinitionsMods = BatchServicesDefinitionModsRepository.GetAllBatchServicesDefinitionMods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            BatchServicesDefinitionsMods = filter.GetFilteredQuery<BatchServicesDefinitionMods>(nonListQueryOperation, BatchServicesDefinitionsMods);
            BatchServicesDefinitionModsQuery BatchServicesDefinitionModsQuery = new BatchServicesDefinitionModsQuery(BatchServicesDefinitionModsRepository);
            IQueryable<BatchServicesDefinitionModsList> query2 = BatchServicesDefinitionModsQuery.GetIQueryableEntityList(BatchServicesDefinitionsMods);
            query2 = filter.GetFilteredQuery<BatchServicesDefinitionModsList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        #endregion


        #region BluesnapContractType

        [Query(HasSideEffects = true)]
        public IQueryable<BluesnapContractTypeList> GetBluesnapContractTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BluesnapContractTypeRepository BluesnapContractTypeRepository = new BluesnapContractTypeRepository(tenant);
            BluesnapContractTypeQuery BluesnapContractTypeQuery = new BluesnapContractTypeQuery(BluesnapContractTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BluesnapContractType> iQueryable = BluesnapContractTypeRepository.GetBluesnapContractTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BluesnapContractType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<BluesnapContractTypeList> query2 = BluesnapContractTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<BluesnapContractTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BluesnapContractTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BluesnapContractType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractTypeList, string>(queryOperations, query2);
                                break;
                            }

                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BluesnapContractTypeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetBluesnapContractTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BluesnapContractTypeRepository BluesnapContractTypeRepository = new BluesnapContractTypeRepository(tenant);
            BluesnapContractTypeQuery BluesnapContractTypeQuery = new BluesnapContractTypeQuery(BluesnapContractTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BluesnapContractType> iQueryable = BluesnapContractTypeRepository.GetBluesnapContractTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BluesnapContractType>(nonListQueryOperation, iQueryable);

            IQueryable<BluesnapContractTypeList> query2 = BluesnapContractTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<BluesnapContractTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }
        #endregion

        //public void LoadDataBases()
        //{
        //    if (!SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
        //    {
        //        GlobalDB db1 = new GlobalDB() { Id = "0", DBConnection = "WebFreightBranch2,simplog@z0n0c08sao,Saas256!@" };
        //        globalDBsRepository.Add(db1);
        //    }
        //    else
        //    {
        //        GlobalDB db1 = new GlobalDB() { Id = "0", DBConnection = "WebFreightBranch2,sa,Saas256" /*BuildConnectionString("WebFreight")*/ };
        //        globalDBsRepository.Add(db1);
        //    }

        //    globalDBsRepository.SubmitChanges();
        //}

        private string BuildConnectionString(string dbName)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.DataSource = ".";
            sqlBuilder.InitialCatalog = dbName;
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.UserID = "sa";
            sqlBuilder.Password = "Saas256";

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = "System.Data.SqlClient";

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", "WebFreightModel");

            return entityBuilder.ToString();
        }

        protected override bool PersistChangeSet()
        {
            this.objectContext.SaveChanges();
            return base.PersistChangeSet();
        }
    }
}


