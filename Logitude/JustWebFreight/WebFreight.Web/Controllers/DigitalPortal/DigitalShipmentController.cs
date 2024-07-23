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
using Simplog.Server.Infrastructure.DataContracts.Models;
using System.Data.Entity;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;
using Logitude.BL.Helpers;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Simplog.Data.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.BL.GlobalModel.EntityQueries;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalShipmentController : ApiController
    {
        [HttpGet]
        [Route("DigitalShipment/GetSingle")]
        public HttpResponseMessage GetSingle(string id, string cardId, string objectTableId, string profileCode)
        {
            int tenant = 0;
            string email = "";
            try
            {
                List<string> cards = new List<string>();
                if (!string.IsNullOrWhiteSpace(cardId))
                {
                    cards = cardId?.Split(',').ToList();
                }

                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, id);
                id = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;

                var shipmentQuery = new ShipmentQuery(tenant);
                var shipmentPM = shipmentQuery.GetSingleDigitalPM(id, tenant, cardId);

                if (string.IsNullOrWhiteSpace(cardId) || cards.Contains(shipmentPM.CustomerId) || cards.Contains(shipmentPM.AgentId))
                {
                    var textCodeQuery = new DigitalTextCodeQueryService(0);

                    var allowedTableNames = new List<string> { "Shipment", "ShipmentPackage", "ShipmentPickUpDelivery" };

                    var objectFieldIds = textCodeQuery.GetDigitalTextCodesObjetTables(0)
                                                      .Where(a => allowedTableNames.Contains(a.ObjectTableName))
                                                      .Select(a => new 
                                                      {
                                                          a.ObjectTableId,
                                                          a.ObjectTableName 
                                                      })
                                                      .ToList();

                    var fields = new Dictionary<string, List<string>>();
                    var helper = new DigitalFieldSecuritesHelper();
                    foreach (var item in objectFieldIds)
                    {
                        var blockedFields = helper.GitDigitalSecuritesFeilds(item.ObjectTableId, profileCode, tenant, true)
                                                  .Where(a => !a.HasPermission)
                                                  .Select(a => a.FieldCode)
                                                  .ToList();

                        if (item.ObjectTableName.Equals("Shipment", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (blockedFields.Contains("Shipment.VolumeInCBM"))
                            {
                                blockedFields.Add("Shipment.VolumeInCBF");
                            }
                            
                            if (blockedFields.Contains("Shipment.ChargeableWeightInKG"))
                            {
                                blockedFields.Add("Shipment.ChargeableWeightInLB");
                            }
                            
                            if (blockedFields.Contains("Shipment.GrossWeightInKG"))
                            {
                                blockedFields.Add("Shipment.GrossWeightInLB");
                            }
                        }

                        if (blockedFields.Any())
                        {
                            fields.Add(item.ObjectTableName, blockedFields);
                        }
                    }

                    shipmentPM.TimeLineData = shipmentQuery.MapVerticalTimeLine(shipmentPM, fields, profileCode);
                    var shipmentPMJson = JsonConvert.SerializeObject(shipmentPM);
                    var temp = (JObject)JsonConvert.DeserializeObject(shipmentPMJson);

                    foreach (var item in fields)
                    {
                        temp.Descendants()
                        .OfType<JProperty>()
                        .Where(attr => (item.Value.Equals($"{item.Key}.{tenant}.{attr.Name}") 
                                           || item.Value.Equals($"{item.Key}.{attr.Name}"))
                                        ||(attr.Name.Contains(".") && item.Value.Contains($"{attr.Name}")))
                        .ToList()
                        .ForEach(attr => attr.Remove());
                    }
                    
                    var json = JsonConvert.SerializeObject(temp);
                    return Request.CreateResponse(HttpStatusCode.OK, json);
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
                var entityLists = shipmentQuery.GetByFilterWithSortingFilter(newFilters);

                var tenantquery = new TenantManagementQuery(tenant);
                var data = tenantquery.GetSinglePM(tenant);

                var response = new ServiceResponse();

                if (newFilters.GetCount)
                {
                    response.Count = entityLists.Count();
                }

                entityLists = QueryableExtensions.Skip(entityLists, () => newFilters.PageIndex);
                entityLists = QueryableExtensions.Take(entityLists, () => newFilters.PageSize);

                var allowedTableNames = new List<string> { "Shipment", "ShipmentPackage", "ShipmentPickUpDelivery" };

                var textCodeQuery = new DigitalTextCodeQueryService(0);
                var objectFieldIds = textCodeQuery.GetDigitalTextCodesObjetTables(0)
                                                  .Where(a => allowedTableNames.Contains(a.ObjectTableName))
                                                  .Select(a => new
                                                  {
                                                      a.ObjectTableId,
                                                      a.ObjectTableName
                                                  })
                                                  .ToList();

                var fields = new Dictionary<string, List<string>>();
                var helper = new DigitalFieldSecuritesHelper();
                foreach (var item in objectFieldIds)
                {
                    var blockedFields = helper.GitDigitalSecuritesFeilds(item.ObjectTableId, newFilters.ProfileCode, tenant, false)
                                              .Where(a => !a.HasPermission)
                                              .Select(a => a.FieldCode)
                                              .ToList();

                    if (blockedFields.Any())
                    {
                        fields.Add(item.ObjectTableName, blockedFields);
                    }
                }

                var allowedFieldSecurites = helper.GitDigitalSecuritesFeilds(newFilters.ObjectTableId, newFilters.ProfileCode, tenant, false)
                                                  .Where(a => a.HasPermission)
                                                  .Select(a => a.FieldCode.Replace($"Shipment.{tenant}.", ""))
                                                  .Select(a => a.Replace("Shipment.", ""))
                                                  .ToList();

                var fieldsToBeSelected = string.Join(",", allowedFieldSecurites);
                var shipments = entityLists.Select("new { " + fieldsToBeSelected + " }").ToDynamicList();

                var isAllShipmentsQuery = newFilters.AdditionalFilters.Where(a => a.FieldName == "AllShipments").Any();

                if (isAllShipmentsQuery)
                {
                    DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

                    var createdDateTime = currentDateTime.AddMonths(-(data.DPArchiveShipmentCreateFilter.HasValue ? data.DPArchiveShipmentCreateFilter.Value : 12));
                    var arrivalDateTime = currentDateTime.AddMonths(-(data.DPArchiveShipmentArrivalFilter.HasValue ? data.DPArchiveShipmentArrivalFilter.Value : 3));
                    var departureDateTime = currentDateTime.AddMonths(-(data.DPArchiveShipmentDepartFilter.HasValue ? data.DPArchiveShipmentDepartFilter.Value : 3));

                    shipments.Where(a => (helper.DoesPropertyExistInDynamic(a, "CreateDateTime") && a.CreateDateTime <= createdDateTime)
                                          || (helper.DoesPropertyExistInDynamic(a, "MainCarriageFinalDestinationATA") 
                                               && (a.MainCarriageFinalDestinationATA <= arrivalDateTime && (a.DirectionId == "I" || a.DirectionId == "D" || a.DirectionId == "R")))
                                          || (helper.DoesPropertyExistInDynamic(a, "MainCarriageATD") && (a.MainCarriageATD <= departureDateTime && a.DirectionId == "E")))
                            .ToList()
                            .ForEach(i => i.IsCustomerArchived = true);
                }

                CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
                customFieldResolver.SetCustomFieldsValues("Shipment", tenant, shipments.Cast<object>().ToList());

                var res = shipmentQuery.BuildShipmentListWithTimeLine(shipments, authToken.Tenant, fields);

                response.Result = res;

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
                    List<string> cardTypes = cardType?.Split(',').ToList<string>();
                    var resultQuery = cardType == null
                                         || cardTypes.Contains("AG")
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
        public HttpResponseMessage GetShipmentRoutingLegs(string shipmentId, string cardId, string profileCode)
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
                
                var textCodeQuery = new DigitalTextCodeQueryService(0);

                var allowedTableNames = new List<string> { "Trucker", "Shipment", "ShipmentPackage", "ShipmentPickUpDelivery" };

                var objectFieldIds = textCodeQuery.GetDigitalTextCodesObjetTables(0)
                                                  .Where(a => allowedTableNames.Contains(a.ObjectTableName))
                                                  .Select(a => new
                                                  {
                                                      a.ObjectTableId,
                                                      a.ObjectTableName
                                                  })
                                                  .ToList();

                var fields = new Dictionary<string, List<string>>();
                var helper = new DigitalFieldSecuritesHelper();
                foreach (var item in objectFieldIds)
                {
                    var blockedFields = helper.GitDigitalSecuritesFeilds(item.ObjectTableId, profileCode, tenant)
                                                      .Where(a => !a.HasPermission)
                                                      .Select(a => a.FieldCode)
                                                      .ToList();

                    if (blockedFields.Any())
                    {
                        fields.Add(item.ObjectTableName, blockedFields);
                    }
                }

                var routingLegs = shipmentQuery.GetDigitalShipmentRoutingLegs(shipmentId, tenant, fields);
                var routingLegsJson = JsonConvert.SerializeObject(routingLegs);
                var temp = (JArray)JsonConvert.DeserializeObject(routingLegsJson);
                foreach (JObject jObject in temp)
                {
                    foreach (var item in jObject)
                    {
                        temp.Descendants()
                        .OfType<JProperty>()
                        .Where(attr => (item.Value.Equals($"{item.Key}.{tenant}.{attr.Name}")
                                           || item.Value.Equals($"{item.Key}.{attr.Name}"))
                                        || (attr.Name.Contains(".") && item.Value.Contains($"{attr.Name}")))
                        .ToList()
                        .ForEach(attr => attr.Value = "");
                    }
                }

                var routingJsonResult = JsonConvert.SerializeObject(temp);
                return Request.CreateResponse(HttpStatusCode.OK, routingJsonResult);

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