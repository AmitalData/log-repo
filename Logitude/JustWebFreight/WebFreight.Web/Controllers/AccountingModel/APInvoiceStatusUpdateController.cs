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
    //[RoutePrefix("api/APInvoiceStatusUpdate")]
    public class APInvoiceStatusUpdateController : ApiController
    {
        public APInvoiceStatusUpdateController()
        {

        }


        public HttpResponseMessage GetAPInvoiceStatusUpdate(int tenant, string invoiceNumber, string fromInvoiceDate, string toInvoiceDate, string batch)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                APInvoiceStatusUpdateArg args = null;
                string message = "";
                bool isSuccess = CreateArgs(tenant, invoiceNumber, fromInvoiceDate, toInvoiceDate, batch, ref args, message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }

                bool batchIt = args.Batch;
                if (batchIt)
                {

                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchAPInvoiceStatusUpdateTask = new BatchAPInvoiceStatusUpdateTask(null);
                    string subj = $"A/P Invoice Status Update";
                    var batchTaskId = myBatchAPInvoiceStatusUpdateTask.CreateQBatchTaskExecution<APInvoiceStatusUpdateArg>(
                        new APInvoiceStatusUpdateArg()
                        {
                            Tenant = tenant,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    APInvoiceStatusUpdateBatch aPInvoiceStatusUpdateBatch = new APInvoiceStatusUpdateBatch();
                    APInvoiceStatusUpdateArg aPInvoiceStatusUpdateArg = new APInvoiceStatusUpdateArg()
                    {
                        Tenant = tenant,
                    };
                    aPInvoiceStatusUpdateBatch.RunAPInvoiceStatusUpdate(aPInvoiceStatusUpdateArg);
                    string responseText = aPInvoiceStatusUpdateBatch.ResponseText();
                    HttpStatusCode StatusCode = aPInvoiceStatusUpdateBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private bool CreateArgs(int tenant, string invoiceNumber, string fromInvoiceDate, string toInvoiceDate, string batch, ref APInvoiceStatusUpdateArg args, string message)
        {
            bool isSuccess = false;
            bool v_batch = false;
            if (String.IsNullOrWhiteSpace(invoiceNumber) && String.IsNullOrWhiteSpace(fromInvoiceDate))  
            {
                message = "FromInvoiceDate is a must, when no Invoice Number is provided";
                return isSuccess;
            }

            DateTime fromDateValue = DateTime.MinValue;
            DateTime toDateValue = DateTime.Today;

            if (!String.IsNullOrWhiteSpace(fromInvoiceDate))
            {
                try
                {
                    fromDateValue = DateTime.ParseExact(fromInvoiceDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    message = "Cannot parse FromInvoiceDate";
                    return isSuccess;

                }
            }
            if (!String.IsNullOrWhiteSpace(toInvoiceDate))
            {
                try
                {
                    toDateValue = DateTime.ParseExact(toInvoiceDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    message = "Cannot parse ToInvoiceDate";
                    return isSuccess;

                }
            }



            if (!String.IsNullOrEmpty(batch) && (batch == "1" || batch.ToUpperInvariant() == "TRUE"))
            {
                v_batch = true;
            }

            args = new APInvoiceStatusUpdateArg()
            {
                Tenant = tenant,
                InvoiceNumber = invoiceNumber,
                FromInvoiceDate = fromDateValue,
                ToInvoiceDate = toDateValue,    
                Batch = v_batch,
            };
            isSuccess = true;
            return isSuccess;
        }


    }
}