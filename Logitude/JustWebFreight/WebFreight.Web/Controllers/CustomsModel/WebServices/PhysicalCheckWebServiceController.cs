using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class PhysicalCheckWebServiceController : ApiController
    {
        public HttpResponseMessage GetPhysicalCheckByDeclarationIdLists(string declarationId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);
                PhysicalCheckQueryService queryService = new PhysicalCheckQueryService(customContext);
                List<PhysicalCheckList> physicalCheckList = queryService.GetPhysicalChecksByDeclarationId(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, physicalCheckList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}