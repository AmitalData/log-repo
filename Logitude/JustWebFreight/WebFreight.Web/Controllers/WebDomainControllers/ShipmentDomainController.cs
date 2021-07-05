using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityOtherServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
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
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
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
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
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
        public HttpResponseMessage GetCustomerCreditLimitDetails(string customerId, string quoteId, bool isBuildFromQuote)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                var limitWarningMsg = ShipmentValidating.GetCustomerCreditLimitDetails(customerId, quoteId, isBuildFromQuote, tenant);
                
                return Request.CreateResponse(HttpStatusCode.OK, limitWarningMsg);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

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
                    bool hasEBookingInProgressFeature = SecurityUtility.CheckTableContactFeature("Shipment", "Shipment.Q.EBookingInProgress", tenant);

                    //ShipmentsSummary myResult= RunStoredProcedureClass.GetShipmentsCounts(tenant, myDirectionId, myTransportModeId, loggedUserEmail, hasETDFeature, hasFollowupsFeature);

                    string loggedContactId = null;
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                    if (loggedContact != null)
                    {
                        loggedContactId = loggedContact.Id;
                    }

                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);

                    myResult = shipmentQuery.GetShipmentsDashBoardSummary(tenant, myDirectionId, myTransportModeId, loggedContactId, hasETDFeature, hasFollowupsFeature, hasExpDepNotTransmittedFeature, hasShippingInstructionsLast7DaysFeature, hasContainerStatusLast7DaysFeature, hasEBookingInProgressFeature);
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

        [ActionName("PostValidateShipmentMasterArgs")]
        public HttpResponseMessage PostValidateShipmentMasterArgs(ValidateShipmentMasterArgs args)
        {
            try
            {
                string myResult = null;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int myTenant = authToken.Tenant;

                try
                {
                    ShipmentMasterIsUsedValidator validator = new ShipmentMasterIsUsedValidator();

                    validator.Validate(new ShipmentMasterIsUsedValidatorArgs()
                    {
                        Tenant = myTenant,
                        ShipmentId = args.ShipmentId,
                        BookingId = args.BookingId,
                        DirectionId = args.DirectionId,
                        TransportModeId = args.TransportModeId,
                        ShipmentLevelCode = args.ShipmentLevelCode,
                        Master = args.Master,
                        AirlinePrefix = args.AirlinePrefix,
                        IsCancelled = args.IsCancelled,
                        OperationalDate = args.OperationalDate,
                    });
                }

                catch (Exception ex)
                {
                    myResult = ex.Message;
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
        public HttpResponseMessage GetShipmentsQueriesCounts([FromUri] ShipmentsQueriesCountsArgs shipmentsQueriesCountsArgs)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(shipmentsQueriesCountsArgs.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", shipmentsQueriesCountsArgs.Tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentsQueriesCountsArgs.Tenant);
                ImporterQueriesDataCounts myResult = shipmentQuery.GetShipmentsQueriesCounts(shipmentsQueriesCountsArgs);

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

        public HttpResponseMessage GetMasterConnectedHouseShipments(string entityId, int tenant)
        {
            try
            {
                //SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("Shipment", "READ", tenant); 
                
                IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
                ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(myContext);
                List<Shipment> housesShipments = shipmentConsoleShipmentQuery.GetMasterConnectedHouseShipments(entityId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, housesShipments);
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
                                                      GrossWeightPerStorageDays= f.GrossWeightPerStorageDays,
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
                List<MessagingStockList> result = service.GetMessagingStockListForTenantManagmentTab(tenantManagementId).Where(d => d.StockType == "Champ").ToList();

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
                    IQuotesContext quotesContext = QuotesContext.GetContext(tenant);
                    QuoteRepository quoteRepository = new QuoteRepository(quotesContext);
                    QuoteComputedFieldRepository quoteComputedFieldRepository = new QuoteComputedFieldRepository(quotesContext);
                    Quote myQuote = quoteRepository.GetSingleQuote(myShipment.QuoteId, tenant);
                    QuoteComputedField quoteComputedField = quoteComputedFieldRepository.GetSingleQuoteComputedField(myShipment.QuoteId, tenant);
                    if (myQuote != null)
                    {
                        if (myQuote.UsageCount == 1)
                        {
                            myQuote.UsageCount = null;
                            if (quoteComputedField != null)
                            {
                                quoteComputedField.ConnectedToShipment = false;
                                quoteComputedFieldRepository.Update(quoteComputedField);
                                quoteComputedFieldRepository.SubmitChanges();
                            }
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
                    myShipment.QuoteNumber = null;
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

                    IShipmentsContext iContext = ShipmentsContext.GetContext(0);
                    ShipmentRepository iShipmentRepository = new ShipmentRepository(iContext);
                    ShipmentQuery iShipmentQuery = new ShipmentQuery(iShipmentRepository);

                    List<Shipment> iShipments = iShipmentRepository.GetMissingMasterDataShipments().ToList();

                    if (iShipments.Count > 0)
                    {
                        foreach (Shipment shipment in iShipments)
                        {
                            int tenant = shipment.Tenant;

                            bool isExist = (from d in iContext.ShipmentMasterDatas where d.Id == shipment.Id && d.Tenant == shipment.Tenant select d).Any();
                            if (isExist)
                            {
                                shipment.MasterShipmentDataId = shipment.Id;
                                iShipmentRepository.Update(shipment);
                            }

                            else
                            {
                                ShipmentPM shipmentPM = iShipmentQuery.GetSinglePM(shipment.Id, tenant);
                                shipmentPM.ConvertFromHouseToDirect = true;
                                //shipmentPM.DontCreateConvertEvent = true;

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

        public HttpResponseMessage GetDownloadShipmentPackages(string shipmentNumber, string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(tenant);
                PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);

                List<ShipmentPackage> shipmentPackages = shipmentPackageRepository.GetShipmentPackagesForShipmentTenant(shipmentId, tenant).ToList();
                List<PackageType> packageTypes = packageTypeRepository.GetPackageTypes(tenant).ToList();

                ExportToExcelHelper helper = new ExportToExcelHelper();
                byte[] data = this.ExportShipmentPackagesToExcel(shipmentPackages, packageTypes, tenant);

                string fileName = "Shipment-" + shipmentNumber + "-" + String.Format("{0:dd-MM-yyyy}", TenantServerConfigration.GetCurrentDateTime(tenant));

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileName,
                    FolderName = "others",
                    Extension = "xls",
                    Tenant = tenant,
                    FileSize = data.Length,
                };

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                storageservice.Write(data, fileInfo);

                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public byte[] ExportShipmentPackagesToExcel(List<ShipmentPackage> shipmentPackages, List<PackageType> packageTypes, int tenant)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(2);

            IWorksheet sheet1 = workbook.Worksheets[0];
            sheet1.Name = "Packages";
            sheet1.Range["A1:I1"].CellStyle.Font.Bold = true;
            sheet1.Range["A1:I1"].CellStyle.Font.Size = 10;
            sheet1.Range["A1:I1"].CellStyle.Font.FontName = "Calibri";
            sheet1.Range["A1:I1"].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range["A1:I1"].CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range["A1:I1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet1.Range["A1:G1"].ColumnWidth = 15;
            sheet1.Range["H1:I1"].ColumnWidth = 17;

            DataTable dataTable1 = new DataTable();
            dataTable1.Columns.Add("Container Type");
            dataTable1.Columns.Add("Container #");
            dataTable1.Columns.Add("Volume");
            dataTable1.Columns.Add("Gross Weight");
            dataTable1.Columns.Add("Tare");
            dataTable1.Columns.Add("Shipper Seal");
            dataTable1.Columns.Add("Carrier Seal");
            dataTable1.Columns.Add("Marks & Numbers");
            dataTable1.Columns.Add("Description");

            IWorksheet sheet2 = workbook.Worksheets[1];
            sheet2.Name = "Package Types";
            sheet2.Range["A1"].CellStyle.Font.Bold = true;
            sheet2.Range["A1"].CellStyle.Font.Size = 11;
            sheet2.Range["A1"].CellStyle.Font.FontName = "Calibri";
            sheet2.Range["A1"].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet2.Range["A1"].CellStyle.Color = System.Drawing.Color.Gray;
            sheet2.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            List<ExcelPackageType> types = new List<ExcelPackageType>();
            if (packageTypes != null && packageTypes.Count > 0)
            {
                types = (from a in packageTypes
                         where a.IsContainer == true
                         select new ExcelPackageType()
                         {
                             Code = a.Code,
                         }).ToList();
            }

            if (shipmentPackages != null && shipmentPackages.Count > 0)
            {
                foreach (ShipmentPackage package in shipmentPackages)
                {
                    DataRow row = dataTable1.NewRow();
                    row[0] = package.PackageType == null ? null : package.PackageType.Code;
                    row[1] = package.ContainerNumber;
                    row[2] = package.Volume;
                    row[3] = package.Weight;
                    row[4] = package.Tare;
                    row[5] = package.ShipperSeal;
                    row[6] = package.CarrierSeal;
                    row[7] = package.MarksAndNumbers;
                    row[8] = package.Description;
                    dataTable1.Rows.Add(row);
                }
            }

            sheet1.Range["A2:A80"].DataValidation.ListOfValues = types.Select(s => s.Code).ToArray();
            sheet1.Range["A2:A80"].DataValidation.IsSuppressDropDownArrow = false;

            DataTable dataTable2 = this.ConvertToDataTable(types);

            sheet1.ImportDataTable(dataTable1, true, 1, 1);
            sheet2.ImportDataTable(dataTable2, true, 1, 1);

            workbook.SaveAs(memory);

            return memory.ToArray();
        }
        private DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (table.Columns.Contains(prop.Name))
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public HttpResponseMessage GetIfConnectedEntryOrRelease(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                IWarehouseContext warehouseContext = WarehouseContext.GetContext(tenant);
                WarehouseEntryRepository warehouseEntryRepository = new WarehouseEntryRepository(warehouseContext);
                IQueryable<WarehouseEntry> warehouseEntries = warehouseEntryRepository.GetWarehouseEntriesByshipmentId(shipmentId, tenant);

                bool myResult = false;
                if (warehouseEntries.Count() > 0)
                {
                    myResult = true;
                }

                if (!myResult)
                {
                    WarehouseReleaseRepository warehouseReleaseRepository = new WarehouseReleaseRepository(warehouseContext);
                    IQueryable<WarehouseRelease> warehouseReleases = warehouseReleaseRepository.GetWarehouseReleasesByshipmentId(shipmentId, tenant);

                    if (warehouseReleases.Count() > 0)
                    {
                        myResult = true;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PostUploadExcelFile")]
        public HttpResponseMessage PostUploadExcelFile(ExcelPackageFilter filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                filter.Tenant = authToken.Tenant;
                byte[] fileData = Convert.FromBase64String(filter.FileData);

                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                IWorksheet sheet = workbook.Worksheets[0];

                List<ExcelPackage> packagesResult = this.BuildPackagesExcelLines(sheet, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, packagesResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private List<ExcelPackage> BuildPackagesExcelLines(IWorksheet sheet, int tenant)
        {
            List<ExcelPackage> myResult = new List<ExcelPackage>();

            PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);
            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count()];
                ExcelPackage excelPackage = new ExcelPackage();

                for (int i = 0; i < sheet.Columns.Count(); i++)
                {
                    rowData[i] = row.Cells[i].Value2.ToString();
                }

                if (rowData.Length > 0)
                {
                    if (!string.IsNullOrEmpty(rowData[0]))
                    {
                        string packageTypeCode = rowData[0].Trim();

                        PackageType packageType = packageTypeRepository.GetSinglePackageTypeByCode(packageTypeCode, tenant, true);
                        if (packageType != null)
                        {
                            excelPackage.ContainerTypeId = packageType.Id;
                            excelPackage.ContainerTypeCode = packageType.Code;
                            excelPackage.ContainerTypeName = packageType.EnglishName;
                            excelPackage.IsRefrigerated = packageType.IsRefrigerated;
                        }

                        else
                        {
                            excelPackage.HasErrors = true;
                        }
                    }

                    else
                    {
                        excelPackage.HasErrors = true;
                    }
                }

                if (rowData.Length > 1)
                {
                    if (!string.IsNullOrEmpty(rowData[1]))
                    {
                        string containerNumber = rowData[1].Trim();

                        if (containerNumber.Length > 20)
                        {
                            excelPackage.ContainerNumber = containerNumber.Substring(0, 20).ToUpper();
                        }

                        else
                        {
                            excelPackage.ContainerNumber = containerNumber.ToUpper();
                        }
                    }
                }

                if (rowData.Length > 2)
                {
                    if (!string.IsNullOrEmpty(rowData[2]))
                    {
                        string volume = rowData[2].Trim();
                        if (this.IsNumber(volume))
                        {
                            excelPackage.Volume = Convert.ToDouble(volume);
                        }

                        else
                        {
                            excelPackage.HasErrors = true;
                        }
                    }
                }

                if (rowData.Length > 3)
                {
                    if (!string.IsNullOrEmpty(rowData[3]))
                    {
                        string grossWeight = rowData[3].Trim();
                        if (this.IsNumber(grossWeight))
                        {
                            excelPackage.GrossWeight = Convert.ToDouble(grossWeight);
                        }

                        else
                        {
                            excelPackage.HasErrors = true;
                        }
                    }

                    else
                    {
                        excelPackage.HasErrors = true;
                    }
                }

                if (rowData.Length > 4)
                {
                    if (!string.IsNullOrEmpty(rowData[4]))
                    {
                        string tare = rowData[4].Trim();
                        if (this.IsNumber(tare))
                        {
                            excelPackage.Tare = Convert.ToDouble(tare);
                        }
                    }
                }

                if (rowData.Length > 5)
                {
                    if (!string.IsNullOrEmpty(rowData[5]))
                    {
                        string shipperSeal = rowData[5].Trim();

                        if (shipperSeal.Length > 15)
                        {
                            excelPackage.ShipperSeal = shipperSeal.Substring(0, 15);
                        }

                        else
                        {
                            excelPackage.ShipperSeal = shipperSeal;
                        }
                    }
                }

                if (rowData.Length > 6)
                {
                    if (!string.IsNullOrEmpty(rowData[6]))
                    {
                        string carrierSeal = rowData[6].Trim();

                        if (carrierSeal.Length > 15)
                        {
                            excelPackage.CarrierSeal = carrierSeal.Substring(0, 15);
                        }

                        else
                        {
                            excelPackage.CarrierSeal = carrierSeal;
                        }
                    }
                }

                if (rowData.Length > 7)
                {
                    if (!string.IsNullOrEmpty(rowData[7]))
                    {
                        string marks = rowData[7].Trim();

                        if (marks.Length > 350)
                        {
                            excelPackage.MarksAndNumbers = marks.Substring(0, 350);
                        }

                        else
                        {
                            excelPackage.MarksAndNumbers = marks;
                        }
                    }
                }

                if (rowData.Length > 8)
                {
                    if (!string.IsNullOrEmpty(rowData[8]))
                    {
                        string description = rowData[8].Trim();

                        if (description.Length > 2000)
                        {
                            excelPackage.Description = description.Substring(0, 2000);
                        }

                        else
                        {
                            excelPackage.Description = description;
                        }
                    }
                }

                myResult.Add(excelPackage);
            }

            return myResult;
        }
        private bool IsNumber(string text)
        {
            bool isNumber = false;

            if (!string.IsNullOrEmpty(text))
            {
                double value;
                if (Double.TryParse(text, out value))
                {
                    isNumber = true;
                }
            }

            return isNumber;
        }

        public HttpResponseMessage GetIfHouseConnectedToMaster(string houseId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                Shipment houseShipment = shipmentRepository.GetSingleShipment(houseId, tenant);
                bool myResult = false;

                if (houseShipment != null)
                {
                    if (!string.IsNullOrEmpty(houseShipment.MasterShipmentDataId))
                    {
                        myResult = true;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetBlockForTransfer(string allIdsString, string entityCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                if (!string.IsNullOrEmpty(allIdsString))
                {
                    List<string> ids = allIdsString.Split('.').ToList();
                    if (ids.Count > 0)
                    {
                        ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                        List<Shipment> shipments = this.GetFilteredShipments(ids, entityCode, tenant, shipmentRepository);
                        this.BlockShipmentsForTransfer(shipments, shipmentRepository);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private List<Shipment> GetFilteredShipments(List<string> ids, string entityCode, int tenant, ShipmentRepository shipmentRepository)
        {
            List<Shipment> shipments = new List<Shipment>();
            shipments = shipmentRepository.GetShipmentsListFromIdList(ids, tenant);

            if (entityCode == "Air")
            {
                shipments = shipments.Where(d => d.TransportModeId == "A").ToList();
            }

            else if (entityCode == "Ocean")
            {
                shipments = shipments.Where(d => d.TransportModeId == "O").ToList();
            }

            return shipments;
        }
        private void BlockShipmentsForTransfer(List<Shipment> shipments, ShipmentRepository shipmentRepository)
        {
            foreach (Shipment item in shipments)
            {
                item.LocalCustomsTransmissionsStatusCode = "BLOK";
                shipmentRepository.Update(item);
            }

            shipmentRepository.SubmitChanges();
        }

        public HttpResponseMessage GetSetAMANACStartDate(string entityCode, string myStartDateString)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticationOnTenant(tenant);

                        CustomsInterfaceSettingRepository customsInterfaceSettingRepository = new CustomsInterfaceSettingRepository(tenant);
                        CustomsInterfaceSetting customsInterfaceSetting = customsInterfaceSettingRepository.GetSingleCustomsInterfaceSetting(tenant, tenant);

                        if (myStartDateString == "null")
                        {
                            myStartDateString = null;
                        }

                        DateTime? myStartDate = DateHelper.GetDate(myStartDateString);
                        this.UpdateAMANACStartDates(customsInterfaceSetting, myStartDate, entityCode, customsInterfaceSettingRepository);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        private void UpdateAMANACStartDates(CustomsInterfaceSetting customsInterfaceSetting, DateTime? startDate, string entityCode, CustomsInterfaceSettingRepository customsInterfaceSettingRepository)
        {
            switch (entityCode)
            {
                case "Air":
                    {
                        customsInterfaceSetting.AMCAirStartDate = startDate;
                        break;
                    }

                case "Ocean":
                    {
                        customsInterfaceSetting.AMCOceanStartDate = startDate;
                        break;
                    }
            }

            customsInterfaceSettingRepository.Update(customsInterfaceSetting);
            customsInterfaceSettingRepository.SubmitChanges();
        }

        public HttpResponseMessage GetOnStartDateEntitiesIds(string entityCode, string myStartDateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                List<string> myResult = new List<string>();

                if (myStartDateString == "null")
                {
                    myStartDateString = null;
                }

                DateTime? myStartDate = DateHelper.GetDate(myStartDateString);

                if (SecurityUtility.CheckTableContactFeature("Shipment", "READ", tenant))
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                    IQueryable<Shipment> iQueryable_Data = shipmentRepository.GetShipments(tenant);
                    iQueryable_Data = iQueryable_Data.Where(d => !d.IsOperationalClosed && !d.IsAccountingClosed && !d.IsCancelled);

                    switch (entityCode)
                    {
                        case "Air":
                            {
                                iQueryable_Data = iQueryable_Data.Where(d => d.TransportModeId == "A");
                                break;
                            }

                        case "Ocean":
                            {
                                iQueryable_Data = iQueryable_Data.Where(d => d.TransportModeId == "O");
                                break;
                            }
                    }

                    iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), iQueryable_Data, tenant);
                    iQueryable_Data = iQueryable_Data.Where(d => d.LocalCustomsTransmissionsStatusCode == "NSEN");

                    if (myStartDate != null)
                    {
                        myStartDate = myStartDate.Value.Date;
                        iQueryable_Data = iQueryable_Data.Where(d => DbFunctions.TruncateTime(d.CreateDateTime) < myStartDate);
                    }

                    myResult = iQueryable_Data.Select(s => s.Id).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSendToAMANAC(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(authToken.Tenant);
                ShipmentDataView shipment = shipmentRepository.GetSingleShipmentDataView(shipmentId, authToken.Tenant);

                string fileName = "";
                if (shipment != null)
                {
                    string transferType = shipment.TransportModeId == "O" ? "AMOS" : "AMAS"; 

                    List<ShipmentDataView> shipments = new List<ShipmentDataView>();
                    shipments.Add(shipment);

                    fileName = "Shipment-" + shipment.ShipmentNumber + "-" + String.Format("{0:dd-MM-yyyy}", TenantServerConfigration.GetCurrentDateTime(authToken.Tenant));
                    CustomsTransferService customsTransferService = new CustomsTransferService(shipments, fileName, transferType, authToken.Tenant);
                    customsTransferService.Transfer();
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetMarkShipmentAsBlocked(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                Shipment shipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);

                if (shipment != null)
                {
                    shipment.LocalCustomsTransmissionsStatusCode = "BLOK";
                    shipmentRepository.Update(shipment);
                    shipmentRepository.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUnblockedShipment(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                Shipment shipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);

                if (shipment != null)
                {
                    shipment.LocalCustomsTransmissionsStatusCode = "NSEN";
                    shipmentRepository.Update(shipment);
                    shipmentRepository.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetShipmentsTransferSummary()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentTransferSummary myResult = new ShipmentTransferSummary();

                if (SecurityUtility.CheckTableContactFeature("Shipment", "READ", tenant))
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                    IQueryable<Shipment> shipments = shipmentRepository.GetShipments(tenant);

                    shipments = shipments.Where(d => !d.IsCancelled && !d.IsAccountingClosed && !d.IsOperationalClosed && d.TransportModeId != "I");
                    shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), shipments, tenant);

                    myResult.BlockedOceanShipmentsCount = shipments.Where(d => d.LocalCustomsTransmissionsStatusCode == "BLOK" && d.TransportModeId == "O").Count();
                    myResult.BlockedAirShipmentsCount = shipments.Where(d => d.LocalCustomsTransmissionsStatusCode == "BLOK" && d.TransportModeId == "A").Count();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRebuildTransferFile(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);

                CustomsTransferHeaderRepository entityRepository = new CustomsTransferHeaderRepository(myContext);
                CustomsTransferHeader entityPoco = entityRepository.GetSingleEntity(entityId, tenant);

                if (entityPoco != null)
                {
                    List<string> shipmentsIdsList = (from a in myContext.CustomsTransferLines
                                                     where a.Tenant == tenant
                                                     && a.CustomsTransferHeaderId == entityId
                                                     select a.ShipmentId).ToList();

                    ShipmentRepository shipmentRepository = new ShipmentRepository(myContext);
                    List<ShipmentDataView> shipments = shipmentRepository.GetShipmentsFromIdList(shipmentsIdsList, tenant);                    
                    CustomsTransferService customsTransferService = new CustomsTransferService(shipments, entityPoco.FileName, entityPoco.CustomsTransferTypeCode, tenant);
                    customsTransferService.Transfer();
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PostValidateAMANACShipmentsBeforeExporting")]
        public HttpResponseMessage PostValidateAMANACShipmentsBeforeExporting(CustomsTransferHeaderPM myEntity)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int myTenant = authToken.Tenant;

                if(myEntity.CustomsTransferLines.Count > 0)
                {
                    List<string> ids = myEntity.CustomsTransferLines.Select(s => s.ShipmentId).ToList();
                    ShipmentRepository shipmentRepository = new ShipmentRepository(myTenant);
                    List<ShipmentDataView>shipments = shipmentRepository.GetShipmentsFromIdList(ids, myTenant);

                    foreach (ShipmentDataView item in shipments)
                    {
                        CustomsTransferLinePM myLine = myEntity.CustomsTransferLines.Where(d => d.ShipmentId == item.Id).FirstOrDefault();
                        if (myLine != null)
                        {
                            if (myEntity.CustomsTransferTypeCode == "AMAS")
                            {
                                if (string.IsNullOrEmpty(item.AirlinePrefix) || string.IsNullOrEmpty(item.Master)
                                || string.IsNullOrEmpty(item.MainCarriageFromPortCode) || string.IsNullOrEmpty(item.MainCarriageFromPortName)
                                || string.IsNullOrEmpty(item.MainCarriageFinalDestinationPortCode) || string.IsNullOrEmpty(item.MainCarriageFinalDestinationPortName)
                                || string.IsNullOrEmpty(item.MainCarriageCarrierCode) || string.IsNullOrEmpty(item.MainCarriageCarrierName)
                                || item.NumberOfPackages == null || item.NumberOfPackages == 0
                                || item.GrossWeight == null || item.GrossWeight == 0
                                || (item.DirectionId == "E" && string.IsNullOrEmpty(item.ShipperId))
                                || (item.DirectionId == "I" && string.IsNullOrEmpty(item.ConsigneeId)))
                                {
                                    myLine.HasError = true;
                                    myLine.ErrorText = this.BuildAMANACErrorText(item, myEntity.CustomsTransferTypeCode);
                                }
                            }

                            else
                            {
                                if (string.IsNullOrEmpty(item.MainCarriageVesselName)
                                || string.IsNullOrEmpty(item.House)
                                || string.IsNullOrEmpty(item.MainCarriageCarrierNumber)
                                || string.IsNullOrEmpty(item.MainCarriageCarrierId)
                                || (item.DirectionId == "E" && string.IsNullOrEmpty(item.ShipperId))
                                || (item.DirectionId == "I" && string.IsNullOrEmpty(item.ConsigneeId)))
                                {
                                    myLine.HasError = true;
                                    myLine.ErrorText = this.BuildAMANACErrorText(item, myEntity.CustomsTransferTypeCode);
                                }
                            }
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myEntity);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private string BuildAMANACErrorText(ShipmentDataView item, string type)
        {
            string myResult = "";

            if (item.DirectionId == "E" && string.IsNullOrEmpty(item.ShipperId))
            {
                if (string.IsNullOrEmpty(myResult))
                {
                    myResult = "Missing Shipper";
                }
                else
                {
                    myResult = myResult + ", Missing Shipper";
                }
            }

            if (item.DirectionId == "I" && string.IsNullOrEmpty(item.ConsigneeId))
            {
                if (string.IsNullOrEmpty(myResult))
                {
                    myResult = "Missing Consignee";
                }
                else
                {
                    myResult = myResult + ", Missing Consignee";
                }
            }

            if (type == "AMAS")
            {
                if (!string.IsNullOrEmpty(item.AirlinePrefix) && !string.IsNullOrEmpty(item.Master))
                {
                    // nothing
                }
                else
                {
                    if(string.IsNullOrEmpty(item.AirlinePrefix) && string.IsNullOrEmpty(item.Master))
                    {
                        if (string.IsNullOrEmpty(myResult))
                        {
                            myResult = "Missing Master B/L";
                        }
                        else
                        {
                            myResult = myResult + ", Missing Master B/L";
                        }
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(myResult))
                        {
                            myResult = "Invalid Master B/L";
                        }
                        else
                        {
                            myResult = myResult + ", Invalid Master B/L";
                        }
                    }
                }

                if (string.IsNullOrEmpty(item.MainCarriageFromPortCode))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Origin Airport Code";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Origin Airport Code";
                    }
                }

                if (string.IsNullOrEmpty(item.MainCarriageFromPortName))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Origin Airport";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Origin Airport";
                    }
                }

                if (string.IsNullOrEmpty(item.MainCarriageFinalDestinationPortCode))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Destination Airport Code";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Destination Airport Code";
                    }
                }

                if (string.IsNullOrEmpty(item.MainCarriageFinalDestinationPortName))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Destination Airport";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Destination Airport";
                    }
                }

                if (string.IsNullOrEmpty(item.MainCarriageCarrierName))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Airline";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Airline";
                    }
                }

                if (item.NumberOfPackages == null || item.NumberOfPackages == 0)
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Pieces";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Pieces";
                    }
                }

                if (item.GrossWeight == null || item.GrossWeight == 0)
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Gross Weight";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Gross Weight";
                    }
                }
            }
            
            else
            {
                if (string.IsNullOrEmpty(item.MainCarriageVesselName))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Vessel";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Vessel";
                    }
                }

                if (string.IsNullOrEmpty(item.House))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing House";
                    }
                    else
                    {
                        myResult = myResult + ", Missing House";
                    }
                }

                if (string.IsNullOrEmpty(item.MainCarriageCarrierNumber))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Voyage";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Voyage";
                    }
                }

                if (string.IsNullOrEmpty(item.MainCarriageCarrierId))
                {
                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = "Missing Carrier";
                    }
                    else
                    {
                        myResult = myResult + ", Missing Carrier";
                    }
                }
            }

            return myResult;
        }

        public HttpResponseMessage GetNumberOfShipmentPackages(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ShipmentQuery query = new ShipmentQuery(tenant);
                var result = query.GetNumberOfShipmentPackages(tenant ,shipmentId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetPickupDeliveryValidForInlandDomestic(string pickupDeliveryId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                bool isValid = true;
                ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(tenant);
                ShipmentPickUpDelivery pickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, pickupDeliveryId);

                if(pickUpDelivery != null)
                {
                    List<DomesticCountry> iDomesticCountries = this.GetDomesticCountries(pickUpDelivery, tenant);

                    if (iDomesticCountries.GroupBy(g => g.CountryId).Count() > 1)
                    {
                        bool isAllPortsEC = iDomesticCountries.Where(d => d.CountryIsEC == false).Any() ? false : true;
                        bool isAllPortsNA = iDomesticCountries.Where(d => d.CountryIsNorthAmerica == false).Any() ? false : true;

                        if (!isAllPortsEC && !isAllPortsNA)
                        {
                            isValid = false;
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, isValid);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private List<DomesticCountry> GetDomesticCountries(ShipmentPickUpDelivery pickUpDelivery, int tenant)
        {
            List<DomesticCountry> iDomesticCountries = new List<DomesticCountry>();
            AddDomesticAddress(iDomesticCountries, pickUpDelivery.FromAddressId, tenant);
            AddDomesticAddress(iDomesticCountries, pickUpDelivery.ToAddressId, tenant);

            return iDomesticCountries;
        }
        private static void AddDomesticAddress(List<DomesticCountry> iDomesticCountries, string iAddressId, int iTenant)
        {
            if (!string.IsNullOrEmpty(iAddressId))
            {
                if (!iDomesticCountries.Where(d => d.Id == iAddressId).Any())
                {
                    AddressRepository addressRepository = new AddressRepository(iTenant);
                    Address iAddress = addressRepository.GetSingleAddress(iAddressId, iTenant);

                    if (iAddress != null)
                    {
                        iDomesticCountries.Add(new DomesticCountry()
                        {
                            Id = iAddress.Id,
                            CountryId = iAddress.CountryId,
                            CountryIsEC = iAddress.Country.EC,
                            CountryIsNorthAmerica = iAddress.Country.IsNorthAmerica,
                        });
                    }
                }
            }
        }
       
        public HttpResponseMessage GetIfShipmentPackageConnectedToPickUpDeliveryPackage(string containerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ShipmentPickUpDeliveryPackageRepository shipmentPickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(tenant);
                var shipmentPickUpDeliveryPackage = shipmentPickUpDeliveryPackageRepository.GetShipmentPickUpDeliveryPackagesByContainerIdAndTenant(containerId,tenant);
                var result = shipmentPickUpDeliveryPackage.Count == 0 ? false : true;

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetIfShipmentPackagesConnectedToStandAloneShipmentPackage(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ShipmentQuery query = new ShipmentQuery(tenant);
                bool result = query.IsShipmentPackagesConnectedToStandAlonePackage(shipmentId,tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}

public class ExcelPackage
{
    public string ContainerTypeId { get; set; }
    public string ContainerTypeCode { get; set; }
    public string ContainerTypeName { get; set; }
    public string ContainerNumber { get; set; }
    public double? Volume { get; set; }
    public double? GrossWeight { get; set; }
    public double? Tare { get; set; }
    public string ShipperSeal { get; set; }
    public string CarrierSeal { get; set; }
    public string MarksAndNumbers { get; set; }
    public string Description { get; set; }
    public bool IsRefrigerated { get; set; }
    public bool HasErrors { get; set; }
}
public class ExcelPackageFilter
{
    public int Tenant { get; set; }
    public string FileData{ get; set; }
    public string ShipmentId{ get; set; }
}
public class ExcelPackageType
{
    public string Code { get; set; }
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

public class ShipmentTransferSummary
{
    public int BlockedOceanShipmentsCount { get; set; }
    public int BlockedAirShipmentsCount { get; set; }
}