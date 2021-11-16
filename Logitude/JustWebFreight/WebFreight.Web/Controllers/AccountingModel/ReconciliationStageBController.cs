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

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/ReconciliationStageB")]
    public class ReconciliationStageBController : ApiController
    {
        public ReconciliationStageBController()
        {

        }

        public HttpResponseMessage GetReconciliationStageB(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                bool batchIt = true;
                if (batchIt)
                {
                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchReconciliationStageBTask = new BatchReconciliationStageBTask(null);
                    string subj = $"Reconciliation Stage B";
                    var batchTaskId = myBatchReconciliationStageBTask.CreateQBatchTaskExecution<ReconciliationStageBArg>(
                        new ReconciliationStageBArg()
                        {
                            Tenant = tenant,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    ReconciliationStageBBatch reconciliationStageBBatch = new ReconciliationStageBBatch();
                    ReconciliationStageBArg reconciliationStageBArg = new ReconciliationStageBArg()
                    {
                        Tenant = tenant,
                    };
                    reconciliationStageBBatch.RunReconciliationStageB(reconciliationStageBArg);
                    string responseText = reconciliationStageBBatch.ResponseText();
                    HttpStatusCode StatusCode = reconciliationStageBBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetReconciliationStageBNoBatch(int tenant, int noBatch)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                bool batchIt = true;
                if (noBatch == 1) batchIt = false;
                if (batchIt)
                {
                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchReconciliationStageBTask = new BatchReconciliationStageBTask(null);
                    string subj = $"Reconciliation Stage B";
                    var batchTaskId = myBatchReconciliationStageBTask.CreateQBatchTaskExecution<ReconciliationStageBArg>(
                        new ReconciliationStageBArg()
                        {
                            Tenant = tenant,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    ReconciliationStageBBatch reconciliationStageBBatch = new ReconciliationStageBBatch();
                    ReconciliationStageBArg reconciliationStageBArg = new ReconciliationStageBArg()
                    {
                        Tenant = tenant,
                    };
                    reconciliationStageBBatch.RunReconciliationStageB(reconciliationStageBArg);
                    string responseText = reconciliationStageBBatch.ResponseText();
                    HttpStatusCode StatusCode = reconciliationStageBBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetReconciliationStageBNoBatchGLAcc(int tenant, string gLAccountId, int noBatch)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                bool batchIt = true;
                if (noBatch == 1) batchIt = false;
                if (batchIt)
                {
                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchReconciliationStageBTask = new BatchReconciliationStageBTask(null);
                    string subj = $"Reconciliation Stage B";
                    var batchTaskId = myBatchReconciliationStageBTask.CreateQBatchTaskExecution<ReconciliationStageBArg>(
                        new ReconciliationStageBArg()
                        {
                            Tenant = tenant,
                            GLAccountId = gLAccountId,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    ReconciliationStageBBatch reconciliationStageBBatch = new ReconciliationStageBBatch();
                    ReconciliationStageBArg reconciliationStageBArg = new ReconciliationStageBArg()
                    {
                        Tenant = tenant,
                        GLAccountId = gLAccountId,
                    };
                    reconciliationStageBBatch.RunReconciliationStageB(reconciliationStageBArg);
                    string responseText = reconciliationStageBBatch.ResponseText();
                    HttpStatusCode StatusCode = reconciliationStageBBatch.StatusCode();
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