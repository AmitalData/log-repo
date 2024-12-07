using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class EntityResourceController : ApiController
    {

        public HttpResponseMessage GetEntityResourceByTableName(string objectTableName, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            tenant = authToken.Tenant;

            //SecurityUtility.AuthenticationOnTenant(tenant);
            byte[] zipfilebyte = null;
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, tenant, false);
            if (objectTable != null)
            {
                zipfilebyte = objectTable.EntityResource;
            }
            return Request.CreateResponse(HttpStatusCode.OK, zipfilebyte);
        }

    }
}