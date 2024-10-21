using System;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Logitude.Accounting.BL.Utils;
using System.Net;
using WebFreight.Web.Helpers;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.CoreBL.Batch;
using System.Globalization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    //[RoutePrefix("api/GLAccountInterestDeactivationBalance")]
    public class GLAccountInterestDeactivationBalanceController : ApiController
    {
        public GLAccountInterestDeactivationBalanceController()
        {

        }

        public HttpResponseMessage GetGLAccountInterestDeactivationBalance(int tenant, string gLAccountId, int batchIt)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                GLAccountInterestDeactivationBalanceArgs args = null;
                string message = "";
                bool isSuccess = CreateArgs(tenant, gLAccountId, batchIt, ref args, ref message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }
                bool toBatchIt = (args.BatchIt == 1);
                if (toBatchIt)
                {
                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myGLAccountInterestDeactivationBalanceTask = new BatchGLAccountInterestDeactivationBalanceTask(null);
                    string subj = $"GLAccount Interest Deactivation Balance";
                    var batchTaskId = myGLAccountInterestDeactivationBalanceTask.CreateQBatchTaskExecution<GLAccountInterestDeactivationBalanceArgs>(
                        new GLAccountInterestDeactivationBalanceArgs()
                        {
                            Tenant = tenant,
                            GLAccountId = args.GLAccountId,
                            ActionDate = args.ActionDate,
                            BatchIt = 1,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    GLAccountInterestDeactivationBalanceBatch gLAccountInterestDeactivationBalanceBatch = new GLAccountInterestDeactivationBalanceBatch();
                    gLAccountInterestDeactivationBalanceBatch.RunGLAccountInterestDeactivationBalance(args);
                    string responseText = gLAccountInterestDeactivationBalanceBatch.ResponseText();
                    HttpStatusCode StatusCode = gLAccountInterestDeactivationBalanceBatch.StatusCode();
                    var res1 = new { Success = (StatusCode == HttpStatusCode.Accepted), Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private bool CreateArgs(int tenant, string gLAccountId, int batchIt, ref GLAccountInterestDeactivationBalanceArgs args, ref string message)
        {
            bool isSuccess = false;
            if (String.IsNullOrWhiteSpace(gLAccountId))
            {
                message = "GLAccountId is a must";
                return isSuccess;
            }




            args = new GLAccountInterestDeactivationBalanceArgs()
            {
                Tenant = tenant,
                GLAccountId = gLAccountId,
                BatchIt = batchIt,
                ActionDate = DateTime.Today,
            };
            isSuccess = true;
            return isSuccess;
        }


    }
}