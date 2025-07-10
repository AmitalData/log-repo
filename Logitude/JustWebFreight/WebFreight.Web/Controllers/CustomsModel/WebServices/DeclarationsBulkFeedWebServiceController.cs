using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Controllers.CustomsModel.Extended;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class DeclarationsBulkFeedWebServiceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage CheckDeclarationsInDisplayOnly([FromBody]checkDeclarationsInDisplayOnlyParamas paramas, [FromUri] ApiQueryFilters filter)
        {
            try
            {
                int tenant = GetTanent();
                QueryOperations queryOperations = CourierDeclarationPendingListExtendedController.CreateQueryOperations(filter, tenant, "Customs.DeclarationCourierStatus");
                List<string> res = new DeclarationQueryService(tenant).CheckDeclarationsInDisplayOnly(paramas.declarationIdsList, paramas.allWithoutdeclarationIdsList, paramas.checkboxAll, queryOperations, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static int GetTanent()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            return tenant;
        }
    }

    public class checkDeclarationsInDisplayOnlyParamas
    {
        public string[] declarationIdsList { get; set; }
        public string[] allWithoutdeclarationIdsList { get; set; }
        public bool checkboxAll { get; set; }
    }
}