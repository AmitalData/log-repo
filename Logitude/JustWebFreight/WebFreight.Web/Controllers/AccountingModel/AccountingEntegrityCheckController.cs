using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
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
using Logitude.Accounting.BL.EntityQueryServices;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class AccountingEntegrityCheckController : ApiController
    {

        public HttpResponseMessage PostFixEntegrityCheckErrorInBatch(AccountingIntegrityCheckPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("AccountingIntegrityCheck", entityPM.Tenant, authToken.Tenant);
                int tenant = authToken.Tenant;
                AccountingIntegrityService accountingIntegrityService = new AccountingIntegrityService();
                BatchTaskExecutionPM btePM = accountingIntegrityService.FixEntegrityCheckErrorInBatch(entityPM.Id, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, btePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetAccountingIntegrityResultByIdAndTenant(string id, int tenant)
        {
            try
            {
                AccountingIntegrityCheckQueryService accountingIntegrityCheckQuery = new AccountingIntegrityCheckQueryService(tenant);
                AccountingIntegrityResult accountingIntegrityResult = accountingIntegrityCheckQuery.GetAccountingIntegrityResultByIdAndTenant(id,tenant);

                return Request.CreateResponse(HttpStatusCode.OK, accountingIntegrityResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}