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
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class TextCodesController : ApiController
    {
        public HttpResponseMessage GetSingle(string id,int tenant)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("MoveType", "READ", authToken.Tenant);
                    GeneralDomainService service = new GeneralDomainService();
                    var SingletextCode = service.GetSingleFieldTranslationForTextCodeId(id,tenant);

                    return Request.CreateResponse(HttpStatusCode.OK, SingletextCode);

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }

            }

        public HttpResponseMessage GetSingleByCode(string code, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("MoveType", "READ", authToken.Tenant);
                GeneralDomainService service = new GeneralDomainService();
                string singleTextCodeId = service.GetTextCodeIdByCode(code, tenant);
                var singleTextCode = service.GetSingleFieldTranslationForTextCodeId(singleTextCodeId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, singleTextCode);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}