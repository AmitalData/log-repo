using Logitude.XSD;
using Logitude.XSD.DataContracts;
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

namespace WebFreight.Web.Controllers.WebServices
{
    public class CCSWebServiceController : ApiController
    {
        public HttpResponseMessage GetMessageResult(string myShipmentId, string myRecipient, bool isSendingCargonaut, bool isSendingDEXX)
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

                    CCSHelper myCCSHelper = new CCSHelper(myShipmentId, tenant, myRecipient, isSendingCargonaut, isSendingDEXX);

                    myCCSHelper.Run();

                    CCSResult myResult = myCCSHelper.Result;

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetFHLsValidation(string myMasterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                AWBValidator validator = new AWBValidator(tenant);

                List<FHLShipmentValidator> myResult = validator.GetFHLsValidation(myMasterId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSendingValidations(string myShipmentId, string myRecipient, bool isSendingFHLs, bool isSendingCargonaut, bool isSendingDEXX, string mainCarriageCarrierId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                AWBValidator validator = new AWBValidator(tenant);

                AWBResultClass myResult = validator.GetSendingValidating(myShipmentId, myRecipient, isSendingFHLs, isSendingCargonaut, isSendingDEXX, mainCarriageCarrierId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAWBPrintingStock(string myShipmentId, bool isCargonautSending, bool isDEXXSending, bool isConfirmedByUser)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                AWBPrintingManager myPrintingManager = new AWBPrintingManager();
                AWBPrintResult myResult = myPrintingManager.GetPrintingResult(myShipmentId, isCargonautSending, isDEXXSending, isConfirmedByUser, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}