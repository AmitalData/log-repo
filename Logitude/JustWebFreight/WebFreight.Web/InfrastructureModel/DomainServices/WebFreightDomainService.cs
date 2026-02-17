using System;
using System.Collections.Generic;

using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.Azure;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.SystemLogs;
using Logitude.Server.Tools.Counters;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Data.Entity.Core;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    // Implements application logic using the WebFreightModelContainer context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
     
    //[RequiresAuthentication]
    [EnableClientAccess()]
    public partial class WebFreightDomainService : LogitudeDomainService
    {
        // TODO: Consider
        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public int LastTenantEntered;
        public string ConnectionString { get; set; }

       
        private PermissionTypeRepository permissionTypesRepository;
        private ObjectTableTypeRepository objectTableTypeRepository;
        private CustomPickListRepository customPickListsRepository;
        private PrepaidCollectRepository prepaidCollectsRepository;
        private DirectionRepository directionsRepository;
        private TransportModeRepository transportModeRepository;
        private MoveTypeRepository moveTypesRepository;
        private SpecialServicesRepository specialServicesRepository;
        private ValidationTypeRepository validationTypesRepository;
        private TraceEventRepository traceEventsRepository;
        private EventTypeRepository eventTypesRepository;
        private RatesTableRepository ratesTablesRepository;
        private IATACodeRepository iAtaCodesRepository;
        private QueryGroupRepository queryGroupRepository;
        private ChargesGroupRepository chargesGroupsRepository;
        private VolumeUnitRepository volumeUnitsRepository;
        private EntityStatusRepository entityStatusRepository;
        private DescriptionOfGoodsRepository descriptionOfGoodsRepository;
        private SharedLogisticsUpdateRepository sharedLogisticsUpdateRepository;
        private SharedLogisticsUpdateStatusRepository sharedLogisticsUpdateStatusRepository;
        private ObjectTableRepository objectTabelRepository;
        private EventTypeCategoryRepository eventTypeCategoryRepository;
        private MoveTypeRepository moveTypeRepository;
        private SharedLogisticsInvitationStatusRepository sharedLogisticsInvitationStatusRepository;
        private InboundEmailRepository inboundEmailRepository;
        private InboundEmailLineRepository inboundEmailLineRepository;
        private APILogsRepository APILogsRepository;
        private APILogsDataRepository APILogsDataRepository;
        private QueueMessageMoreDetailsRepository QueueMessageMoreDetailsRepository;
        private TasksSchedulerRepository TasksSchedulerRepository;
        private TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository;

        private IWebFreightContext objectContext;
        private UserData currentUser;

        private ChargesGroupQuery chargesGroupQuery;
        private DescriptionOfGoodsQuery descriptionOfGoodsQuery;
        private DirectionQuery directionQuery;
        private EntityStatusQuery entityStatusQuery;
        private EventTypeQuery eventTypeQuery;
        private FollowUpQuery followUpQuery;
        private IATACodeQuery iataCodeQuery;
        private CustomPickListQuery customPickListQuery;
        private PrepaidCollectQuery prepaidCollectQuery;
        private QueryGroupQuery queryGroupQuery;
        private RatesTableQuery ratesTableQuery;
        private SharedLogisticsUpdateQuery sharedLogisticsUpdateQuery;
        private TraceEventQuery traceEventQuery;
        private TransportModeQuery transportModeQuery;
        private VolumeUnitQuery volumeUnitQuery;
        private EventTypeCategoryQuery eventTypeCategoryQuery;
        private MoveTypeQuery moveTypeQuery;
        private SharedLogisticsInvitationStatusQuery sharedLogisticsInvitationStatusQuery;
        private InboundEmailQuery inboundEmailQuery;
        private InboundEmailLineQuery inboundEmailLineQuery;
        private APILogsQuery APILogsQuery;
        private APILogsDataQuery APILogsDataQuery;
        private QueueMessageMoreDetailsQuery QueueMessageMoreDetailsQuery;
        private TasksSchedulerQuery TasksSchedulerQuery;
        private TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery;

        public WebFreightDomainService(UserData currentUser)
        {
        }

        public WebFreightDomainService()
        {
        }

        public WebFreightDomainService(IWebFreightContext context)
        {
           this.objectContext = context as WebFreightContext;
        }

        public DateTime? GetFollowUpDate(string entityDateId, Shipment currentEntity, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            switch (entityDateId)
            {
                case "PicD":
                    {
                        return null;

                    }
                case "PicA":
                    {
                        return null;

                    }
                case "Open":
                    {
                        if (currentEntity.CreateDateTime != null)
                        {
                            return currentEntity.CreateDateTime;
                        }

                        else
                        {
                            return null;
                        }
                        break;
                    }
                case "McAr":
                    {

                        return null;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public IQueryable<FollowUpPM> GetFollowUpsFromTodayAndBeforForDashBoard(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            FollowUpRepository followUpRepository = new FollowUpRepository(tenant);
            followUpQuery = new FollowUpQuery(followUpRepository);
            return followUpQuery.GetFollowUpPMs(tenant);
        }

        #region EntityLastAccess
        public EntityLastAccessPM GetEntityLastAccessByEntity(string objectTableId, string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            return GetEntityLastAccessByEntity(objectTableId, entityId, tenant);
        }
        #endregion

        #region load functions
        public string LoadSampleBranchDepartments(int tenant,string countryCode)
        {
            try
            {
            }
            catch(Exception ex) 
            {
                if (ex.InnerException != null)
                {
                    return "exception: " + ex.Message + "iner Exception: " + ex.InnerException.Message;
                }
                else
                {
                    return "exception: " + ex.Message;
                }
            }
            return ""; 
        }

        public string LoadSampleUsers(int tenant, string countryCode)
        {
            try
            {
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return "exception: " + ex.Message + "iner Exception: " + ex.InnerException.Message;
                }
                else
                {
                    return "exception: " + ex.Message;
                }
            }
            return "";
         }
        
        public string LoadSampleCards(int tenant, string countryCode)
        {
            try
            {
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return "exception: " + ex.Message + "iner Exception: " + ex.InnerException.Message;
                }
                else
                {
                    return "exception: " + ex.Message;
                }
            }
            return "";
        }

        public string LoadSamplePorts(int tenant, string countryCode)
        {
            try
            {
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return "exception: " + ex.Message + "iner Exception: " + ex.InnerException.Message;
                }
                else
                {
                    return "exception: " + ex.Message;
                }
            }
            return "";
        }

        public string LoadSampleCurrencies(int tenant, string countryCode)
        {
            try
            {
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return "exception: " + ex.Message + "iner Exception: " + ex.InnerException.Message;
                }
                else
                {
                    return "exception: " + ex.Message;
                }
            }
            return "";
        }

        public string LoadSampleCarriers(int tenant, string countryCode)
        {
            try
            {
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return "exception: " + ex.Message + "iner Exception: " + ex.InnerException.Message;
                }
                else
                {
                    return "exception: " + ex.Message;
                }
            }
            return "";
        }

        DateTime currentOpenDate;
        [Invoke]
        public DateTime LoadSampleShipments(int tenant,int shipmentCount,int originalCount,DateTime theCurrentOpenDate,string countryCode)
        {
            return theCurrentOpenDate;
        }

        public void LoadMassiveData()
        {
            
        }

        public void SetUserData(int tenant)
        {
            WebFreightDomainService defaultDomainService = new WebFreightDomainService();
            CommonDataDomainService infraDomainService = new CommonDataDomainService();

            List<GlobalZone> zones = infraDomainService.GetGlobalZonesByTenant(0).ToList<GlobalZone>();
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(tenant);
            foreach (GlobalZone z in zones)
            {
                GlobalZone zone = new GlobalZone()
                {
                    Code = z.Code,
                    EnglishName = z.EnglishName,
                    LocalName = z.LocalName,
                    Tenant = tenant,
                    Id = IdCounter.GetNumber("GlobalZone", tenant).ToString(),
                    InActive = false,
                };

                globalZoneRepository.Add(zone);
            }
            globalZoneRepository.SubmitChanges();

            List<Country> countries = infraDomainService.GetCountriesByTenant(0).ToList<Country>();
            CountryRepository countryRepository = new CountryRepository(tenant);
            foreach (Country c in countries)
            {
                Country country = new Country()
                {
                    Code = c.Code,
                    EnglishName = c.EnglishName,
                    LocalName = c.LocalName,
                    Tenant = tenant,
                    AddedManually = false,
                    InActive = false,
                    Id = IdCounter.GetNumber("Country", tenant).ToString(),
                    GlobalZoneId = globalZoneRepository.GetGlobalZones(tenant).Where(d => d.Code == c.GlobalZone.Code).FirstOrDefault().Id,
                    EC = c.EC,
                    Notes = c.Notes,
                };
                countryRepository.Add(country);
            }

            countryRepository.SubmitChanges();

            IQueryable<State> states = infraDomainService.GetStatesByTenant(0);
            StateRepository stateRepository = new StateRepository(tenant);
            foreach (State s in states)
            {
                State state = new State()
                {
                    Code = s.Code,
                    EnglishName = s.EnglishName,
                    LocalName = s.LocalName,
                    Tenant = tenant,
                    AddedManually = false,
                    InActive = false,
                    Id = IdCounter.GetNumber("State",tenant).ToString(),
                    CountryId = countryRepository.GetCountries(tenant).Where(d => d.Code == s.Country.Code).FirstOrDefault().Id
                };
               
                WebFreightDomainService a = new WebFreightDomainService();

                stateRepository.Add(state);
            }
            stateRepository.SubmitChanges();

            #region currencies
            CurrencyRepository currencyRep = new CurrencyRepository(tenant);
            CurrencyQuery currencyQuery = new CurrencyQuery(currencyRep);
            List<CurrencyPM> currencies = currencyQuery.GetCurrenciesByTenantPM(0).Where(d => d.Code == "USD" || d.Code == "EUR").ToList();

            foreach (CurrencyPM currency in currencies)
            {
                Currency newCurrency = new Currency()
                {
                    Code = currency.Code,
                    EnglishName = currency.EnglishName,
                    Id = IdCounter.GetNumber("Currency",tenant).ToString(),
                    InActive = currency.InActive,
                    LocalName = currency.LocalName,
                    Notes = currency.Notes,
                    Tenant = tenant,

                };
                currencyRep.Add(newCurrency);
            }
            currencyRep.SubmitChanges();
            #endregion

            IncotermRepository incotermsRepository = new IncotermRepository(tenant);
            List<Incoterm> incoterms = incotermsRepository.GetIncoterms(0).ToList<Incoterm>();
           foreach (Incoterm i in incoterms)
           {
               Incoterm incoterm = new Incoterm()
               {
                   Id = IdCounter.GetNumber("Incoterm",tenant).ToString(),
                   Name = i.Name,
                   LocalName = i.LocalName,
                   Tenant = tenant,
                   Code =i.Code,
                   Freight = i.Freight,
                   OtherCharges = i.OtherCharges,
                   AddedManually = i.AddedManually,
                   InActive = i.InActive,
                   Notes = i.Notes,
               };
               incotermsRepository.Add(incoterm);
           }

           incotermsRepository.SubmitChanges();
        }

        //public override bool Submit(ChangeSet changeSet)
        //{
        //    using (TransactionScope scope = TransactionFactory.GetTransaction())
        //    {
        //        bool f = true;
        //        try
        //        {
        //            f = base.Submit(changeSet);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new DomainException("Submit failed", ex);
        //        }
        //        scope.Complete();
        //        return f;
        //    }
        //}
        #endregion

        public bool DoesEntityHasUpdates(string entityId, string objectTableId, int tenant,string entityGUID)
        {
            EntityLastUpdateRepository entityLastUpdateRepository = new EntityLastUpdateRepository(tenant);
            return entityLastUpdateRepository.DoesEntityHasUpdates(entityId, objectTableId, tenant, entityGUID);
        }

        public void ClearEntityLastAccessLog(int tenant)
        {          
        }

        protected override bool PersistChangeSet()
        {
            try
            {                
                objectContext.SaveChanges();
            }
            catch (OptimisticConcurrencyException ex)
            {               
                throw new Exception("Sorry you can't update this record right now it's being updated by another user");                
            }
            return base.PersistChangeSet();
        }

        public override void Initialize(DomainServiceContext context)
        {       
            base.Initialize(context);
        }

        //protected override void OnError(DomainServiceErrorInfo errorInfo)
        //{           
        //    ExceptionHandler.HandleException(errorInfo.Error, DateTime.Now, 0, "", "WebFreight Domain Service", "OnError()");
        //    //string errorMessage;
        //    //errorMessage = errorInfo.Error.Message;

        //    //if (errorInfo.Error.InnerException != null)
        //    //{
        //    //    errorMessage += Environment.NewLine + errorInfo.Error.InnerException.Message;
        //    //}
        //    //errorMessage += Environment.NewLine + errorInfo.ToString();
        //    //if (!string.IsNullOrEmpty(errorInfo.Error.StackTrace))
        //    //{
        //    //    errorMessage += Environment.NewLine + errorInfo.Error.StackTrace;
        //    //}
        //    //AzureLog.SaveLogsInStorage(errorMessage, "E", DateTime.Now, errorInfo.Error.Message, errorInfo.Error.StackTrace, 0, ServiceContext.User != null ? ServiceContext.User.Identity.Name : "", ServiceContext.User != null ? ServiceContext.User.Identity.Name : "");
        //    base.OnError(errorInfo);
        //    throw errorInfo.Error;
        //}

        private string BuildConnectionString(string dbConnectionInfo)
        {
            string[] information = dbConnectionInfo.Split(',');
            string dbName = information[0];
            string userName = information[1];
            string pass = information[2];
            SqlConnectionStringBuilder sqlBuilder =
               new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = ".";
            sqlBuilder.InitialCatalog = dbName;
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.UserID = userName;
            sqlBuilder.Password = pass;

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
            entityBuilder.Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl",
                "WebFreightModel");

            return entityBuilder.ToString();
        }

        bool isUpdate = false;

        //----IATACodes-----
        Dictionary<string, string> iataCodesDect = new Dictionary<string, string>();

        public void LoadIATACodes(string iataCodesStringOfLine)
        {
            string[] stringLineArray = iataCodesStringOfLine.Split('\n');
            //portLinesCount = stringLineArray.Length;
            string[] readIATACodesData = null;

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readIATACodesData = stringLineArray[i].Split(',');

                    if (readIATACodesData.Length >= 2)
                    {
                        if (readIATACodesData[1].Length > 119)
                            readIATACodesData[1] = readIATACodesData[1].Substring(0, 119);

                        if (readIATACodesData[0].Length == 2 && readIATACodesData[1].Trim() != String.Empty)
                        {
                            if (!iataCodesDect.Keys.Contains(readIATACodesData[0]))
                            {
                                IATACode newIATACode = new IATACode();
                                newIATACode.Code = readIATACodesData[0];
                                newIATACode.Name = readIATACodesData[1];

                                iataCodesDect.Add(readIATACodesData[0], readIATACodesData[0]);
                                iAtaCodesRepository.Add(newIATACode);
                            }
                        }
                    }
                }
            }
            iAtaCodesRepository.SubmitChanges();
        }

        //[Invoke]
        //public void GetSavedTenantEventsDocs(TenantPM tenant, List<EventTypePM> events, List<DocumentTypePM> docs )
        //{
        //    DocumentTypeRepository docsRep = new DocumentTypeRepository(tenant.Id);
        //    EventTypeRepository eventsRep = new EventTypeRepository(tenant.Id);
        //    TenantRepository tenRep = new TenantRepository(tenant.Id);

        //    Tenant tenantPoco = tenRep.GetSingleTenant(tenant.Id);
        //    tenRep.Update(tenantPoco);
        //    tenRep.SubmitChanges();

        //    foreach(EventTypePM item in events)
        //    {
        //        EventType ev = eventsRep.GetSingleEventType(item.Id, item.Tenant, false);
        //        eventsRep.Update(ev);
        //    }
        //    eventsRep.SubmitChanges();

        //    foreach (DocumentTypePM item in docs)
        //    {
        //        DocumentType doc = docsRep.GetSingleDocumentTypes(item.Id, item.Tenant);
        //        docsRep.Update(doc);
        //    }
        //    docsRep.SubmitChanges();
        //}
    }
}


