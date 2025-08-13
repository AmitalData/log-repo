using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.Controllers.InfrastructureModel.Services;
using Logitude.Customs.Data;
using Simplog.Data.InfrastructureModel.Repositories;


namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class ObjectTableExtendedController : ApiController
    {
        public HttpResponseMessage GetObjectTableByName(string objectTableName, int contextTenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                if(contextTenant == 0 && tenant > 0)
                {
                    contextTenant = tenant;
                }
                ICustomContext customContext = CustomContext.GetContext(contextTenant);
                var objectTableRepository = new ObjectTableRepository(contextTenant);
                var objectTablePM = objectTableRepository.GetObjectTableByName(objectTableName, 0, true, contextTenant);
                return Request.CreateResponse(HttpStatusCode.OK, objectTablePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}