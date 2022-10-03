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
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using System.Data.Entity;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using WebFreight.Web.Controllers.InvoiceModel.ApiHelpers;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.CustomFilters;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityLists;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDashboardController : ApiController
    {
        [HttpGet]
        [Route("DigitalDashboardController/GetInvoicesGroupedByPaidStatus")]
        public IHttpActionResult GetInvoicesGroupedByPaidStatus(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                var invoices = GetFilteredInvoicesList(newFilters, authToken.Tenant);
                var invoicesGroupedByStatus = invoices?.Where(r => !string.IsNullOrEmpty(r.PaidStatus)).GroupBy(r => r.PaidStatus).ToList().ToDictionary(t => t.Key, t => t.Key.Count());

                return Ok(invoicesGroupedByStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpGet]
        [Route("DigitalDashboardController/GetShipmentsGroupedByStatus")]
        public IHttpActionResult GetShipmentsGroupedByStatus(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                var shipments = GetFilteredShipmentList(newFilters, authToken.Tenant);
                var shipmentsGroupedByStatus = shipments?.Where(r => !string.IsNullOrEmpty(r.StatusCode)).GroupBy(r => r.StatusCode).ToList().ToDictionary(t => t.Key, t => t.Key.Count());

                return Ok(shipmentsGroupedByStatus);
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
                PageIndex = newFilters.PageIndex,
                PageSize = newFilters.PageSize,
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

            foreach (var filter in newFilters.AdditionalFilters)
            {
                var field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                if (field != null)
                {
                    string valuestring1 = filter.FieldValue?.ToString();
                    object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                    string valuestring2 = filter.FieldValue2?.ToString();
                    object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                    queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                }
                else
                {
                    queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                }
            }

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
                PageIndex = newFilters.PageIndex,
                PageSize = newFilters.PageSize,
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

            if (newFilters.AdditionalFilters.Any())
            {
                foreach (var filter in newFilters.AdditionalFilters)
                {
                    var field = ARInvoiceObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                    if (field != null)
                    {
                        string valuestring1 = filter.FieldValue?.ToString();
                        object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                        string valuestring2 = filter.FieldValue2?.ToString();
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }

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