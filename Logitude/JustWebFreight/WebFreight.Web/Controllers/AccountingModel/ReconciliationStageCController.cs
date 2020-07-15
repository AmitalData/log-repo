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
using System.Globalization;

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/ReconciliationStageC")]
    public class ReconciliationStageCController : ApiController
    {
        public ReconciliationStageCController()
        {

        }

        public HttpResponseMessage GetReconciliationStageC(int tenant, string gLAccountId, string accountTypeCode, string upToAccountingDate, int lT_LinesMaximum) //, decimal maximalDifference)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                ReconciliationStageCArg args = null;
                string message = "";
                decimal maximalDifference = Decimal.MaxValue;
                bool isSuccess = CreateArgs(tenant, gLAccountId, accountTypeCode, upToAccountingDate, lT_LinesMaximum, maximalDifference, ref args, message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }

                bool batchIt = true;
                if (batchIt)
                {
                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchReconciliationStageCTask = new BatchReconciliationStageCTask(null);
                    string subj = $"Reconciliation Stage C";
                    var batchTaskId = myBatchReconciliationStageCTask.CreateQBatchTaskExecution<ReconciliationStageCArg>(args, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    ReconciliationStageCBatch reconciliationStageCBatch = new ReconciliationStageCBatch();
                    reconciliationStageCBatch.RunReconciliationStageC(args);
                    string responseText = reconciliationStageCBatch.ResponseText();
                    HttpStatusCode StatusCode = reconciliationStageCBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private bool CreateArgs(int tenant, string gLAccountId, string accountTypeCode, string upToAccountingDate, int lT_LinesMaximum, decimal maximalDifference, ref ReconciliationStageCArg args, string message)
        {
            bool isSuccess = false;
            if (String.IsNullOrWhiteSpace(gLAccountId) && (String.IsNullOrWhiteSpace(accountTypeCode) || (accountTypeCode != "2" && accountTypeCode != "3"))) // 2=Client, 3=Vendor
            {
                message = "AccountTypeCode is a must (2=Client, 3=Vendor), when no GLAccountId is provided";
                return isSuccess;
            }

            if (String.IsNullOrWhiteSpace(upToAccountingDate)) 
            {
                message = "UpToAccountingDate is a must";
                return isSuccess;
            }

            DateTime upToDateValue;
            try
            {
                upToDateValue = DateTime.ParseExact(upToAccountingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                message = "Cannot parse UpToAccountingDate";
                return isSuccess;

            }

            if (lT_LinesMaximum < ReconciliationStageCBatch.LT_LinesMaximum_MIN || lT_LinesMaximum > ReconciliationStageCBatch.LT_LinesMaximum_MAX)
            {
                message = "LT_LinesMaximum must be between " + ReconciliationStageCBatch.LT_LinesMaximum_MIN + " and " + ReconciliationStageCBatch.LT_LinesMaximum_MAX;
                return isSuccess;
            }

            if (maximalDifference < 0.00m)
            {
                message = "MaximalDifference must not be less than zero";
                return isSuccess;
            }

            args = new ReconciliationStageCArg()
            {
                Tenant = tenant,
                GLAccountId = gLAccountId,
                AccountTypeCode = accountTypeCode,
                UpToAccountingDate = upToDateValue,
                LT_LinesMaximum = lT_LinesMaximum,
                MaximalDifference = maximalDifference,
            };
            isSuccess = true;
            return isSuccess;
        }


       // public HttpResponseMessage GetReconciliationStageCNoBatch(int tenant, string gLAccountId, string accountTypeCode, string upToAccountingDate, int lT_LinesMaximum, decimal maximalDifference, int noBatch)
        public HttpResponseMessage GetReconciliationStageCNoBatch(int tenant, string gLAccountId, string accountTypeCode, string upToAccountingDate, int lT_LinesMaximum, int noBatch) // penultimate parameter was decimal maximalDifference 
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                ReconciliationStageCArg args = null;
                string message = "";
                decimal maximalDifference = Decimal.MaxValue;
                bool isSuccess = CreateArgs(tenant, gLAccountId, accountTypeCode, upToAccountingDate, lT_LinesMaximum, maximalDifference, ref args, message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }

                bool batchIt = true;
                if (noBatch == 1) batchIt = false;
                if (batchIt)
                {
                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchReconciliationStageCTask = new BatchReconciliationStageCTask(null);
                    string subj = $"Reconciliation Stage C";
                    var batchTaskId = myBatchReconciliationStageCTask.CreateQBatchTaskExecution<ReconciliationStageCArg>(
                        args, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    ReconciliationStageCBatch reconciliationStageCBatch = new ReconciliationStageCBatch();
                    reconciliationStageCBatch.RunReconciliationStageC(args);
                    string responseText = reconciliationStageCBatch.ResponseText();
                    HttpStatusCode StatusCode = reconciliationStageCBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}