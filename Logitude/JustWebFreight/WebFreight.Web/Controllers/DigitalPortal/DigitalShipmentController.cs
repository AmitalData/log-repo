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

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalShipmentController : ApiController
    {
        [HttpGet]
        [Route("DigitalShipment/GetSingle")]
        public HttpResponseMessage GetSingle(string id, string cardId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, id);
                id = shipmentIdAndTenant.Item1;
                var tenant = shipmentIdAndTenant.Item2;
                var shipmentQuery = new ShipmentQuery(tenant);
                var shipmentPM = shipmentQuery.GetSinglePM(id, tenant, cardId);
                if (shipmentPM.CustomerId == cardId || shipmentPM.AgentId == cardId || string.IsNullOrWhiteSpace(cardId))
                {
                    shipmentPM.TimeLineData = shipmentQuery.MapVerticalTimeLine(shipmentPM);
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                    return Request.CreateResponse(HttpStatusCode.OK, shipmentPM);
                }

                throw new AutenticationException("Sorry! you are not authorized to read data!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalShipment/GetByFilters")]
        public HttpResponseMessage GetByFilters(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                var myTenantRepository = new TenantRepository(authToken.Tenant);
                var myTenant = myTenantRepository.GetSingleTenant(authToken.Tenant);

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

                var ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", authToken.Tenant);

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

                ShipmentAPiHelper.AddFilters(queryOperations, authToken.Tenant);
                var shipmentRepository = new ShipmentRepository(authToken.Tenant);

                var customfilters = new ShipmentCustomFilter(authToken.Tenant);

                IQueryable<DigitalShipmentsDataView> shipments = shipmentRepository.GetDigitalShipmentViewsByTenant(authToken.Tenant);

                shipments = DigitalPortalCustomFilter.GetDigtalFilteredQuery(queryOperations, shipments, shipmentRepository, authToken.Tenant);

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
                var entityLists = myShipmentQuery.GetDigitalIQueryableShipmentList(shipments, authToken.Tenant);
                entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    var propInfo = typeof(DigitalShipmentList).GetProperty(queryOperations.SortByColumnName);
                    var shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", authToken.Tenant).ToList();

                    var objectField = shipmentObjectFields.FirstOrDefault(a => a.FieldName == queryOperations.SortByColumnName);

                    if (objectField != null)
                    {
                        var sortClass = new GenericSort();

                        if (!objectField.IsCustom)
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "text":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "lookup":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                default:
                                    {
                                        entityLists = entityLists.OrderByDescending(d => d.CreateDateTime);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderByDescending(d => d.CreateDateTime);
                }

                var response = new ServiceResponse();

                if (newFilters.GetCount)
                {
                    response.Count = entityLists.Count();
                }

                entityLists = QueryableExtensions.Skip(entityLists, () => queryOperations.PageIndex);
                entityLists = QueryableExtensions.Take(entityLists, () => queryOperations.PageSize);

                List<DigitalShipmentList> listQuery = entityLists.ToList();

                myShipmentQuery.BuildShipmentListWithTimeLine(listQuery, authToken.Tenant);

                response.Result = listQuery;

                var reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetShipmentActiveStatuses")]
        public IHttpActionResult GetShipmentActiveStatuses(string cardId)
        {
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                int tenant = authToken.Tenant;
                SecurityUtility.CheckDigitalUserAuthentication(tenant, cardId);
                var entityStatusQuery = new EntityStatusQuery(tenant);
                var digitalPortalActiveStatuses = entityStatusQuery.GetDigitalPortalActiveStatuses(tenant);
                var deliveryStatus = digitalPortalActiveStatuses.Where(c => c.Code.Equals("SDLY",StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (deliveryStatus != null)
                    deliveryStatus.DisplayName = "Out for Delivery";

                var orderStatus = digitalPortalActiveStatuses.Where(c => c.Code.Equals("SHOR", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (orderStatus != null)
                    orderStatus.DisplayName = "Created";

                return Ok(digitalPortalActiveStatuses);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetEntityEvents")]
        public IHttpActionResult GetEntityEvents(string entityId, string objectTableName, string cardType, string cardId)
        {
            var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
            var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, entityId);
            entityId = shipmentIdAndTenant.Item1;
            var tenant = shipmentIdAndTenant.Item2;
            var result = new List<TraceEventPM>();
            var objectTable = new ObjectTableRepository(tenant).GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                var traceEventsRepository = new TraceEventRepository(tenant);
                var traceEventQuery = new TraceEventQuery(traceEventsRepository);

                var dataQuery = traceEventQuery.GetTraceEventPMsByTenantByEntityId(tenant, entityId, objectTable.Id);

                var resultQuery = cardType == null 
                                  || cardType.Equals("AG", StringComparison.InvariantCultureIgnoreCase)
                                     ? dataQuery.Where(d => d.IsAgentView)
                                     : dataQuery.Where(d => d.IsCustomerView);

                return Ok(resultQuery.OrderByDescending(s => s.EventDateTime).ToList());
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("DigitalShipment/GetShipmentRoutingLegs")]
        public IHttpActionResult GetShipmentRoutingLegs(string shipmentId, string cardId)
        {
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, shipmentId);
                shipmentId = shipmentIdAndTenant.Item1;
                var tenant = shipmentIdAndTenant.Item2;
                var shipmentQuery = new ShipmentQuery(tenant);
                var routingLegs = shipmentQuery.GetDigitalShipmentRoutingLegs(shipmentId, tenant);
                return Ok(routingLegs);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

    }
}
