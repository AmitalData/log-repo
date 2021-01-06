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

        public HttpResponseMessage GetReconciliationStageC(int tenant, string gLAccountId, string accountTypeCode, string upToDueDate, int lT_LinesMaximum, int maxPageSize, string closeOnlyZeroes)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                ReconciliationStageCArg args = null;
                string message = "";
                decimal maximalDifference = Decimal.MaxValue;
                bool isSuccess = CreateArgs(tenant, gLAccountId, accountTypeCode, upToDueDate, lT_LinesMaximum, maxPageSize, maximalDifference, closeOnlyZeroes, ref args, message);
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

        private bool CreateArgs(int tenant, string gLAccountId, string accountTypeCode, string upToDueDate, int lT_LinesMaximum, int maxPageSize, decimal maximalDifference, string closeOnlyZeroes, ref ReconciliationStageCArg args, string message)
        {
            bool isSuccess = false;
            bool v_closeOnlyZeroes = false;
            if (String.IsNullOrWhiteSpace(gLAccountId) && (String.IsNullOrWhiteSpace(accountTypeCode) || (accountTypeCode != "2" && accountTypeCode != "3"))) // 2=Client, 3=Vendor
            {
                message = "AccountTypeCode is a must (2=Client, 3=Vendor), when no GLAccountId is provided";
                return isSuccess;
            }

            if (String.IsNullOrWhiteSpace(upToDueDate)) 
            {
                message = "UpToDueDate is a must";
                return isSuccess;
            }

            DateTime upToDateValue;
            try
            {
                upToDateValue = DateTime.ParseExact(upToDueDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                message = "Cannot parse UpToDueDate";
                return isSuccess;

            }

            if (lT_LinesMaximum < ReconciliationStageCBatch.LT_LinesMaximum_MIN || lT_LinesMaximum > ReconciliationStageCBatch.LT_LinesMaximum_MAX)
            {
                message = "LT_LinesMaximum must be between " + ReconciliationStageCBatch.LT_LinesMaximum_MIN + " and " + ReconciliationStageCBatch.LT_LinesMaximum_MAX;
                return isSuccess;
            }

            if (maxPageSize < lT_LinesMaximum || maxPageSize > ReconciliationStageCBatch.MaxPageSize_MAX)
            {
                message = "maxPageSize must be between " + lT_LinesMaximum + " and " + ReconciliationStageCBatch.MaxPageSize_MAX;
                return isSuccess;
            }

            if (!String.IsNullOrEmpty(closeOnlyZeroes) && closeOnlyZeroes.ToUpperInvariant() == "TRUE")
            {
                v_closeOnlyZeroes = true;
            }

            args = new ReconciliationStageCArg()
            {
                Tenant = tenant,
                GLAccountId = gLAccountId,
                AccountTypeCode = accountTypeCode,
                UpToDueDate = upToDateValue,
                LT_LinesMaximum = lT_LinesMaximum,
                MaxPageSize = maxPageSize,
                MaximalDifference = maximalDifference,
                CloseOnlyZeroes = v_closeOnlyZeroes,
            };
            isSuccess = true;
            return isSuccess;
        }

        public HttpResponseMessage GetReconciliationStageCNoBatch(int tenant, string gLAccountId, string accountTypeCode, string upToDueDate, int lT_LinesMaximum, int maxPageSize, string closeOnlyZeroes, int noBatch)  
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                ReconciliationStageCArg args = null;
                string message = "";
                decimal maximalDifference = Decimal.MaxValue;
                bool isSuccess = CreateArgs(tenant, gLAccountId, accountTypeCode, upToDueDate, lT_LinesMaximum, maxPageSize, maximalDifference, closeOnlyZeroes, ref args, message);
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