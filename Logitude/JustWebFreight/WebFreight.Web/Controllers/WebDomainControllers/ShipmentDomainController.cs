using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.XSD.Artemus;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Security;
using WebFreight.Web.ShipmentsModel.DomainServices;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class ShipmentDomainController : ApiController
    {
        public HttpResponseMessage GetRecentShipments()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ContactQuery contactQuery = new ContactQuery(tenant);

                string loggedContactId = null;
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (contact != null)
                {
                    loggedContactId = contact.Id;
                }

                string objectTableId = null;
                ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
                ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Shipment", 0, true);
                if (objecttable != null)
                {
                    objectTableId = objecttable.Id;
                }

                IQueryable<ShipmentList> first = shipmentQuery.GetLastActivityShipments(tenant, loggedContactId, objectTableId).AsQueryable();
                IQueryable<ShipmentList> list = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);

                if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
                {
                    list = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), first, tenant);
                }

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentsCounts(string myDirectionId, string myTransportModeId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                ShipmentsSummary myResult = new ShipmentsSummary() { Id = tenant };

                bool isCloudDeployment = false;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    SettingRepository settingRepository = new SettingRepository();
                    Setting setting = settingRepository.GetSingleSetting("1");
                    if (setting != null)
                    {
                        if (setting.DeploymentStage != null)
                        {
                            if (setting.DeploymentStage.ToLower() == "amitalstorage")
                            {
                                isCloudDeployment = true;
                            }
                        }
                    }

                    scope.Complete();
                }

                if (isCloudDeployment == false)
                {
                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                    if (myDirectionId == "All")
                    {
                        myDirectionId = null;
                    }

                    if (myTransportModeId == "All")
                    {
                        myTransportModeId = null;
                    }

                    bool hasETDFeature = SecurityUtility.CheckTableContactFeature("Shipment", "EXPECTEDDEPATURE", tenant);
                    bool hasFollowupsFeature = SecurityUtility.CheckTableContactFeature("Shipment", "MYFOLLOWUPS", tenant) || SecurityUtility.CheckTableContactFeature("Shipment", "ALLFOLLOWUPS", tenant);
                    bool hasExpDepNotTransmittedFeature = SecurityUtility.CheckTableContactFeature("Shipment", "ExpectedDeparturesNotTransmitted", tenant);
                    bool hasShippingInstructionsLast7DaysFeature = SecurityUtility.CheckTableContactFeature("Shipment", "ShippingInstructionsLast7Days", tenant);
                    bool hasContainerStatusLast7DaysFeature = SecurityUtility.CheckTableContactFeature("Shipment", "ContainerStatusLast7Days", tenant);

                    //ShipmentsSummary myResult= RunStoredProcedureClass.GetShipmentsCounts(tenant, myDirectionId, myTransportModeId, loggedUserEmail, hasETDFeature, hasFollowupsFeature);

                    string loggedContactId = null;
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                    if (loggedContact != null)
                    {
                        loggedContactId = loggedContact.Id;
                    }

                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);

                    myResult = shipmentQuery.GetShipmentsDashBoardSummary(tenant, myDirectionId, myTransportModeId, loggedContactId, hasETDFeature, hasFollowupsFeature, hasExpDepNotTransmittedFeature, hasShippingInstructionsLast7DaysFeature, hasContainerStatusLast7DaysFeature);
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDeparturesArrivals(string myDirectionId, string myTransportModeId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                if (myDirectionId == "All")
                {
                    myDirectionId = null;
                }

                if (myTransportModeId == "All")
                {
                    myTransportModeId = null;
                }

                string loggedUserEmail = authToken.Email;
                string loggedContactId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);

                List<FlightSummary> myResult = shipmentQuery.GetShipmentsDashBoardDeparturesArrivals(tenant, myDirectionId, myTransportModeId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentCarrierStatuses(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentCarrierStatusRepository entityRepository = new ShipmentCarrierStatusRepository(tenant);
                ShipmentCarrierStatusQuery entityQuery = new ShipmentCarrierStatusQuery(entityRepository);

                List<ShipmentCarrierStatusList> myResult = entityQuery.GetShipmentCarrierStatusLists(entityId, tenant).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage CheckHousesOpenAmounts(string masterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ShipmentsDomainService shipmentDomainService = new ShipmentsDomainService();
                var myResult = shipmentDomainService.CheckHousesOpenAmounts(masterId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetLoggedTenantMessagingStockLists()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("MessagingStock", "READ", tenant);

                MessagingStockRepository messagingStockRepository = new MessagingStockRepository(tenant);
                MessagingStockQuery messagingStockQuery = new MessagingStockQuery(messagingStockRepository);

                IQueryable<MessagingStockDataView> iQueryable = messagingStockRepository.GetMessagingStockDataViewsByTenant(tenant);
                IQueryable<MessagingStockList> myResult = messagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetLoggedTenantMessagingStockUsageHistoryLists(string stockId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("MessagingStock", "READ", tenant);

                MessagingStockUsageHistoryRepository messagingStockUsageHistoryRepository = new MessagingStockUsageHistoryRepository(tenant);
                MessagingStockUsageHistoryQuery messagingStockUsageHistoryQuery = new MessagingStockUsageHistoryQuery(messagingStockUsageHistoryRepository);

                IQueryable<MessagingStockUsageHistory> iQueryable = messagingStockUsageHistoryRepository.GetMessagingStockUsageHistories(stockId, tenant);
                IQueryable<MessagingStockUsageHistoryList> myResult = messagingStockUsageHistoryQuery.GetIQueryableEntityList(iQueryable, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PostValidateShipmentMasterArgs(ValidateShipmentMasterArgs args)
        {
            try
            {
                string myResult = null;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int myTenant = authToken.Tenant;

                bool isFieldExists = ShipmentValidating.IsMasterFieldUsedByAnotherShipment(args.ShipmentId, args.Master, args.AirlinePrefix, args.DirectionId, args.TransportModeId, args.ShipmentLevelCode, args.IsCancelled, myTenant);
                if (isFieldExists)
                {
                    myResult = "Master field already used in another Shipment";
                }

                else
                {
                    isFieldExists = ShipmentValidating.IsMasterFieldUsedByAnotherBooking(args.BookingId, args.Master, args.AirlinePrefix, args.DirectionId, args.TransportModeId, args.ShipmentLevelCode, args.IsCancelled, myTenant);
                    if (isFieldExists)
                    {
                        myResult = "Master field already used in another Booking";
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentsQuotesCount()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;


                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                int result = 0;
                int shipmentsCount = shipmentRepository.GetShipmentsCount(tenant);

                QuoteRepository quoteRepository = new QuoteRepository(tenant);
                int quotesCount = quoteRepository.GetQuotesCount(tenant);
                result = shipmentsCount + quotesCount;

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentConsolidationPackages(string masterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                List<ShipmentPackagePM> myResult = new List<ShipmentPackagePM>();
                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                myResult = shipmentQuery.GetShipmentConsolidationPackages(masterId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentConnectedEntities(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                List<ShipmentConnectedEntity> myResult = new List<ShipmentConnectedEntity>();

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                Shipment myShipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);

                EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);
                IQueryable<EntityStatus> entityStatus = entityStatusRepository.GetEntityStatus(tenant);

                if (myShipment != null)
                {
                    #region  Warehouse Entry and Release

                    IWarehouseContext warehouseContext = WarehouseContext.GetContext(tenant);
                    WarehouseEntryRepository warehouseEntryRepository = new WarehouseEntryRepository(warehouseContext);
                    WarehouseReleaseRepository warehouseReleaseRepository = new WarehouseReleaseRepository(warehouseContext);

                    IQueryable<WarehouseEntry> warehouseEntryLists = warehouseEntryRepository.GetWarehouseEntriesByshipmentId(shipmentId, tenant);
                    IQueryable<WarehouseRelease> warehouseReleaseLists = warehouseReleaseRepository.GetWarehouseReleasesByshipmentId(shipmentId, tenant);

                    foreach (WarehouseEntry item in warehouseEntryLists.OrderByDescending(d => d.CreateDate))
                    {
                        myResult.Add(new ShipmentConnectedEntity()
                        {
                            EntityId = item.Id,
                            EntityType = "Cross Dock Entry",
                            Reference = item.EntryNumber,
                            ObjectTableName = "WarehouseEntry",
                            EntityStatus = warehouseContext.WarehouseEntryStatuses.Where(d => d.Code == item.StatusCode).FirstOrDefault().Name,
                            ActualDate = item.ActualEntryDate,
                            ExpectedDate = item.ExpectedEntryDate,
                        });
                    }

                    foreach (WarehouseRelease item in warehouseReleaseLists.OrderByDescending(d => d.CreateDate))
                    {
                        myResult.Add(new ShipmentConnectedEntity()
                        {
                            EntityId = item.Id,
                            EntityType = "Cross Dock Release",
                            Reference = item.ReleaseNumber,
                            ObjectTableName = "WarehouseRelease",
                            EntityStatus = warehouseContext.WarehouseReleaseStatuses.Where(d => d.Code == item.StatusCode).FirstOrDefault().Name,
                            ActualDate = item.ActualReleaseDate,
                            ExpectedDate = item.ExpectedReleaseDate,
                        });
                    }

                    #endregion

                    if (myShipment.ShipmentLevelCode == "H")
                    {
                        Shipment myMaster = shipmentRepository.GetSingleShipment(myShipment.MasterShipmentDataId, tenant);
                        if (myMaster != null)
                        {
                            myResult.Add(new ShipmentConnectedEntity()
                            {
                                EntityId = myMaster.Id,
                                EntityType = "Master",
                                Reference = myMaster.ShipmentNumber,
                                ObjectTableName = "Shipment",
                                EntityStatus = entityStatus.Where(d => d.Id == myMaster.StatusId).FirstOrDefault().Name,
                            });
                        }
                    }

                    if (myShipment.CustomFileId != null)
                    {
                        Shipment myCustomFile = shipmentRepository.GetSingleShipment(myShipment.CustomFileId, tenant);
                        if (myCustomFile != null)
                        {
                            myResult.Add(new ShipmentConnectedEntity()
                            {
                                EntityId = myCustomFile.Id,
                                EntityType = "Custom File",
                                Reference = myCustomFile.ShipmentNumber,
                                ObjectTableName = "Shipment",
                                EntityStatus = entityStatus.Where(d => d.Id == myCustomFile.StatusId).FirstOrDefault().Name,
                            });
                        }
                    }

                    if (myShipment.QuoteId != null)
                    {
                        IQuotesContext quotesContext = QuotesContext.GetContext(tenant);
                        QuoteRepository quoteRepository = new QuoteRepository(quotesContext);
                        Quote myQuote = quoteRepository.GetSingleQuote(myShipment.QuoteId, tenant);
                        if (myQuote != null)
                        {
                            string salesman = null;
                            if (!string.IsNullOrEmpty(myQuote.SalesmanUserId))
                            {
                                UserQuery userQuery = new UserQuery(tenant);
                                UserPM user = userQuery.GetSinglePM(myQuote.SalesmanUserId, tenant);
                                if (user != null)
                                {
                                    salesman = user.EnglishName;
                                }
                            }

                            myResult.Add(new ShipmentConnectedEntity()
                            {
                                EntityId = myQuote.Id,
                                EntityType = "Quote",
                                Reference = myQuote.QuoteNumber,
                                ObjectTableName = "Quote",
                                EntityStatus = quotesContext.QuoteStages.Where(d => d.Id == myQuote.StageId).FirstOrDefault().Name,
                                OpenDate = myQuote.OpenDate,
                                AcceptedDate = myQuote.AcceptedDate,
                                Salesman = salesman,
                            });
                        }
                    }

                    ICRMContext crmContext = CRMContext.GetContext(tenant);
                    TicketListQueryService listService = new TicketListQueryService(crmContext);
                    List<TicketList> myTickets = listService.GetTicketListByShipmentIdList(shipmentId, tenant);
                    CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                    customFieldResolver.SetCustomFieldsValues("Ticket", tenant, myTickets.Cast<object>().ToList());

                    foreach (TicketList item in myTickets)
                    {
                        myResult.Add(new ShipmentConnectedEntity()
                        {
                            EntityId = item.Id,
                            EntityType = "Ticket",
                            Reference = item.TicketNumber,
                            ObjectTableName = "Ticket",
                            EntityStatus = crmContext.TicketStages.Where(d => d.Id == item.StageId).FirstOrDefault().Name,
                        });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentsQueriesCounts(int tenant, string transportModeId, string SearchFilter, string serviceContextUser, string TypeCode = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ImporterQueriesDataCounts myResult = shipmentQuery.GetShipmentsQueriesCounts(tenant, transportModeId, SearchFilter, serviceContextUser, TypeCode);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }
        public HttpResponseMessage GetMasterReceivables(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                List<ShipmentReceivablePM> myResult = new List<ShipmentReceivablePM>();

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM masterPM = shipmentQuery.GetSinglePM(entityId, tenant);

                foreach (ShipmentReceivablePM item in masterPM.ShipmentReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "OAMT" && d.UnitPrice > 0))
                {
                    myResult.Add(item);
                }

                foreach (ConsoleShipmentPM item in masterPM.ShipmentConsoleShipments)
                {
                    ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(item.Id, tenant);
                    if (shipmentPM != null)
                    {
                        foreach (ShipmentReceivablePM rec in shipmentPM.ShipmentReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "OAMT" && d.UnitPrice > 0))
                        {
                            myResult.Add(rec);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetInvoiceOpenAmountReceivables(string invoiceTypeCode, string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                List<ShipmentReceivablePM> myResult = new List<ShipmentReceivablePM>();
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                ShipmentReceivableQuery shipmentReceivableQuery = new ShipmentReceivableQuery(tenant);
                Shipment shipment = shipmentRepository.GetSingleShipment(entityId, tenant);

                if (shipment != null)
                {
                    if (invoiceTypeCode == "MN")
                    {
                        foreach (ShipmentReceivablePM item in shipmentReceivableQuery.GetShipmentReceivablePMsByShipmentId(entityId, tenant).Where(d => d.ShipmentReceivableLineStatusCode == "OAMT"))
                        {
                            myResult.Add(item);
                        }

                        List<Shipment> housesShipments = shipmentRepository.GetHouseShipmentsForMaster(entityId, tenant);
                        foreach (Shipment house in housesShipments)
                        {
                            foreach (ShipmentReceivablePM item in shipmentReceivableQuery.GetShipmentReceivablePMsByShipmentId(house.Id, tenant).Where(d => d.ShipmentReceivableLineStatusCode == "OAMT"))
                            {
                                myResult.Add(item);
                            }
                        }
                    }

                    else
                    {
                        foreach (ShipmentReceivablePM item in shipmentReceivableQuery.GetShipmentReceivablePMsByShipmentId(entityId, tenant).Where(d => d.ShipmentReceivableLineStatusCode == "OAMT"))
                        {
                            myResult.Add(item);
                        }
                    }
                }

                myResult = myResult.Where(d => d.ARInvoiceLineId == null && d.ARInvoiceId == null && d.Quantity != null && d.UnitPrice != null).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetActivityStatus(string type, int lastMonths, int lastDays, int currentTenant, string customerid)
        {
            try
            {
                if (customerid == "null")
                    customerid = null;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(currentTenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", currentTenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(currentTenant);
                List<DashBoardClass> myResult = shipmentQuery.GetShipmentsByMonthDashBoard(type, lastMonths, lastDays, currentTenant, customerid);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }
        public HttpResponseMessage GetActivityStatusByType(string type, string FromDate, string ToDate, int currentTenant, string customerid, string directionId, string transportmodeid)
        {
            try
            {
                if (customerid == "null")
                    customerid = null;

                if (FromDate == "null")
                    FromDate = null;

                if (ToDate == "null")
                    ToDate = null;
                if (directionId == "null")
                    directionId = null;
                if (transportmodeid == "null")
                    transportmodeid = null;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(currentTenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", currentTenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(currentTenant);

                DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                if (FromDate == null)
                {
                    FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }


                DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                if (ToDateOBJ == null)
                {
                    ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }


                List<DashBoardClass> myResult = shipmentQuery.GetShipmentsByCreateOperationalDate(type, FromDateOBJ, ToDateOBJ, currentTenant, customerid, directionId, transportmodeid);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }
        public HttpResponseMessage GetShipmentByDirectionAndTransmode(string type, int lastMonths, int lastDays, int currentTenant, string customerid)
        {
            try
            {
                if (customerid == "null")
                    customerid = null;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(currentTenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", currentTenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(currentTenant);
                List<DashBoardClass> myResult = shipmentQuery.GetShipmentsByDirectionAndTransMode(type, lastMonths, lastDays, currentTenant, customerid).AsQueryable().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);



            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        public HttpResponseMessage GetShipmentByDirectionAndTransmodeCustom(string type, string FromDate, string ToDate, string customerid)
        {
            try
            {
                if (customerid == "null")
                    customerid = null;
                if (FromDate == "null")
                    FromDate = null;

                if (ToDate == "null")
                    ToDate = null;
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);

                DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                if (FromDate == null)
                {
                    FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }


                DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                if (ToDateOBJ == null)
                {
                    ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }

                List<DashBoardClass> myResult = shipmentQuery.GetShipmentsByDirectionAndTransModeCustom(type, FromDateOBJ, ToDateOBJ, tenant, customerid);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);



            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage GetShipmentsByTop10CountriesDashBoard(string type, int lastMonths, int lastDays, int measurment, int currentTenant, int top, bool includeOthers, string customerid, string directionId, string transmodeId)
        {
            try
            {
                if (directionId == null)
                    directionId = "";
                if (transmodeId == null)
                    transmodeId = "";


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(currentTenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", currentTenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(currentTenant);
                List<DashBoardClass> myResult = shipmentQuery.GetShipmentsByTop10CountriesDashBoard(type, lastMonths, lastDays, measurment, currentTenant, top, includeOthers, customerid, directionId, transmodeId).AsQueryable().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage GetShipmentsByTop10CountriesDashBoardCustom(string type, string FromDate, string ToDate, int measurment, int top, bool includeOthers, string customerid, string directionId, string transmodeId)
        {
            try
            {
                if (directionId == null)
                    directionId = "";
                if (transmodeId == null)
                    transmodeId = "";


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                if (FromDate == null)
                {
                    FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }


                DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                if (ToDateOBJ == null)
                {
                    ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }


                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                List<DashBoardClass> myResult = shipmentQuery.GetShipmentsByTop10CountriesDashBoardCustom2(type, FromDateOBJ, ToDateOBJ, measurment, tenant, top, includeOthers, customerid, directionId, transmodeId).AsQueryable().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage GetTop10DashBoardCustom(string type, string FromDate, string ToDate, int measurment, int currentTenant, int top, bool includeOthers, string directionId, string transportmodeid)
        {
            try
            {


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(currentTenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", currentTenant);

                DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                if (FromDate == null)
                {
                    FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }


                DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                if (ToDateOBJ == null)
                {
                    ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }


                ShipmentQuery shipmentQuery = new ShipmentQuery(currentTenant);
                List<DashBoardClass> myResult = shipmentQuery.GetTop10DashBoardCustom(type, FromDateOBJ, ToDateOBJ, measurment, currentTenant, top, includeOthers, directionId, transportmodeid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage GetTop10DashBoard(string type, int lastMonths, int lastDays, int measurment, int currentTenant, int top, bool includeOthers, string directionid, string transportmodeId)
        {
            try
            {


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(currentTenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", currentTenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(currentTenant);
                List<DashBoardClass> myResult = shipmentQuery.GetTop10DashBoard(type, lastMonths, lastDays, measurment, currentTenant, top, includeOthers, directionid, transportmodeId).AsQueryable().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        public HttpResponseMessage GetAllMasterHousesPayables(string allHousesIdsString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Master", "READ", tenant);

                List<ShipmentPayablePM> myResult = new List<ShipmentPayablePM>();
                ShipmentPayableQuery query = new ShipmentPayableQuery(tenant);

                if (!string.IsNullOrEmpty(allHousesIdsString))
                {
                    string[] housesIds = allHousesIdsString.Split('.');

                    foreach (string houseId in housesIds)
                    {
                        List<ShipmentPayablePM> housePayables = query.GetShipmentPayablePMsByShipment(houseId, tenant);

                        foreach (ShipmentPayablePM item in housePayables.Where(d => d.Quantity != null && d.UnitPrice != null))
                        {
                            myResult.Add(item);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllMasterHousesReceivables(string allHousesIdsString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Master", "READ", tenant);

                List<ShipmentReceivablePM> myResult = new List<ShipmentReceivablePM>();
                ShipmentReceivableQuery receivablesQuery = new ShipmentReceivableQuery(tenant);

                if (!string.IsNullOrEmpty(allHousesIdsString))
                {
                    string[] housesIds = allHousesIdsString.Split('.');

                    foreach (string houseId in housesIds)
                    {
                        List<ShipmentReceivablePM> houseReceivables = receivablesQuery.GetShipmentReceivablePMsByShipmentId(houseId, tenant);

                        foreach (ShipmentReceivablePM item in houseReceivables.Where(d => d.Quantity != null && d.UnitPrice != null))
                        {
                            myResult.Add(item);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetConnectedShipmentsByMasterIdAndTenant(string masterId, int currentTenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Master", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Master", "READ", tenant);
                var myResult = shipmentQuery.GetShipmentPMsByMasterIdAndTenant(masterId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentsCountByQuoteId(string quoteId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                int result = 0;
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                result = shipmentRepository.GetShipmentsCountByQuoteId(quoteId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentLevelCode(string myShipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                string myResult = shipmentRepository.GetShipmentLevelCode(myShipmentId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetInvoiceOpenAmountPayables(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);


                ShipmentPayableQuery shipmentPayableQuery = new ShipmentPayableQuery(tenant);

                List<ShipmentPayablePM> myResult = shipmentPayableQuery.GetInvoiceOpenAmountPayables(entityId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetPayableInvoices(string PayableId, string PayableParentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                APInvoiceLineQuery entityQuery = new APInvoiceLineQuery(tenant);
                List<PayableInvoiceClass> myResult = entityQuery.GetPayableInvoices(PayableId, PayableParentId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentsByQuoteId(string quoteId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentsByQuoteId(quoteId, tenant);

                IQueryable<ShipmentList> result = from f in shipments
                                                  select new ShipmentList()
                                                  {
                                                      Id = f.Id,
                                                      ShipmentViewId = f.Id,
                                                      Tenant = f.Tenant,
                                                      ShipmentNumber = f.ShipmentNumber,
                                                      ShipmentType = !string.IsNullOrEmpty(f.ShipmentTypeName) ? f.ShipmentTypeName + " " + f.ShipmentLevelName : f.ShipmentLevelName,
                                                      CreateDateTime = f.CreateDateTime,
                                                      Shipper = f.ShipperName,
                                                      Consignee = f.ConsigneeName,
                                                      DirectionId = f.DirectionId,
                                                      DirectionName = f.DirectionName,
                                                      TransportModeName = f.TransportModeName,
                                                      House = f.House,
                                                      TransportModeId = f.TransportModeId,
                                                      ChargeableWeightInKG = f.ChargeableWeightInKG,
                                                      ChargeableWeight = f.ChargeableWeight,
                                                      GrossWeight = f.GrossWeight,
                                                      ShipperReference1 = f.ShipperReference1,
                                                      Master = f.Master,
                                                      BranchId = f.BranchId,
                                                      DepartmentId = f.DepartmentId,
                                                      MainCarriageETA = f.MainCarriageETA,
                                                      MainCarriageATD = f.MainCarriageATD,
                                                      Routing = f.Routing,
                                                      FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                                                      ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                                                      FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                                                      FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                                                      FromPortCountry = f.MainCarriageFromPortCountryName,
                                                      ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                                                      ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                                                      ToPortCountry = f.MainCarriageToPortCountryName,
                                                      MasterShipmentDataId = f.MasterShipmentDataId,
                                                      BranchName = f.BranchName,
                                                      CustomerName = f.CustomerName,
                                                      GrossWeightInKG = f.GrossWeightInKG,
                                                      ShipmentLevelCode = f.ShipmentLevelCode,
                                                      ShipmentLevelName = f.ShipmentLevelName,
                                                      VolumetricWeight = f.VolumetricWeight,
                                                      AirlinePrefix = f.AirlinePrefix,
                                                      IncotermId = f.IncotermId,
                                                      VolumeInCBM = f.VolumeInCBM,
                                                      FromPortCountryCode = f.FromPortCountryCode,
                                                      ToPortCountryCode = f.ToPortCountryCode,
                                                      StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                                                      StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                                                      StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                                                      StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                                                      LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                                                  };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSingleShipmentPMByNumber(string shipmentNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM myResult = shipmentQuery.GetSingleShipmentPMByNumber(shipmentNumber, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSingleShipmentPMWithoutComposition(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM myResult = shipmentQuery.GetSinglePMWithoutComposition(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetBlockNewARInvoice(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                Shipment myShipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);
                if (myShipment != null)
                {
                    myShipment.IsNewARInvoiceBlocked = true;
                    shipmentRepository.Update(myShipment);
                    shipmentRepository.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetShipmentFullTextSearch([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Shipment",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Shipments",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    QueryFilterItems = new List<QueryFilterItem>(),
                    GetAll = filters.GetAll,
                };

                List<ObjectField> ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
                        ObjectField field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                FilterSerializer serializer = new FilterSerializer();
                byte[] arrayOfBytes = serializer.SerializeFilterItems(queryOperations);

                ShipmentsDomainService domain = new ShipmentsDomainService();
                var result = domain.GetShipmentFullTextSearch(arrayOfBytes, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetMessagingStockListForTenantManagmentTab(int tenantManagementId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ShipmentsDomainService service = new ShipmentsDomainService();
                IQueryable<MessagingStockList> result = service.GetMessagingStockListForTenantManagmentTab(tenantManagementId).Where(d => d.StockType == "Champ");

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSendToAESCustoms(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                string fileName = "aes.txt";

                FlatFileHelper helper = new FlatFileHelper(tenant);
                helper.MapAESFile(shipmentId, fileName);

                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetShipmentCustomsTransmissionByShipmnetId(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ShipmentCustomsTransmissionQuery query = new ShipmentCustomsTransmissionQuery(tenant);
                var temp = query.GetShipmentCustomsTransmissionPMsByShipmentId(shipmentId, tenant);
                List<ShipmentCustomsTransmissionPM> result = new List<ShipmentCustomsTransmissionPM>();
                if (temp != null)
                {
                    result = temp.ToList();
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetArtemusStatus(string shipmentNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                string xmlString1 = @"
                        <AMSResponse>
                            <response>
                                <code>INFO</code>
                                <description>Voyage with Voyage number 'V006' Save Sucessfully</description>
                                <element>Voyage</element>
                                <source>data</source>
                            </response>
                        </AMSResponse>
                        ";
                string xmlString2 = @"
                            <AMSResponse>
                                <response>
                                    <code>ERROR</code>
                                    <description>Voyage 'USAAU' does not exist.</description>
                                    <element>Voyage</element>
                                    <source>data</source>
                                </response>
                                <response>
                                    <code>ERROR</code>
                                    <description>Voyage 'USABE' does not exist.</description>
                                    <element>Voyage</element>
                                    <source>data</source>
                                </response>
                            </AMSResponse>
                            ";
                string xmlString3 = @"
                           <AMSResponse>
                                <response>
                                    <code>INFO</code>
                                    <description>Bill '453232' Saved succesfully</description>
                                    <element>BL</element>
                                    <source>data</source>
                                </response>
                            </AMSResponse>
                            ";
                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
                AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                {
                    Subject = "Voyage Response",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                    From = "Artemus",
                    Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                    MessageBody = Encoding.UTF8.GetBytes(xmlString1),
                    Status = "W",
                    Retries = 0,
                    ConnectedToEntity = false,
                    ConnectedToTenant = true,
                    Tenant = tenant,
                    FileSize = xmlString1.Length,
                };

                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                analyzeQueueReposiory.Add(analyzeQueue);
                analyzeQueueReposiory.SubmitChanges();
                ArtemusAnalyzer analyzer = new ArtemusAnalyzer(analyzeQueue, analyzeQueueReposiory);
                analyzer.Simulate(tenant, shipmentNumber, Encoding.UTF8.GetBytes(xmlString1), "Voyage");
                analyzer.Simulate(tenant, shipmentNumber, Encoding.UTF8.GetBytes(xmlString2), "Voyage");
                analyzer.Simulate(tenant, shipmentNumber, Encoding.UTF8.GetBytes(xmlString3), "BL");
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDisconnectQuote(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);

                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                Shipment myShipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);
                if (myShipment != null)
                {
                    QuoteRepository quoteRepository = new QuoteRepository(tenant);
                    Quote myQuote = quoteRepository.GetSingleQuote(myShipment.QuoteId, tenant);
                    if (myQuote != null)
                    {
                        if (myQuote.UsageCount == 1)
                        {
                            myQuote.UsageCount = null;
                        }

                        else
                        {
                            myQuote.UsageCount -= 1;
                        }

                        myQuote.LastUsageDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                        quoteRepository.Update(myQuote);
                        quoteRepository.SubmitChanges();

                        ContactQuery contactQuery = new ContactQuery(tenant);
                        string loggedContactId = null;
                        ContactPM contact = contactQuery.GetContactByEmailOnly(authToken.Email, tenant);
                        if (contact != null)
                        {
                            loggedContactId = contact.Id;
                        }

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = tenant,
                            EventTypeCode = "DCQT",
                            UserId = loggedContactId,
                            EntityId = myQuote.Id,
                            ObjectTableName = "Quote",
                            Notes = "Disconnected from Shipment#: " + myShipment.ShipmentNumber,
                        });
                    }

                    ShipmentReceivableRepository receivableRepository = new ShipmentReceivableRepository(shipmentsContext);
                    ShipmentPayableRepository payableRepository = new ShipmentPayableRepository(shipmentsContext);

                    List<ShipmentReceivable> receivables = receivableRepository.GetShipmentReceivablesByShipmentId(myShipment.Id, tenant);
                    List<ShipmentPayable> payables = payableRepository.GetShipemntPayablesByShipmentId(myShipment.Id, tenant);

                    if (receivables.Count > 0)
                    {
                        foreach (ShipmentReceivable item in receivables)
                        {
                            item.IsFromQuote = false;
                            item.QuoteChargeId = null;
                            item.QuoteSaleMinAmount = null;
                            item.QuoteSaleMaxAmount = null;
                            receivableRepository.Update(item);
                        }
                    }

                    if (payables.Count > 0)
                    {
                        foreach (ShipmentPayable item in payables)
                        {
                            item.IsFromQuote = false;
                            item.QuoteChargeId = null;
                            item.QuoteCostMinAmount = null;
                            item.QuoteCostMaxAmount = null;

                            payableRepository.Update(item);
                        }
                    }

                    myShipment.QuoteId = null;
                    shipmentRepository.Update(myShipment);

                    shipmentsContext.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCreateMissingMasterData()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;


                    IShipmentsContext iContext = ShipmentsContext.GetContext(tenant);
                    ShipmentRepository iShipmentRepository = new ShipmentRepository(iContext);
                    ShipmentQuery iShipmentQuery = new ShipmentQuery(iShipmentRepository);

                    List<Shipment> iShipments = iShipmentRepository.GetMissingMasterDataShipments(tenant).ToList();

                    if (iShipments.Count > 0)
                    {
                        foreach (Shipment shipment in iShipments)
                        {
                            bool isExist = (from d in iContext.ShipmentMasterDatas where d.Id == shipment.Id && d.Tenant == tenant select d).Any();
                            if (isExist)
                            {
                                shipment.MasterShipmentDataId = shipment.Id;
                                iShipmentRepository.Update(shipment);
                            }

                            else
                            {
                                ShipmentPM shipmentPM = iShipmentQuery.GetSinglePM(shipment.Id, tenant);
                                shipmentPM.ConvertFromHouseToDirect = true;
                                shipmentPM.DontCreateConvertEvent = true;

                                string systemEmail = "system@tenant" + tenant + ".com";
                                ShipmentService iShipmentService = new ShipmentService(iContext, shipmentPM, systemEmail);
                                iShipmentService.Update();
                            }
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}

public class ShipmentConnectedEntity
{
    public string EntityId { get; set; }
    public string EntityType { get; set; }
    public string Reference { get; set; }
    public string ObjectTableName { get; set; }
    public string EntityStatus { get; set; }
    public DateTime? ActualDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public DateTime? OpenDate { get; set; }
    public DateTime? AcceptedDate { get; set; }
    public string Salesman { get; set; }
}
