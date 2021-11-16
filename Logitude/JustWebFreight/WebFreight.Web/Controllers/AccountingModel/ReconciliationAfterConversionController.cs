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
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;

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
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("TaxDeductionReport", "NEW", authToken.Tenant);

                ReconciliationAfterConversionBatch reconciliationAfterConversionBatch = new ReconciliationAfterConversionBatch();
                ReconciliationAfterConversionArg reconciliationAfterConversionArg = new ReconciliationAfterConversionArg()
                {
                    Tenant = tenant,
                    FromExtNum = fromExtNum,
                    ToExtNum = toExtNum,
                };
                reconciliationAfterConversionBatch.RunReconciliationAfterConversion(reconciliationAfterConversionArg);
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


        public HttpResponseMessage GetReconciliationAfterConversion(int tenant, int NoBatch)
        {
            string fromExtNum = "000000000000001";
            string toExtNum = "999999999999999";
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("TaxDeductionReport", "NEW", authToken.Tenant);

                ReconciliationAfterConversionBatch reconciliationAfterConversionBatch = new ReconciliationAfterConversionBatch();
                ReconciliationAfterConversionArg reconciliationAfterConversionArg = new ReconciliationAfterConversionArg()
                {
                    Tenant = tenant,
                    FromExtNum = fromExtNum,
                    ToExtNum = toExtNum,
                };
                reconciliationAfterConversionBatch.RunReconciliationAfterConversion(reconciliationAfterConversionArg);
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
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("TaxDeductionReport", "NEW", authToken.Tenant);

                string _myfromExtNum = "";
                if (String.IsNullOrWhiteSpace(fromExtNum) || fromExtNum == "undefined")
                {
                    fromExtNum = "1";
                    _myfromExtNum = "000000000000001";
                }
                else
                {
                    _myfromExtNum = Regex.Replace(fromExtNum, @"\d+", n => n.Value.PadLeft(15, '0'));
                }

                string _mytoExtNum = "";
                if (String.IsNullOrWhiteSpace(toExtNum) || toExtNum == "undefined")
                {
                    toExtNum = "999999999999999";
                    _mytoExtNum = "999999999999999";
                }
                else
                {
                    _mytoExtNum = Regex.Replace(toExtNum, @"\d+", n => n.Value.PadLeft(15, '0'));
                }

                
                bool batchIt = true;
                if (batchIt)
                {
                    int SUB_BATCH_SIZE = 50;
                    int MAX_PARALLELS = 3;
                    long fromExt_long = Convert.ToInt64(fromExtNum);
                    long toExt_long = Convert.ToInt64(toExtNum);
                    long big_batch_size = 1;
                    long all_in_total = toExt_long - fromExt_long + 1;
                    if (all_in_total > SUB_BATCH_SIZE)
                    {
                        big_batch_size = all_in_total / MAX_PARALLELS + 1;
                    }
                    int count = 0;
                    string firstSubj = ""; 
                    string lastSubj = "";


                    for (long lower = fromExt_long; ;)
                    {

                        long upper = lower + big_batch_size - 1;
                        if (upper > toExt_long)
                        {
                            upper = toExt_long;
                        }
                        string lower_str = "";
                        if (lower <= 0)
                        {
                            lower_str = "000000000000001";
                        }
                        else
                        {
                            lower_str = Regex.Replace(lower.ToString(), @"\d+", n => n.Value.PadLeft(15, '0'));
                        }

                        string upper_str = "";
                        upper_str = Regex.Replace(upper.ToString(), @"\d+", n => n.Value.PadLeft(15, '0'));

                        var accountingContext = AccountingContext.GetContext(tenant);

                        var myBatchReconciliationAfterConversionTask = new BatchReconciliationAfterConversionTask(null);
                        string subj = $"Reconciliation After Conversion({lower_str}- {upper_str})";
                        if (String.IsNullOrEmpty(firstSubj))
                            firstSubj = subj;
                        lastSubj = $" to ({lower_str}- {upper_str})";
                        var batchTaskId = myBatchReconciliationAfterConversionTask.CreateQBatchTaskExecution<ReconciliationAfterConversionArg>(
                            new ReconciliationAfterConversionArg()
                            {
                                Tenant = tenant,
                                FromExtNum = lower_str,
                                ToExtNum = upper_str,
                            }, tenant, subj, false);
                        count++;


                        if (lower + big_batch_size - 1 >= toExt_long)
                        {
                            break;
                        }
                        else
                        {
                            lower += big_batch_size;
                            if (lower > toExt_long)
                            {
                                lower = toExt_long;
                            }
                        }
                    }



                    string message = $"Send to {count} Batch Tasks {firstSubj}";
                    if (count > 1)
                        message = message + lastSubj;


                    var res1 = new { Success = true, Message = message };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    ReconciliationAfterConversionBatch reconciliationAfterConversionBatch = new ReconciliationAfterConversionBatch();
                    ReconciliationAfterConversionArg reconciliationAfterConversionArg = new ReconciliationAfterConversionArg()
                    {
                        Tenant = tenant,
                        FromExtNum = _myfromExtNum,
                        ToExtNum = _mytoExtNum,
                    };
                    reconciliationAfterConversionBatch.RunReconciliationAfterConversion(reconciliationAfterConversionArg);
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





        public HttpResponseMessage GetReconciliationAfterConversionFromToNoBatch(int tenant, string fromExtNum, string toExtNum, int noBatch)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("TaxDeductionReport", "NEW", authToken.Tenant);

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


                bool batchIt = true;
                if (noBatch == 1) batchIt = false;
                if (batchIt)
                {

                    int SUB_BATCH_SIZE = 50;
                    int MAX_PARALLELS = 3;

                    long fromExt_long = Convert.ToInt64(fromExtNum);
                    long toExt_long = Convert.ToInt64(toExtNum);
                    long big_batch_size = 1;
                    long all_in_total = toExt_long - fromExt_long + 1;
                    if (all_in_total > SUB_BATCH_SIZE)
                    {
                        big_batch_size = all_in_total / MAX_PARALLELS + 1;
                    }


                    int count = 0;
                    string firstSubj = "";
                    string lastSubj = "";


                    for (long lower = fromExt_long; ;)
                    {

                        long upper = lower + big_batch_size - 1;
                        if (upper > toExt_long)
                        {
                            upper = toExt_long;
                        }
                        string lower_str = "";
                        if (lower <= 0)
                        {
                            lower_str = "000000000000001";
                        }
                        else
                        {
                            lower_str = Regex.Replace(lower.ToString(), @"\d+", n => n.Value.PadLeft(15, '0'));
                        }

                        string upper_str = "";
                        upper_str = Regex.Replace(upper.ToString(), @"\d+", n => n.Value.PadLeft(15, '0'));

                        var accountingContext = AccountingContext.GetContext(tenant);

                        var myBatchReconciliationAfterConversionTask = new BatchReconciliationAfterConversionTask(null);
                        string subj = $"Reconciliation After Conversion({lower_str}- {upper_str})";
                        if (String.IsNullOrEmpty(firstSubj))
                            firstSubj = subj;
                        lastSubj = $" to ({lower_str}- {upper_str})";
                        var batchTaskId = myBatchReconciliationAfterConversionTask.CreateQBatchTaskExecution<ReconciliationAfterConversionArg>(
                            new ReconciliationAfterConversionArg()
                            {
                                Tenant = tenant,
                                FromExtNum = lower_str,
                                ToExtNum = upper_str,
                            }, tenant, subj, false);
                        count++;


                        if (lower + big_batch_size - 1 >= toExt_long)
                        {
                            break;
                        }
                        else
                        {
                            lower += big_batch_size;
                            if (lower > toExt_long)
                            {
                                lower = toExt_long;
                            }
                        }
                    }



                    string message = $"Send to {count} Batch Tasks {firstSubj}";
                    if (count > 1)
                        message = message + lastSubj;


                    var res1 = new { Success = true, Message = message };


                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    ReconciliationAfterConversionBatch reconciliationAfterConversionBatch = new ReconciliationAfterConversionBatch();
                    ReconciliationAfterConversionArg reconciliationAfterConversionArg = new ReconciliationAfterConversionArg()
                    {
                        Tenant = tenant,
                        FromExtNum = _myfromExtNum,
                        ToExtNum = _mytoExtNum,
                    };
                    reconciliationAfterConversionBatch.RunReconciliationAfterConversion(reconciliationAfterConversionArg);
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