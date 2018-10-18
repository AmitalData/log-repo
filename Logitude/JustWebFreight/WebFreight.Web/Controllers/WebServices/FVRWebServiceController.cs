using Logitude.XSD.FVR;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.WebServices
{
    public class FVRWebServiceController : ApiController
    {
        public HttpResponseMessage PostSendFVR(FVRServiceArgs args)
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

                    FVRManager myManager = new FVRManager(tenant);
                    FVRResultClass myResult = myManager.SendFVR(args.AirlineId, args.FromPortId, args.ToPortId, args.ETD, args.ETA, args.Volume, args.GrossWeight, args.VolumeUnitCode, args.GrossWeightUnitCode, args.ShipmentId, args.BookingId, args.Recipient);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSimulateXML(string xmlString, string myShipmentId, string myBookingId, bool isFNA)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    FVRManager myManager = new FVRManager(tenant);
                    FVASimulatorResult myResult = new FVASimulatorResult();

                    if (isFNA)
                    {
                        myResult = myManager.SimulateFNA(xmlString, myShipmentId, myBookingId);
                    }

                    else
                    {
                        myResult = myManager.SimulateXML(xmlString, myShipmentId, myBookingId);
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCopyFlightsSchedulesPorts(string myResponseIds)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Port", "READ", tenant);

                    List<string> allIds = new List<string>();
                    if (!string.IsNullOrEmpty(myResponseIds))
                    {
                        string[] ids = myResponseIds.Split(':');
                        foreach (string id in ids)
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                allIds.Add(id);
                            }
                        }
                    }

                    FVRManager myManager = new FVRManager(tenant);
                    List<FlightSchedulePort> myResult = myManager.CopyFlightsSchedulesPorts(allIds.ToArray());                       

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