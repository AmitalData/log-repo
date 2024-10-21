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
    //[RoutePrefix("api/GLAccountInterestActivationBalance")]
    public class GLAccountInterestActivationBalanceController : ApiController
    {
        public GLAccountInterestActivationBalanceController()
        {

        }

        public HttpResponseMessage GetGLAccountInterestActivationBalance(int tenant, string gLAccountId, string accountTypeCode, string interestActivationDate, int batchIt,
            string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                GLAccountInterestActivationBalanceArgs args = null;
                string message = "";
                bool isSuccess = CreateArgs(tenant, gLAccountId, accountTypeCode, interestActivationDate, batchIt,
                    lastMadeGLAccountId, maxGLAccountsPerQuery, ref args, ref message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }
                bool toBatchIt = (args.BatchIt == 1);
                if (toBatchIt)
                {
                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myGLAccountInterestActivationBalanceTask = new BatchGLAccountInterestActivationBalanceTask(null);
                    string subj = $"GLAccount Interest Activation Balance";
                    var batchTaskId = myGLAccountInterestActivationBalanceTask.CreateQBatchTaskExecution<GLAccountInterestActivationBalanceArgs>(
                        new GLAccountInterestActivationBalanceArgs()
                        {
                            Tenant = tenant,
                            GLAccountId = args.GLAccountId,
                            AccountTypeCode = args.AccountTypeCode,
                            InterestActivationDate = args.InterestActivationDate,
                            LastMadeGLAccountId = args.LastMadeGLAccountId, 
                            MaxGLAccountsPerQuery = args.MaxGLAccountsPerQuery,
                            ActionDate = args.ActionDate,
                            BatchIt = 1, 
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    GLAccountInterestActivationBalanceBatch gLAccountInterestActivationBalanceBatch = new GLAccountInterestActivationBalanceBatch();
                    gLAccountInterestActivationBalanceBatch.RunGLAccountInterestActivationBalance(args);
                    string responseText = gLAccountInterestActivationBalanceBatch.ResponseText();
                    HttpStatusCode StatusCode = gLAccountInterestActivationBalanceBatch.StatusCode();
                    var res1 = new { Success = (StatusCode == HttpStatusCode.Accepted), Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private bool CreateArgs(int tenant, string gLAccountId, string accountTypeCode, string interestActivationDate, int batchIt,
            string lastMadeGLAccountId, int maxGLAccountsPerQuery, ref GLAccountInterestActivationBalanceArgs args, ref string message)
        {
            bool isSuccess = false;
            if (String.IsNullOrWhiteSpace(gLAccountId) && String.IsNullOrWhiteSpace(accountTypeCode)) // 2=Client, 3=Vendor
            {
                message = "AccountTypeCode is a must, when no GLAccountId is provided";
                return isSuccess;
            }

            if (String.IsNullOrWhiteSpace(interestActivationDate) || interestActivationDate == "DD.MM.YYYY")
            {
                message = "InterestActivationDate is a must";
                return isSuccess;
            }

            DateTime interestActivationDate_DT;
            try
            {
                interestActivationDate_DT = DateTime.ParseExact(interestActivationDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                message = "Cannot parse InterestActivationDate";
                return isSuccess;

            }



            args = new GLAccountInterestActivationBalanceArgs()
            {
                Tenant = tenant,
                GLAccountId = gLAccountId,
                AccountTypeCode = accountTypeCode,
                InterestActivationDate = interestActivationDate_DT,
                BatchIt = batchIt,
                LastMadeGLAccountId = lastMadeGLAccountId,
                MaxGLAccountsPerQuery = maxGLAccountsPerQuery,
                ActionDate = DateTime.Today,
            };
            isSuccess = true;
            return isSuccess;
        }


    }
}