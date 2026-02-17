using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.XSD.FSR;
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
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.WebServices
{
    public class FSRWebServiceController : ApiController
    {
        public HttpResponseMessage GetSendFSR(string shipmentId, string objectTableId, string myRecipient)
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

                    FSRManager myManager = new FSRManager(tenant, loggedUserEmail);
                    FSRResultClass myResult = myManager.SendFSR(shipmentId, objectTableId, myRecipient);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendFSRShipment(ShipmentPM entityPM)
        {
            try
            {
                // Ayman: this Transaction will be Aborted!

                //using (TransactionScope scope = TransactionFactory.GetTransaction())
                //{
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                FSRManager fSRManager = new FSRManager(tenant, loggedUserEmail);
                FSRResultClass myResult = fSRManager.SendShipmentFSR(entityPM);

                //scope.Complete();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                //}
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSendBookingFSR(string bookingId, string objectTableId, string myRecipient)
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

                    FSRWebService webService = new FSRWebService();
                    FSRResultClass myResult = webService.SendRequest(bookingId, objectTableId, myRecipient, tenant);

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