using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
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

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{
    public class HybridPartnerExtendedListController : ApiController
    {
        public HttpResponseMessage GetHybridPartnerLists(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int Tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(Tenant);
                var HybridPartnerQuery = new HybridPartnerQuery(tenant);

                var requestsList = HybridPartnerQuery.GetHybridPartnerLists(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, requestsList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetHybridPartnerListWithNoRequest(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int Tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(Tenant);
                var HybridPartnerQuery = new HybridPartnerQuery(tenant);

                var requestsList = HybridPartnerQuery.GetHybridPartnerListWithNoRequest(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, requestsList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetAllowdHybridPartnerLists(string hybridPartnerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("HybridPartner", "READ", authToken.Tenant);

                var HybridPartnerQuery = new HybridPartnerQuery(tenant);

                var requestsList = HybridPartnerQuery.GetAllowdHybridPartnerLists(hybridPartnerId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, requestsList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAllowingHybridPartnerLists(string hybridPartnerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("HybridPartner", "READ", authToken.Tenant);
                var HybridPartnerQuery = new HybridPartnerQuery(tenant);

                var requestsList = HybridPartnerQuery.GetAllowingHybridPartnerLists(hybridPartnerId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, requestsList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}