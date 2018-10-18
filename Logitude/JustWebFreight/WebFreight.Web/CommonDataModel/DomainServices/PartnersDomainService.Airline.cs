using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdateAirlineList(AirlineList currentEntity)
        {
        }

        public IQueryable<Airline> GetAirlines(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineRepository = new AirlineRepository(tenant);
            return airlineRepository.GetAirlines(0);
        }

        public IQueryable<AirlinePM> GetAirlinesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineQuery = new AirlineQuery(tenant);
            return airlineQuery.GetAirlinePMsByTenant(tenant);
        }

        public bool DoesAirlineCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineRepository = new AirlineRepository(tenant);
            return (airlineRepository.GetAirlines(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<AirlinePM> GetAirlineSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineQuery = new AirlineQuery(tenant);
            IQueryable<AirlinePM> q = airlineQuery.GetAirlineByCodeOrName(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public AirlinePM GetAirlineById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineQuery = new AirlineQuery(tenant);
            return airlineQuery.GetSinglePM(id, tenant);
        }

        public AirlinePM GetAirlineByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineQuery = new AirlineQuery(tenant);
            return airlineQuery.GetSinglePMByCode(code, tenant);
        }

        public AirlineList GetSingleAirlineList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineRepository = new AirlineRepository(tenant);
            AirlineList airlineList = null;
            Airline airline = airlineRepository.GetSingleAirline(id, tenant);

            if (airline != null)
            {
                List<Airline> singleEntityList = new List<Airline>();
                singleEntityList.Add(airline);

                airlineQuery = new AirlineQuery(airlineRepository);
                IQueryable<Airline> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AirlineList> iQueryableEntityList = airlineQuery.GetIQueryableEntityList(iQueryable);
                airlineList = iQueryableEntityList.FirstOrDefault();
            }
            return airlineList;
        }

        public AirlineList GetAirlineByPrefix(string myPrefix, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineRepository = new AirlineRepository(tenant);
            AirlineList airlineList = null;
            Airline airline = airlineRepository.GetSingleAirlineByPrefix(myPrefix, tenant);

            if (airline != null)
            {
                List<Airline> singleEntityList = new List<Airline>();
                singleEntityList.Add(airline);

                airlineQuery = new AirlineQuery(airlineRepository);
                IQueryable<Airline> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AirlineList> iQueryableEntityList = airlineQuery.GetIQueryableEntityList(iQueryable);
                airlineList = iQueryableEntityList.FirstOrDefault();
            }
            return airlineList;
        }

        public IQueryable<AirlineList> GetAirlineLists(int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineRepository = new AirlineRepository(tenant);
            airlineQuery = new AirlineQuery(airlineRepository);

            IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
            IQueryable<AirlineList> query2 = airlineQuery.GetIQueryableEntityList(airlines);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AirlineList> GetAirlineFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineRepository = new AirlineRepository(tenant);
            airlineQuery = new AirlineQuery(airlineRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AirlineCustomFilter customfilters = new AirlineCustomFilter(tenant);
            airlines = customfilters.GetFilteredQuery(queryOperations, airlines);

            airlines = filter.GetFilteredQuery<Airline>(nonListQueryOperation, airlines);
            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AirlineList> query2 = airlineQuery.GetIQueryableEntityList(airlines);
            query2 = filter.GetFilteredQuery<AirlineList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AirlineList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Airline", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
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

        public int GetAirlineFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineRepository = new AirlineRepository(tenant);
            airlineQuery = new AirlineQuery(airlineRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AirlineCustomFilter customfilters = new AirlineCustomFilter(tenant);
            airlines = customfilters.GetFilteredQuery(queryOperations, airlines);
            airlines = filter.GetFilteredQuery<Airline>(nonListQueryOperation, airlines);

            IQueryable<AirlineList> query2 = airlineQuery.GetIQueryableEntityList(airlines);
            query2 = filter.GetFilteredQuery<AirlineList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertAirline(AirlinePM airline)
        {
            SecurityUtility.AuthenticationOnTenant(airline.Tenant);
            SecurityUtility.CheckContactFeature("Airline", "NEW", airline.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(airline.Tenant);
            }

            AirlineService service = new AirlineService(objectContext, airline.Tenant);
            service.Create(airline);
        }

        public void UpdateAirline(AirlinePM currentAirline)
        {
            SecurityUtility.AuthenticationOnTenant(currentAirline.Tenant);
            SecurityUtility.CheckContactFeature("Airline", "UPDATE", currentAirline.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentAirline.Tenant);
            }

            List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrenciesChangeSet = ChangeSet.GetAssociatedChanges(currentAirline, d => d.CardExternalCodeByCurrencies).Cast<CardExternalCodeByCurrencyPM>().ToList();
            foreach (CardExternalCodeByCurrencyPM itemPM in cardExternalCodeByCurrenciesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            AirlineService service = new AirlineService(objectContext, currentAirline.Tenant);
            service.SetChangeSet(cardExternalCodeByCurrenciesChangeSet);
            service.Update(currentAirline);
        }

        public void DeleteAirline(AirlinePM airline)
        {
            SecurityUtility.AuthenticationOnTenant(airline.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(airline.Tenant);
            }

            airlineRepository = new AirlineRepository(objectContext);
            Airline entity = airlineRepository.GetSingleAirline(airline.Id, airline.Tenant);
            airlineRepository.Remove(entity);
        }

        #region Is Registered

        public List<AirlineList> GetNeedsRegistrationAirlines(int tenant, string ccsTypeCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            List<AirlineList> myResult = new List<AirlineList>();
            airlineRepository = new AirlineRepository(tenant);
            airlineQuery = new AirlineQuery(airlineRepository);

            if (ccsTypeCode == "GLSHK")
            {
                IQueryable<Airline> iQueryable = airlineRepository.GetGLSHKNeedsRegistrationAirlines();
                myResult = airlineQuery.GetIQueryableEntityList(iQueryable).ToList();
            }

            else
            {
                IQueryable<Airline> iQueryable = airlineRepository.GetChampNeedsRegistrationAirlines();
                myResult = airlineQuery.GetIQueryableEntityList(iQueryable).ToList();
            }

            return myResult;
        }

        [Invoke]
        public void RegisteringAirline(bool isRegistering, string myTenantAirlineId, string tenantZeroAirlineId, int loggedTenantId, int tenantManagmentId, string ccsTypeCode, string updatedBy)
        {
            SecurityUtility.AuthenticationOnTenant(loggedTenantId);
            SecurityUtility.CheckContactFeature("Airline", "UPDATE", loggedTenantId);

            if (!string.IsNullOrEmpty(tenantZeroAirlineId))
            {
                if (!string.IsNullOrEmpty(ccsTypeCode))
                {
                    airlineRepository = new AirlineRepository(tenantManagmentId);
                    Airline myTenantAirline = airlineRepository.GetSingleAirline(myTenantAirlineId, tenantManagmentId);

                    if (myTenantAirline != null)
                    {
                        if (ccsTypeCode.ToUpper() == "GLSHK")
                        {
                            myTenantAirline.IsGLSHKRegistered = isRegistering;
                        }

                        else
                        {
                            myTenantAirline.IsChampRegistered = isRegistering;
                        }

                        myTenantAirline.RegistrationUpdatedBy = updatedBy;

                        this.CheckPartcipant(myTenantAirline, tenantManagmentId, isRegistering, "Reg", updatedBy);

                        airlineRepository.Update(myTenantAirline);
                        airlineRepository.SubmitChanges();

                        this.UpdateTenantManagementAirlinesFields(tenantManagmentId, isRegistering, "Reg", myTenantAirline);
                    }

                    else
                    {
                        if (isRegistering)
                        {
                            GetCarrierCopyToCurrentTenant(tenantZeroAirlineId, tenantManagmentId, ccsTypeCode, "Reg", isRegistering, null);
                        }
                    }

                    if (isRegistering)
                    {
                        string airlineCode = "";
                        string airlineName = "";

                        Card card = CardRepository.GetSingleCard(tenantZeroAirlineId, 0, true);
                        if (card != null)
                        {
                            airlineCode = card.Code;
                            airlineName = card.EnglishName;
                        }

                        ContactRepository contactRep = new ContactRepository(loggedTenantId);
                        Contact loggedContact = contactRep.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), loggedTenantId);

                        string note = "";
                        if (!string.IsNullOrEmpty(airlineName) && !string.IsNullOrEmpty(airlineCode))
                        {
                            note = airlineCode + ", " + airlineName;
                        }

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = loggedTenantId,
                            EventTypeCode = "ARRG",
                            UserId = loggedContact.Id,
                            EntityId = tenantManagmentId.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = note,
                        });
                    }
                }
            }
        }

        #endregion

        #region Is Allowed
        [Invoke]
        public void AllowAirline(bool isAllowed, string myAirlineCode, int loggedTenantId, int tenantManagmentId)
        {
            SecurityUtility.AuthenticationOnTenant(loggedTenantId);
            SecurityUtility.CheckContactFeature("Airline", "UPDATE", loggedTenantId);

            if (!string.IsNullOrEmpty(myAirlineCode))
            {
                if (isAllowed)
                {
                    airlineRepository = new AirlineRepository(loggedTenantId);
                    Airline myTargetTenantAirline = airlineRepository.GetSingleAirlineByCode(myAirlineCode, tenantManagmentId);
                    if (myTargetTenantAirline != null)
                    {
                        myTargetTenantAirline.IsAllowedInAirlinesRestriction = isAllowed;
                        airlineRepository.Update(myTargetTenantAirline);
                        airlineRepository.SubmitChanges();
                    }

                    else
                    {
                        Airline myZeroTenantAirline = airlineRepository.GetSingleAirlineByCode(myAirlineCode, 0);
                        if (myZeroTenantAirline != null)
                        {
                            GetCarrierCopyToCurrentTenant(myZeroTenantAirline.Id, tenantManagmentId, null, "All", isAllowed, null);
                        }
                    }
                }

                else
                {
                    airlineRepository = new AirlineRepository(tenantManagmentId);
                    Airline myAirline = airlineRepository.GetSingleAirlineByCode(myAirlineCode, tenantManagmentId);
                    if (myAirline != null)
                    {
                        myAirline.IsAllowedInAirlinesRestriction = isAllowed;
                        airlineRepository.Update(myAirline);
                        airlineRepository.SubmitChanges();
                    }
                }
            }
        }
        #endregion

        #region Is Requested
        [Invoke]
        public void RegistrationRequested(bool isRequested, string myTenantAirlineId, string tenantZeroAirlineId, int loggedTenantId, int tenantManagmentId, string ccsTypeCode)
        {
            SecurityUtility.AuthenticationOnTenant(loggedTenantId);
            SecurityUtility.CheckContactFeature("Airline", "UPDATE", loggedTenantId);

            if (!string.IsNullOrEmpty(tenantZeroAirlineId))
            {
                if (!string.IsNullOrEmpty(ccsTypeCode))
                {
                    airlineRepository = new AirlineRepository(tenantManagmentId);
                    Airline myAirline = airlineRepository.GetSingleAirline(myTenantAirlineId, tenantManagmentId);

                    if (myAirline != null)
                    {
                        if (ccsTypeCode.ToUpper() == "GLSHK")
                        {
                            myAirline.GLSHKRegistrationRequested = isRequested;
                        }

                        else
                        {
                            myAirline.ChampRegistrationRequested = isRequested;
                        }

                        this.CheckPartcipant(myAirline, tenantManagmentId, isRequested, "Req", null);

                        airlineRepository.Update(myAirline);
                        airlineRepository.SubmitChanges();

                        this.UpdateTenantManagementAirlinesFields(tenantManagmentId, isRequested, "Req", myAirline);
                    }

                    else
                    {
                        if (isRequested)
                        {
                            GetCarrierCopyToCurrentTenant(tenantZeroAirlineId, tenantManagmentId, ccsTypeCode, "Qeq", isRequested, null);
                        }
                    }

                    if (isRequested)
                    {
                        string airlineCode = "";
                        string airlineName = "";

                        Card card = CardRepository.GetSingleCard(tenantZeroAirlineId, 0, true);
                        if (card != null)
                        {
                            airlineCode = card.Code;
                            airlineName = card.EnglishName;
                        }

                        ContactRepository contactRep = new ContactRepository(loggedTenantId);
                        Contact loggedContact = contactRep.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), loggedTenantId);

                        string note = "";
                        if (!string.IsNullOrEmpty(airlineName) && !string.IsNullOrEmpty(airlineCode))
                        {
                            note = airlineCode + ", " + airlineName;
                        }

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = loggedTenantId,
                            EventTypeCode = "ARGR",
                            UserId = loggedContact.Id,
                            EntityId = tenantManagmentId.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = note,
                        });
                    }
                }
            }
        }
        #endregion

        #region Is Direct
        [Invoke]
        public void SetIsDirect(bool isDirect, string myTenantAirlineId, string tenantZeroAirlineId, int loggedTenantId, int tenantManagmentId, string ccsTypeCode)
        {
            SecurityUtility.AuthenticationOnTenant(loggedTenantId);
            SecurityUtility.CheckContactFeature("Airline", "UPDATE", loggedTenantId);

            if (!string.IsNullOrEmpty(tenantZeroAirlineId))
            {
                if (!string.IsNullOrEmpty(ccsTypeCode))
                {
                    airlineRepository = new AirlineRepository(tenantManagmentId);
                    Airline myAirline = airlineRepository.GetSingleAirline(myTenantAirlineId, tenantManagmentId);

                    if (myAirline != null)
                    {
                        this.CheckPartcipant(myAirline, tenantManagmentId, isDirect, "Dir", null);
                    }

                    else
                    {
                        if (isDirect)
                        {
                            GetCarrierCopyToCurrentTenant(tenantZeroAirlineId, tenantManagmentId, ccsTypeCode, "Dir", isDirect, null);
                        }
                    }

                    if (isDirect)
                    {
                        string airlineCode = "";
                        string airlineName = "";

                        Card card = CardRepository.GetSingleCard(tenantZeroAirlineId, 0, true);
                        if (card != null)
                        {
                            airlineCode = card.Code;
                            airlineName = card.EnglishName;
                        }

                        ContactRepository contactRep = new ContactRepository(loggedTenantId);
                        Contact loggedContact = contactRep.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), loggedTenantId);

                        string note = "";
                        if (!string.IsNullOrEmpty(airlineName) && !string.IsNullOrEmpty(airlineCode))
                        {
                            note = airlineCode + ", " + airlineName;
                        }

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = loggedTenantId,
                            EventTypeCode = "ASDI",
                            UserId = loggedContact.Id,
                            EntityId = tenantManagmentId.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = note,
                        });
                    }
                }
            }
        }
        #endregion

        #region Is Declined
        [Invoke]
        public void SetIsDeclined(bool isDeclined, string declineNotes, string myTenantAirlineId, string tenantZeroAirlineId, int loggedTenantId, int tenantManagmentId, string ccsTypeCode)
        {
            SecurityUtility.AuthenticationOnTenant(loggedTenantId);
            SecurityUtility.CheckContactFeature("Airline", "UPDATE", loggedTenantId);

            if (!string.IsNullOrEmpty(tenantZeroAirlineId))
            {
                if (!string.IsNullOrEmpty(ccsTypeCode))
                {
                    airlineRepository = new AirlineRepository(tenantManagmentId);
                    Airline myTenantAirline = airlineRepository.GetSingleAirline(myTenantAirlineId, tenantManagmentId);

                    if (myTenantAirline != null)
                    {
                        myTenantAirline.IsDeclined = isDeclined;
                        myTenantAirline.DeclineNotes = declineNotes;

                        airlineRepository.Update(myTenantAirline);
                        airlineRepository.SubmitChanges();

                        this.UpdateTenantManagementAirlinesFields(tenantManagmentId, isDeclined, "Dec", myTenantAirline);
                    }

                    else
                    {
                        if (isDeclined)
                        {
                            GetCarrierCopyToCurrentTenant(tenantZeroAirlineId, tenantManagmentId, ccsTypeCode, "Dec", isDeclined, declineNotes);
                        }
                    }

                    if (isDeclined)
                    {
                        string airlineCode = "";
                        string airlineName = "";

                        Card card = CardRepository.GetSingleCard(tenantZeroAirlineId, 0, true);
                        if (card != null)
                        {
                            airlineCode = card.Code;
                            airlineName = card.EnglishName;
                        }

                        ContactRepository contactRep = new ContactRepository(loggedTenantId);
                        Contact loggedContact = contactRep.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), loggedTenantId);

                        string note = "";
                        if (!string.IsNullOrEmpty(airlineName) && !string.IsNullOrEmpty(airlineCode))
                        {
                            note = airlineCode + ", " + airlineName;
                        }

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = loggedTenantId,
                            EventTypeCode = "ARDE",
                            UserId = loggedContact.Id,
                            EntityId = tenantManagmentId.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = note,
                        });
                    }
                }
            }
        }
        #endregion

        [Invoke]
        public string GetRestrictedAirlineId(int tenant)
        {
            AirlineRepository myRepository = new AirlineRepository(tenant);
            string myAirlineId = myRepository.GetAllowedAirlineId(tenant);
            return myAirlineId;
        }

        public AirlinePM GetAirlineByICAO(string ICAO, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            airlineQuery = new AirlineQuery(tenant);
            return airlineQuery.GetSinglePMByICAO(ICAO, tenant);
        }

        private void CheckPartcipant(Airline myTenantAirline, int tenantManagmentId, bool isProcessed, string processType, string updatedBy)
        {
            TenantManagement airlineTenant = null;
            TenantManagement forwarderTenant = null;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                airlineTenant = tenantManagementRepository.GetTenantManagementByConnectedArline(myTenantAirline.Card.Code);
                forwarderTenant = tenantManagementRepository.GetSingleTenantManagement(tenantManagmentId);

                scope.Complete();
            }

            if (airlineTenant != null)
            {
                #region
                participantRepository = new ParticipantRepository(airlineTenant.Id);
                Participant participant = participantRepository.GetSingleParticipantByForwarderandAirlineTenant(tenantManagmentId, airlineTenant.Id);

                if (participant != null)
                {
                    if (processType == "Reg")
                    {
                        participant.Registered = isProcessed;
                        participant.RegistrationUpdatedBy = updatedBy;
                    }

                    else if (processType == "Dir")
                    {
                        participant.IsDirect = isProcessed;
                    }

                    else
                    {
                        participant.RegistrationRequested = isProcessed;
                    }

                    participantRepository.Update(participant);
                    participantRepository.SubmitChanges();
                }

                else
                {
                    if (processType != "Reg")
                    {
                        if (forwarderTenant != null)
                        {
                            AddressRepository addressRepository = new AddressRepository(airlineTenant.Id);
                            TenantRepository tenantRepository = new TenantRepository(airlineTenant.Id);

                            Tenant myTenant = tenantRepository.GetSingleByTenant(forwarderTenant.Id);
                            Address myAddress = addressRepository.GetSingleAddress(myTenant.AddressId, forwarderTenant.Id);

                            ParticipantPM entityPM = new ParticipantPM()
                            {
                                Tenant = airlineTenant.Id,
                                EnglishName = forwarderTenant.Name,
                                LocalName = forwarderTenant.Name,
                                TTY = forwarderTenant.TTY,
                                ForwarderTenant = tenantManagmentId,
                                RegistrationRequested = processType == "Req" ? isProcessed : false,
                                IsDirect = processType == "Dir" ? isProcessed : false,
                                PartnerTypeId = "PT",
                            };

                            if (myAddress != null)
                            {
                                entityPM.Addresses.Add(new AddressPM()
                                {
                                    Tenant = airlineTenant.Id,
                                    AddressTypeId = "M",
                                    Description = "Main Address",
                                    Address1 = myAddress.Address1,
                                    Address2 = myAddress.Address2,
                                    ATTN = myAddress.ATTN,
                                    City = myAddress.City,
                                    CountryId = myAddress.CountryId,
                                    PhoneNumber = myAddress.PhoneNumber,
                                    FaxNumber = myAddress.FaxNumber,
                                    Name = myAddress.Name,
                                    StateId = myAddress.StateId,
                                    ZipCode = myAddress.ZipCode,
                                });
                            }

                            InsertParticipant(entityPM);
                        }
                    }
                }
                #endregion
            }
        }

        private void UpdateTenantManagementAirlinesFields(int tenantManagementId, bool isProcessing, string processType, Airline myAirline)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenantManagementId);

                if (tenantManagement != null)
                {
                    switch (processType)
                    {
                        #region Register
                        case "Reg":
                            {
                                if (isProcessing)
                                {
                                    if (string.IsNullOrEmpty(tenantManagement.RegisteredAirlines))
                                    {
                                        tenantManagement.RegisteredAirlines = myAirline.Card.Code;
                                    }

                                    else
                                    {
                                        if (!tenantManagement.RegisteredAirlines.Contains(myAirline.Card.Code))
                                        {
                                            tenantManagement.RegisteredAirlines = tenantManagement.RegisteredAirlines + ", " + myAirline.Card.Code;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(tenantManagement.PendingAirlines))
                                    {
                                        if (tenantManagement.PendingAirlines.Contains(myAirline.Card.Code))
                                        {
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Replace(myAirline.Card.Code, "");
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Trim();
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.TrimEnd(',');
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.TrimStart(',');
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Replace(" ,", "");
                                        }
                                    }
                                }

                                else
                                {
                                    bool isRequested = tenantManagement.AWBMessagesCCSTypeCode.ToUpper() == "GLSHK" ? myAirline.GLSHKRegistrationRequested : myAirline.ChampRegistrationRequested;
                                    bool isDeclined = myAirline.IsDeclined;

                                    if (!string.IsNullOrEmpty(tenantManagement.RegisteredAirlines))
                                    {
                                        if (tenantManagement.RegisteredAirlines.Contains(myAirline.Card.Code))
                                        {
                                            tenantManagement.RegisteredAirlines = tenantManagement.RegisteredAirlines.Replace(myAirline.Card.Code, "");
                                            tenantManagement.RegisteredAirlines = tenantManagement.RegisteredAirlines.Trim();
                                            tenantManagement.RegisteredAirlines = tenantManagement.RegisteredAirlines.TrimEnd(',');
                                            tenantManagement.RegisteredAirlines = tenantManagement.RegisteredAirlines.TrimStart(',');
                                            tenantManagement.RegisteredAirlines = tenantManagement.RegisteredAirlines.Replace(" ,", "");
                                        }
                                    }

                                    if (isRequested && !isDeclined)
                                    {
                                        if (string.IsNullOrEmpty(tenantManagement.PendingAirlines))
                                        {
                                            tenantManagement.PendingAirlines = myAirline.Card.Code;
                                        }

                                        else
                                        {
                                            if (!tenantManagement.PendingAirlines.Contains(myAirline.Card.Code))
                                            {
                                                tenantManagement.PendingAirlines = tenantManagement.PendingAirlines + ", " + myAirline.Card.Code;
                                            }
                                        }
                                    }
                                }

                                break;
                            }
                        #endregion

                        #region Request
                        case "Req":
                            {
                                bool isRegistered = tenantManagement.AWBMessagesCCSTypeCode.ToUpper() == "GLSHK" ? myAirline.IsGLSHKRegistered : myAirline.IsChampRegistered;
                                bool isDeclined = myAirline.IsDeclined;

                                if (isProcessing)
                                {
                                    if (string.IsNullOrEmpty(tenantManagement.RequestedAirlines))
                                    {
                                        tenantManagement.RequestedAirlines = myAirline.Card.Code;
                                    }

                                    else
                                    {
                                        if (!tenantManagement.RequestedAirlines.Contains(myAirline.Card.Code))
                                        {
                                            tenantManagement.RequestedAirlines = tenantManagement.RequestedAirlines + ", " + myAirline.Card.Code;
                                        }
                                    }

                                    if (!isRegistered && !isDeclined)
                                    {
                                        if (string.IsNullOrEmpty(tenantManagement.PendingAirlines))
                                        {
                                            tenantManagement.PendingAirlines = myAirline.Card.Code;
                                        }

                                        else
                                        {
                                            if (!tenantManagement.PendingAirlines.Contains(myAirline.Card.Code))
                                            {
                                                tenantManagement.PendingAirlines = tenantManagement.PendingAirlines + ", " + myAirline.Card.Code;
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    if (!string.IsNullOrEmpty(tenantManagement.RequestedAirlines))
                                    {
                                        if (tenantManagement.RequestedAirlines.Contains(myAirline.Card.Code))
                                        {
                                            tenantManagement.RequestedAirlines = tenantManagement.RequestedAirlines.Replace(myAirline.Card.Code, "");
                                            tenantManagement.RequestedAirlines = tenantManagement.RequestedAirlines.Trim();
                                            tenantManagement.RequestedAirlines = tenantManagement.RequestedAirlines.TrimEnd(',');
                                            tenantManagement.RequestedAirlines = tenantManagement.RequestedAirlines.TrimStart(',');
                                            tenantManagement.RequestedAirlines = tenantManagement.RequestedAirlines.Replace(" ,", "");
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(tenantManagement.PendingAirlines))
                                    {
                                        if (tenantManagement.PendingAirlines.Contains(myAirline.Card.Code))
                                        {
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Replace(myAirline.Card.Code, "");
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Trim();
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.TrimEnd(',');
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.TrimStart(',');
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Replace(" ,", "");
                                        }
                                    }
                                }

                                break;
                            }
                        #endregion

                        #region Decline
                        case "Dec":
                            {
                                bool isRequested = tenantManagement.AWBMessagesCCSTypeCode.ToUpper() == "GLSHK" ? myAirline.GLSHKRegistrationRequested : myAirline.ChampRegistrationRequested;
                                bool isRegistered = tenantManagement.AWBMessagesCCSTypeCode.ToUpper() == "GLSHK" ? myAirline.IsGLSHKRegistered : myAirline.IsChampRegistered;

                                if (isProcessing)
                                {
                                    if (!string.IsNullOrEmpty(tenantManagement.PendingAirlines))
                                    {
                                        if (tenantManagement.PendingAirlines.Contains(myAirline.Card.Code))
                                        {
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Replace(myAirline.Card.Code, "");
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Trim();
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.TrimEnd(',');
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.TrimStart(',');
                                            tenantManagement.PendingAirlines = tenantManagement.PendingAirlines.Replace(" ,", "");
                                        }
                                    }
                                }

                                else
                                {
                                    if (isRequested && !isRegistered)
                                    {
                                        if (string.IsNullOrEmpty(tenantManagement.PendingAirlines))
                                        {
                                            tenantManagement.PendingAirlines = myAirline.Card.Code;
                                        }

                                        else
                                        {
                                            if (!tenantManagement.PendingAirlines.Contains(myAirline.Card.Code))
                                            {
                                                tenantManagement.PendingAirlines = tenantManagement.PendingAirlines + ", " + myAirline.Card.Code;
                                            }
                                        }
                                    }
                                }

                                break;
                            }
                        #endregion
                    }

                    tenantManagementRepository.Update(tenantManagement);
                    tenantManagementRepository.SubmitChanges();
                }

                scope.Complete();
            }
        }
    }
}