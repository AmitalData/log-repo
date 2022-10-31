using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using System.Data.Entity;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.SystemLogs;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalShipmentController : ApiController
    {
        [HttpGet]
        [Route("DigitalShipment/GetSingle")]
        public HttpResponseMessage GetSingle(string id, string cardId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, id);
                id = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;

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
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalShipment/GetByFilters")]
        public HttpResponseMessage GetByFilters(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = string.Empty;
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                tenant = authToken.Tenant;
                email = authToken.Email;
                newFilters.Tenant = authToken.Tenant;
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var entityLists = shipmentQuery.GetByFilters(newFilters);
                var response = new ServiceResponse();

                if (newFilters.GetCount)
                {
                    response.Count = entityLists.Count();
                }

                entityLists = QueryableExtensions.Skip(entityLists, () => newFilters.PageIndex);
                entityLists = QueryableExtensions.Take(entityLists, () => newFilters.PageSize);

                List<DigitalShipmentList> listQuery = entityLists.ToList();

                shipmentQuery.BuildShipmentListWithTimeLine(listQuery, authToken.Tenant);

                response.Result = listQuery;

                var reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetShipmentActiveStatuses")]
        public HttpResponseMessage GetShipmentActiveStatuses(string cardId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);
                tenant = authToken.Tenant;
                email = authToken.Email;
                var entityStatusQuery = new EntityStatusQuery(tenant);
                var res = entityStatusQuery.GetDigitalPortalActiveStatuses(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetEntityEvents")]
        public HttpResponseMessage GetEntityEvents(string entityId, string objectTableName, string cardType, string cardId)
        {
            int tenant = 0;
            string email = "";

            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, entityId, true, true);
                entityId = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;
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

                    return Request.CreateResponse(HttpStatusCode.OK, resultQuery.OrderByDescending(s => s.EventDateTime).ToList());
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetShipmentRoutingLegs")]
        public HttpResponseMessage GetShipmentRoutingLegs(string shipmentId, string cardId)
        {
            int tenant = 0;
            string email = "";

            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, shipmentId);
                shipmentId = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;
                var shipmentQuery = new ShipmentQuery(tenant);
                var routingLegs = shipmentQuery.GetDigitalShipmentRoutingLegs(shipmentId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, routingLegs);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPut]
        [Route("DigitalShipment/PutShipmentDigitalField")]
        public HttpResponseMessage PutShipmentDigitalField(ShipmentDigitalArchivedArgs archivedrgs)
        {
            int tenant = 0;
            string email = "";

            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(archivedrgs.CardId, archivedrgs.ShipmentId);
                var shipmentId = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;
                ShipmentDigitalFieldRepository shipmentDigitalFieldRepository = new ShipmentDigitalFieldRepository(tenant);
                var shipmentDigitalField = shipmentDigitalFieldRepository.GetSingleShipmentDigitalFields(shipmentId, tenant);
                shipmentDigitalField.IsCustomerArchived = archivedrgs.IsCustomerArchived;
                shipmentDigitalFieldRepository.Update(shipmentDigitalField);
                shipmentDigitalFieldRepository.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, shipmentDigitalField);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetShipmentDigitalTransports")]
        public HttpResponseMessage GetShipmentDigitalTransports(string cardId)
        {
            int tenant = 0;
            string email = "";

            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;            
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var routingLegs = shipmentQuery.GetTransportModesWithSubTypes(authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, routingLegs);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}