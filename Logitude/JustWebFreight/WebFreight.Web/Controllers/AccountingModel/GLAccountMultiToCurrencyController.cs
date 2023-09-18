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

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/GLAccountMultiToCurrency")]
    public class GLAccountMultiToCurrencyController : ApiController
    {
        public GLAccountMultiToCurrencyController()
        {

        }


        public HttpResponseMessage GetGLAccountMultiToCurrency(int tenant, string accountId, string toAccountingDate, string batch)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                GLAccountMultiToCurrencyArg args = null;
                string message = "";
                bool isSuccess = CreateArgs(tenant, accountId, toAccountingDate, batch, ref args, message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }

                bool batchIt = args.Batch;
                if (batchIt)
                {

                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchGLAccountMultiToCurrencyTask = new BatchGLAccountMultiToCurrencyTask(null);
                    string subj = $"Convert GLAccount from Multi to Currency";
                    var batchTaskId = myBatchGLAccountMultiToCurrencyTask.CreateQBatchTaskExecution<GLAccountMultiToCurrencyArg>(
                        new GLAccountMultiToCurrencyArg()
                        {
                            Tenant = tenant,
                            AccountId = args.AccountId,
                            ToCurrencyId = args.ToCurrencyId,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    GLAccountMultiToCurrencyBatch gLAccountMultiToCurrencyBatch = new GLAccountMultiToCurrencyBatch();
                    GLAccountMultiToCurrencyArg gLAccountMultiToCurrencyArg = new GLAccountMultiToCurrencyArg()
                    {
                        Tenant = tenant,
                        AccountId = args.AccountId,
                        ToCurrencyId = args.ToCurrencyId,
                    };
                    gLAccountMultiToCurrencyBatch.RunGLAccountMultiToCurrency(gLAccountMultiToCurrencyArg);
                    string responseText = gLAccountMultiToCurrencyBatch.ResponseText();
                    HttpStatusCode StatusCode = gLAccountMultiToCurrencyBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private bool CreateArgs(int tenant, string accountId, string toCurrencyId, string batch, ref GLAccountMultiToCurrencyArg args, string message)
        {
            bool isSuccess = false;
            bool v_batch = false;
            if (String.IsNullOrWhiteSpace(accountId))
            {
                message = "AccountId is a must";
                return isSuccess;
            }


            if (String.IsNullOrWhiteSpace(toCurrencyId))
            {
                message = "CurrencyId is a must";
                return isSuccess;
            }



            if (!String.IsNullOrEmpty(batch) && (batch == "1" || batch.ToUpperInvariant() == "TRUE"))
            {
                v_batch = true;
            }

            args = new GLAccountMultiToCurrencyArg()
            {
                Tenant = tenant,
                AccountId = accountId,
                ToCurrencyId = toCurrencyId,
                Batch = v_batch,
            };
            isSuccess = true;
            return isSuccess;
        }


    }
}