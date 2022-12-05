using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{

    public class InterfaceTenantDefinitionWebServiceController : ApiController
    {
        public HttpResponseMessage GetSingleByTenantCode(int tenant, string code)
        {
            try
            {
               

                ICustomContext MyContext = CustomContext.GetContext(tenant);
                InterfaceTenantDefinitionQueryService interfaceTenantDefinitionQuery = new InterfaceTenantDefinitionQueryService(MyContext);
                InterfaceTenantDefinitionPM interfaceTenantDefinitionPM = interfaceTenantDefinitionQuery.GetFromCacheByTenatCode(tenant, code);


                return Request.CreateResponse(HttpStatusCode.OK, interfaceTenantDefinitionPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}