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

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/InterestTransactionsCheckA")]
    public class InterestTransactionsCheckAController : ApiController
    {
        public InterestTransactionsCheckAController()
        {

        }

        public HttpResponseMessage GetInterestTransactionsCheckA(int tenant, string gLAccountId, string accountTypeCode, int lT_LinesMaximum, int maxPageSize, 
            string specificJournalId, string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                InterestTransactionsCheckAArg args = null;
                string message = "";
                decimal maximalDifference = Decimal.MaxValue;
                bool isSuccess = CreateArgs(tenant, gLAccountId, accountTypeCode, lT_LinesMaximum, maxPageSize, maximalDifference, 
                    specificJournalId, lastMadeGLAccountId, maxGLAccountsPerQuery, ref args, message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }

                    InterestTransactionsCheckARun interestTransactionsCheckARun = new InterestTransactionsCheckARun();
                    interestTransactionsCheckARun.RunInterestTransactionsCheckA(args);
                    string responseText = interestTransactionsCheckARun.ResponseText();
                    HttpStatusCode StatusCode = interestTransactionsCheckARun.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private bool CreateArgs(int tenant, string gLAccountId, string accountTypeCode, int lT_LinesMaximum, int maxPageSize, decimal maximalDifference, 
            string specificJournalId, string lastMadeGLAccountId, int maxGLAccountsPerQuery, ref InterestTransactionsCheckAArg args, string message)
        {
            bool isSuccess = false;
            if (String.IsNullOrWhiteSpace(gLAccountId) && String.IsNullOrWhiteSpace(accountTypeCode)) // 2=Client, 3=Vendor
            {
                message = "AccountTypeCode is a must, when no GLAccountId is provided";
                return isSuccess;
            }

            //if (String.IsNullOrWhiteSpace(upToDueDate))
            //{
            //    message = "UpToDueDate is a must";
            //    return isSuccess;
            //}

            //DateTime upToDateValue;
            //try
            //{
            //    upToDateValue = DateTime.ParseExact(upToDueDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            //}
            //catch (FormatException ex)
            //{
            //    message = "Cannot parse UpToDueDate";
            //    return isSuccess;

            //}

            if (lT_LinesMaximum < InterestTransactionsCheckARun.LT_LinesMaximum_MIN || lT_LinesMaximum > InterestTransactionsCheckARun.LT_LinesMaximum_MAX)
            {
                message = "LT_LinesMaximum must be between " + InterestTransactionsCheckARun.LT_LinesMaximum_MIN + " and " + InterestTransactionsCheckARun.LT_LinesMaximum_MAX;
                return isSuccess;
            }

            if (maxPageSize < lT_LinesMaximum || maxPageSize > InterestTransactionsCheckARun.MaxPageSize_MAX)
            {
                message = "maxPageSize must be between " + lT_LinesMaximum + " and " + InterestTransactionsCheckARun.MaxPageSize_MAX;
                return isSuccess;
            }


            args = new InterestTransactionsCheckAArg()
            {
                Tenant = tenant,
                GLAccountId = gLAccountId,
                AccountTypeCode = accountTypeCode,
            //  UpToDueDate = upToDateValue,
                LT_LinesMaximum = lT_LinesMaximum,
                MaxPageSize = maxPageSize,
                MaximalDifference = maximalDifference,
                SpecificJournalId = specificJournalId, 
                LastMadeGLAccountId = lastMadeGLAccountId,
                MaxGLAccountsPerQuery = maxGLAccountsPerQuery,
            };
            isSuccess = true;
            return isSuccess;
        }

 
    }
}