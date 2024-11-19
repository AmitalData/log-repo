using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Logitude.Accounting.BL.Utils;
using System.Net;
using WebFreight.Web.Helpers;
using System.Text.RegularExpressions;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.CoreBL.Batch;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;
using System.Globalization;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/GLAccountRecalculate")]
    public class GLAccountRecalculateController : ApiController
    {
        public GLAccountRecalculateController()
        {

        }


        public HttpResponseMessage GetGLAccountRecalculate(int tenant, string accountId, string batch = "1")
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
                GLAccountRecalculateArg args = null;
                string message = "";
                bool isSuccess = CreateArgs(tenant, accountId, loggedContact.Id, batch, ref args, message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }

                bool batchIt = args.Batch;
                if (batchIt)
                {

                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchGLAccountRecalculateTask = new BatchGLAccountRecalculateTask(null);
                    string subj = $"GLAccount Recalculation";
                    var batchTaskId = myBatchGLAccountRecalculateTask.CreateQBatchTaskExecution<GLAccountRecalculateArg>(
                        new GLAccountRecalculateArg()
                        {
                            Tenant = tenant,
                            AccountId = args.AccountId,
                            UserId = args.UserId,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    GLAccountRecalculateBatch gLAccountRecalculateBatch = new GLAccountRecalculateBatch();
                    GLAccountRecalculateArg gLAccountRecalculateArg = new GLAccountRecalculateArg()
                    {
                        Tenant = tenant,
                        AccountId = args.AccountId,
                        UserId = args.UserId,
                    };
                    gLAccountRecalculateBatch.RunGLAccountRecalculate(gLAccountRecalculateArg);
                    string responseText = gLAccountRecalculateBatch.ResponseText();
                    HttpStatusCode StatusCode = gLAccountRecalculateBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private bool CreateArgs(int tenant, string accountId, string userId, string batch, ref GLAccountRecalculateArg args, string message)
        {
            bool isSuccess = false;
            bool v_batch = false;
            if (String.IsNullOrWhiteSpace(accountId))
            {
                message = "AccountId is a must";
                return isSuccess;
            }

            if (String.IsNullOrWhiteSpace(userId))
            {
                message = "UserId is a must";
                return isSuccess;
            }

            IAccountingContext accContext = AccountingContext.GetContext(tenant);
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(accContext);
            var acc = gLAccountQueryService.GetSingleByAccountId(accountId, tenant);
            if (acc == null)
            {
                message = "Account {accountId} not found";
                return isSuccess;
            }


            if (!String.IsNullOrEmpty(batch) && (batch == "1" || batch.ToUpperInvariant() == "TRUE"))
            {
                v_batch = true;
            }

            args = new GLAccountRecalculateArg()
            {
                Tenant = tenant,
                AccountId = accountId,
                Batch = v_batch,
                UserId = userId,
            };
            isSuccess = true;
            return isSuccess;
        }


    }
}