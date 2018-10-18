


using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Testers.LoadTest;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsLoadTestController : ApiController
    {

        public HttpResponseMessage GetNewCustomFile(int tenant ,string ConsigneeId, string CustomerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                var createCustomFileService = new CreateCustomFileService();
                var shipmentAM = createCustomFileService.GetShipmentAM(ConsigneeId, CustomerId);
                var newFileNo = createCustomFileService.SendHybridInterface(shipmentAM);
                var myJsonObj = new { newFileNo = newFileNo };
                return Request.CreateResponse(HttpStatusCode.OK, myJsonObj);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDeclarationFromFileNo(int tenant, string fileNo,string filingCopy)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                var createCustomFileService = new CreateCustomFileService();
                var returnFileNo = createCustomFileService.DeclarationUpsert(fileNo, filingCopy);
                
                var myJsonObj = new { returnFileNo = returnFileNo };
                return Request.CreateResponse(HttpStatusCode.OK, myJsonObj);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTicket(int tenant, string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                var createCustomFileService = new CreateCustomFileService();
                //string returnFileNo = 
                createCustomFileService.ConnectTicket(tenant, declarationId);

                var myJsonObj = new { returnFileNo = "Ok" };
                return Request.CreateResponse(HttpStatusCode.OK, myJsonObj);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}