
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
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
    public class PortListViewsExtededController : ApiController
    {
        public HttpResponseMessage GetPortCopyToCurrentTenant(string id)
        {
            PortList entityList = null;
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Port", "READ", authToken.Tenant);
                PortQuery portQuery = new PortQuery(authToken.Tenant);
                entityList = portQuery.GetPortCopyToCurrentTenant(id, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, entityList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}