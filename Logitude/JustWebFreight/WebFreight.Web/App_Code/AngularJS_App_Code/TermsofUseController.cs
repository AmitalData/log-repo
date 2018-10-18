using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
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
    public class TermsofUseController : ApiController
    {
        public HttpResponseMessage GetCheckIfGoToTermUseComponent(int tenant, string userId)
        {
            try
            {
                TermsofUseArgs result = new TermsofUseArgs();

                if (!string.IsNullOrEmpty(userId))
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    TermsofUseQuery termsofUseQuery = new TermsofUseQuery(tenant);
                    TermsofUsePM termofuse = termsofUseQuery.GetTermsofUseDeflut();
                    TermsofUseSignatureQuery termsofUseSignatureQuery = new TermsofUseSignatureQuery(tenant);
                    if (termofuse == null) result.IsTermOfUse = false;
                    else
                    {
                        result.Version = termofuse.Version;
                        TermsofUseSignaturePM termsofUseSignaturePM = termsofUseSignatureQuery.GetTermsofUseSignatureByContactIdAndVersion(termofuse.Version, userId , authToken.Tenant);
                        if (termsofUseSignaturePM != null)
                        {
                            result.IsTermOfUse = false;
                        }
                        else
                        {
                            if (LogitudeSettings.DeploymentStage == "logboxwe1")
                            {
                                TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
                                if (string.IsNullOrEmpty(currentTenant.PrivateLabelId))
                                {
                                    result.IsTermOfUse = false;
                                }
                                else
                                {
                                    result.IsTermOfUse = true;
                                }
                            }
                            else
                            {
                                result.IsTermOfUse = true;
                            }
                           
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


    }
}