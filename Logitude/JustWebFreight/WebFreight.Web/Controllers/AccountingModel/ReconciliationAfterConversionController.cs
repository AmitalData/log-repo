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

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/ReconciliationAfterConversion")]
    public class ReconciliationAfterConversionController : ApiController
    {
        public ReconciliationAfterConversionController()
        {

        }

        public HttpResponseMessage GetReconciliationAfterConversion(int tenant)
        {
            string fromExtNum = "000000000000001";
            string toExtNum = "999999999999999";
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //SecurityUtility.CheckContactFeature("TaxDeductionReport", "NEW", authToken.Tenant);

                ReconciliationAfterConversionBatch reconciliationAfterConversionBatch = new ReconciliationAfterConversionBatch();
                reconciliationAfterConversionBatch.RunReconciliationAfterConversion(tenant, fromExtNum, toExtNum);
                string responseText = reconciliationAfterConversionBatch.ResponseText();
                HttpStatusCode StatusCode = reconciliationAfterConversionBatch.StatusCode();
                var res1 = new { Success = true, Message = responseText };

                return Request.CreateResponse(StatusCode, res1);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetReconciliationAfterConversionFromTo(int tenant, string fromExtNum, string toExtNum)
        {
            try
            {
                string _myfromExtNum = "";
                if (String.IsNullOrWhiteSpace(fromExtNum) || fromExtNum == "undefined")
                {
                    _myfromExtNum = "000000000000001";
                }
                else
                {
                    _myfromExtNum = Regex.Replace(fromExtNum, @"\d+", n => n.Value.PadLeft(15, '0'));
                }

                string _mytoExtNum = "";
                if (String.IsNullOrWhiteSpace(toExtNum) || toExtNum == "undefined")
                {
                    _mytoExtNum = "999999999999999";
                }
                else
                {
                    _mytoExtNum = Regex.Replace(toExtNum, @"\d+", n => n.Value.PadLeft(15, '0'));
                }

                string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //SecurityUtility.CheckContactFeature("TaxDeductionReport", "NEW", authToken.Tenant);
                bool batchIt = true;
                if (batchIt)
                {
                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatckReconciliationAfterConversionTask = new BatckReconciliationAfterConversionTask(null);
                    string subj = $"Reconciliation After Conversion({_myfromExtNum}- {_mytoExtNum})";
                    var batchTaskId = myBatckReconciliationAfterConversionTask.CreateQBatchTaskExecution<ReconciliationAfterConversionArg>(
                        new ReconciliationAfterConversionArg()
                        {
                            Tenant = tenant,
                            FromExtNum = _myfromExtNum,
                            ToExtNum = _mytoExtNum
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    ReconciliationAfterConversionBatch reconciliationAfterConversionBatch = new ReconciliationAfterConversionBatch();
                    reconciliationAfterConversionBatch.RunReconciliationAfterConversion(tenant, _myfromExtNum, _mytoExtNum);
                    string responseText = reconciliationAfterConversionBatch.ResponseText();
                    HttpStatusCode StatusCode = reconciliationAfterConversionBatch.StatusCode();
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