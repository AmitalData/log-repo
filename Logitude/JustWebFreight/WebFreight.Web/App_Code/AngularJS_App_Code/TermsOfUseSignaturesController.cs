using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class TermsOfUseSignaturesController : ApiController
    {
        public HttpResponseMessage Post(TermsofUseSignaturePM entityPM)
        {
            try
            {
               
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    TermsofUseSignatureService service = new TermsofUseSignatureService(MyContext, entityPM.Tenant);
                    service.Create(entityPM);

                  
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
               
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTermsofUseSignatures(int tenant, string contactId)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                TermsofUseSignatureQuery termsofUseSignatureQuery = new TermsofUseSignatureQuery(tenant);

                List<TermsofUseSignaturePM> TermsofUseSignaturePMLists = termsofUseSignatureQuery.GetTermsofUseSignaturesByTenant(tenant, contactId).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, TermsofUseSignaturePMLists);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}