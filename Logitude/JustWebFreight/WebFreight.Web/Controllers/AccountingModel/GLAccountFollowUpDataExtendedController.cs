using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
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

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class GLAccountFollowUpDataExtendedController: ApiController
    {
        public HttpResponseMessage GetSingleByAccountId(string accountid)
        {
            try
            {
             
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("GLAccountFollowUpData", "READ", authToken.Tenant);
                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountFollowUpDataQueryService gLAccountFollowUpDataQuery = new GLAccountFollowUpDataQueryService(MyContext);
                GLAccountFollowUpDataPM gLAccountFollowUpData = gLAccountFollowUpDataQuery.GetSinglePMByAccountId(accountid, authToken.Tenant);            
                return Request.CreateResponse(HttpStatusCode.OK, gLAccountFollowUpData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}