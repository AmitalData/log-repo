using Logitude.BL.InvoiceModel.APIDataContract;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Logitude.SystemLogs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.Def.EntityPMs;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ARPaymentLiteController : ApiController
    {

        public HttpResponseMessage GetSingleARPaymentLite(string id, string number)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

            try
            {

                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("ARPayment", authToken.Tenant);

                ARPaymentQueryService Service = new ARPaymentQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
             // var Result = new ARPayment();
                var Result = new ARPaymentLite();
                if (!string.IsNullOrEmpty(id))
                {
                    Result = Service.GetARPaymentLiteById(id, tenant);
                }
                else if (!string.IsNullOrEmpty(number))
                {
                //  Result = Service.GetARPaymentByNumber(number, tenant);
                    Result = Service.GetARPaymentLiteByNumber(number, tenant);
                }
                //Result.PaymentInvoices = null;
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                //ExceptionHandler.HandleException
                //ExceptionHandler.HandleException(ex, DateTime.Now, authToken.Tenant , authToken.Email , "", "AuthenticationController : PostLoginData", null);

                //var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                string exceptionMessage = ex.Message + Environment.NewLine + ex.StackTrace;
                if (ex.InnerException != null)
                {
                    exceptionMessage += Environment.NewLine + "inner1: " + ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace;
                }


                return Request.CreateResponse(HttpStatusCode.InternalServerError, exceptionMessage);//(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}