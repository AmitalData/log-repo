using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using Simplog.Data.InvoiceModel.Repositories;
using WebFreight.Web.Controllers.InvoiceModel.ApiHelpers;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.CustomFilters;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Simplog.Data.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDashboardController : ApiController
    {
        [HttpPost]
        [Route("DigitalDashboardController/GetInvoicesGroupedByPaidStatus")]
        public IHttpActionResult GetInvoicesGroupedByPaidStatus(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                var invoices = GetFilteredInvoicesList(newFilters, authToken.Tenant);
                var invoicesGroupedByStatus = invoices.Where(r => !string.IsNullOrEmpty(r.PaidStatus))
                                                      .GroupBy(r => r.PaidStatus)
                                                      .ToDictionary(t => t.Key, t => t.Count());

                return Ok(invoicesGroupedByStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPost]
        [Route("DigitalDashboardController/GetShipmentsGroupedByStatus")]
        public IHttpActionResult GetShipmentsGroupedByStatus(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                var shipments = GetFilteredShipmentList(newFilters, authToken.Tenant);
                var shipmentsGroupedByStatus = shipments.Where(r => !string.IsNullOrEmpty(r.StatusCode))
                                                        .GroupBy(r => r.StatusCode)
                                                        .ToDictionary(t => t.Key, t => t.Count());

                return Ok(shipmentsGroupedByStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPost]
        [Route("DigitalDashboardController/GetDashboardSummary")]
        public IHttpActionResult GetDashboardSummary(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                var dashboardSummary = new DigitalDashboardSummary();
                var currentDateTime = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant).Date;
                var currentWeek = currentDateTime.AddDays(7).Date;
                var shipments = GetFilteredShipmentList(newFilters, authToken.Tenant);

                dashboardSummary.TotalShipmentsByETACount = shipments.Where(d=> d.MainCarriageETA >= currentDateTime
                                                                                && d.MainCarriageETA <= currentWeek)
                                                                     .Count();

                var invoices = GetFilteredInvoicesList(newFilters, authToken.Tenant);
                dashboardSummary.TotalInvoicesByDueDateCount = invoices.Where(d => d.DueDate >= currentDateTime 
                                                                                    && d.DueDate <= currentWeek)
                                                                       .Count();
               
                return Ok(dashboardSummary);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        #region Private methods 
        private IQueryable<DigitalShipmentList> GetFilteredShipmentList(GeneralFilters newFilters, int tenant)
        {
            var myTenantRepository = new TenantRepository(tenant);
            var myTenant = myTenantRepository.GetSingleTenant(tenant);

            var filters = new ApiQueryFilters()
            {
                Filter1Value = newFilters.CardId,
                Filter2Value = newFilters.CardType
            };

            var queryOperations = new QueryOperations()
            {
                ObjectTableName = "Shipment",
                QuerySection = "Shipments",
                SortByColumnName = newFilters.SortBy,
                SortDirectin = newFilters.SortDirection,
                QueryFilterItems = new List<QueryFilterItem>(),
            };

            string partnerTypeName = string.Empty;
            string shipmentLevelCodeValue = string.Empty;

            if (newFilters.CardType == "CS")
            {
                shipmentLevelCodeValue = "D,H,A";
                partnerTypeName = "CustomerId";
            }
            else if (newFilters.CardType == "AG")
            {
                shipmentLevelCodeValue = "D,C";
                partnerTypeName = "AgentId";
            }

            if (!string.IsNullOrEmpty(newFilters.CardId))
            {
                queryOperations.SetFilter(partnerTypeName, newFilters.CardId, false, "Equals", null, false);
            }

            if (!string.IsNullOrEmpty(shipmentLevelCodeValue))
            {
                queryOperations.SetFilter("ShipmentLevelCode", shipmentLevelCodeValue, false, "InListExact", null, false);
            }

            var ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant);

            ShipmentAPiHelper.AddFilters(queryOperations, tenant);
            var shipmentRepository = new ShipmentRepository(tenant);

            var customfilters = new ShipmentCustomFilter(tenant);

            IQueryable<DigitalShipmentsDataView> shipments = shipmentRepository.GetDigitalShipmentViewsByTenant(tenant);

            shipments = Logitude.BL.ShipmentsModel.CustomFilters.DigitalPortalCustomFilter.GetDigtalFilteredQuery(queryOperations, shipments, shipmentRepository, tenant);

            var nonListQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList()
            };

            var listQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList()
            };

            var genericFilter = new GenericFilter();

            shipments = genericFilter.GetFilteredQuery(nonListQueryOperation, shipments);

            var myShipmentQuery = new ShipmentQuery(shipmentRepository);
            var entityLists = myShipmentQuery.GetDigitalIQueryableShipmentList(shipments, tenant);
            entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

            return entityLists;
        }
        private IQueryable<ARInvoiceList> GetFilteredInvoicesList(GeneralFilters newFilters, int tenant)
        {
            var myTenantRepository = new TenantRepository(tenant);
            var myTenant = myTenantRepository.GetSingleTenant(tenant);

            var filters = new ApiQueryFilters()
            {
                Filter1Value = newFilters.CardId,
                Filter2Value = newFilters.CardType
            };

            var queryOperations = new QueryOperations()
            {
                ObjectTableName = "ARInvoice",
                QuerySection = "ARInvoices",
                SortByColumnName = newFilters.SortBy,
                SortDirectin = newFilters.SortDirection
            };

            queryOperations.SetFilter("IsPrinted", true, false, "Equals", null, false);
            queryOperations.SetFilter("IsConstituentInvoice", false, false, "Equals", null, false);

            var cardFilterValues = newFilters.CardId;
            if (!string.IsNullOrWhiteSpace(cardFilterValues))
            {
                var cardBillToId = GetCardBillToId(newFilters.CardId, tenant);
                if (!string.IsNullOrWhiteSpace(cardBillToId))
                {
                    cardFilterValues = cardFilterValues + "," + cardBillToId;
                    queryOperations.SetFilter("PartnerId", newFilters.CardId, false, "Equals", null, false);
                }

                queryOperations.SetFilter("BillToId", cardFilterValues, false, "InList", null, false);
            }

            var ARInvoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARInvoice", tenant);

            ARInvoiceAPiHelper.AddFilters(queryOperations, tenant);
            var genericFilter = new GenericFilter();
            var MyContext = InvoiceContext.GetContext(tenant);

            var nonListQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList()
            };

            var listQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList()
            };

            var aRInvoiceRepository = new ARInvoiceRepository(MyContext);
            var aRInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);

            var entityPocos = aRInvoiceRepository.GetARInvoices(tenant);

            entityPocos = aRInvoiceRepository.FilterInvoicesStatusesForList(entityPocos);

            var customfilters = new ARInvoiceCustomFilter(tenant);
            entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);
            entityPocos = ARInvoiceAPiHelper.ApplyFilters(entityPocos, tenant);
            entityPocos = genericFilter.GetFilteredQuery(nonListQueryOperation, entityPocos);

            var entityLists = aRInvoiceQuery.GetIQueryableEntityList(entityPocos);
            entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

            return entityLists;

        }
        private string GetCardBillToId(string cardId, int tenant)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            var cardBillToId = cardRepository.GetBillToCardById(cardId, tenant);
            return cardBillToId;
        }

        #endregion Private methods 

    }
}