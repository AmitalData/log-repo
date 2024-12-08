using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using System.Transactions;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Accounting.BL.Utils;

namespace WebFreight.Web.Controllers.AccountingModel //AccountingPeriodViewsController.cs
{
    //[RoutePrefix("api/GLAccountOp")]
    public partial class GLAccountOpController : ApiController
    {
        public GLAccountOpController()
        {

        }
        public HttpResponseMessage GetCalculateFutureChequesForTenant(int tenant)
        {
            try
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                    FutureOpenChequesBatch FutureOpenChequesBatch = new FutureOpenChequesBatch();
                    FutureOpenChequesBatch.SetTotalFutureOpenChequesInLocalCurrency(tenant);

                    return Request.CreateResponse(HttpStatusCode.OK, "OK");
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




    }


}