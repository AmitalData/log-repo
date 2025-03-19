
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;


namespace WebFreight.Web.Controllers.InvoiceModel.Extended
{
    public class ARInvoiceExtendedController : ApiController
    {
        public HttpResponseMessage GetInvoiceSequenceStatus(string fromDate, string toDate)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                var fromDate2 = Convert.ToDateTime(fromDate);
                var toDate1 = Convert.ToDateTime(toDate);

                ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(tenant);
                List<ARInvoiceList> InvoiceSequence = invoiceQuery.GetInvoiceSequenceStatus(tenant , fromDate2, toDate1);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, InvoiceSequence);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}