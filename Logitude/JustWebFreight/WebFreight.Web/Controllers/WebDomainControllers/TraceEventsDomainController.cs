using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TraceEventsDomainController : ApiController
    {
        public HttpResponseMessage GetTraceEventsForEntity(string objectTableId, string entityId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    TraceEventRepository traceEventsRepository = new TraceEventRepository(tenant);
                    TraceEventQuery traceEventQuery = new TraceEventQuery(traceEventsRepository);
                    IQueryable<TraceEventPM> myResult = traceEventQuery.GetTraceEventPMsByTenantByEntityId(tenant, entityId, objectTableId).OrderByDescending(s => s.LogDateTime);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Post(TraceEventsServiceArgs args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    AddManualTraceEventsHelper addManualTraceEventsHelper = new AddManualTraceEventsHelper(tenant);
                    NewTraceEventResult myResult = addManualTraceEventsHelper.Trace(args, loggedUserEmail);
                    
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutDeleteTraceEvent(TraceEventsServiceArgs args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    bool isShipment = false;
                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
                    ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(args.ObjectTableId, tenant, true);
                    if (objectTable != null)
                    {
                        if (objectTable.Name == "Shipment" || objectTable.Name == "Master")
                        {
                            isShipment = true;
                        }
                    }

                    if (isShipment)
                    {
                        ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                        ShipmentPM entityPM = shipmentQuery.GetSinglePM(args.EntityId, tenant);
                        if (entityPM != null)
                        {
                            ShipmentTracing.DeleteShipmentTraceEvent(entityPM, args.TraceEventId, tenant, args.IsExternal);
                        }
                    }

                    else
                    {
                        TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEvent(args.TraceEventId);
                        if (traceEvent != null)
                        {
                            traceEvent.Deleted = true;
                            traceEventRepository.Update(traceEvent);
                            traceEventRepository.SubmitChanges();
                        }
                    }

                    bool myResult = true;

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }        
    }
}