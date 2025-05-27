
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class SupplierInvoiceItemsReqListExtendedController : ApiController
    {
        public HttpResponseMessage GetSingle(string declarationid, int linenumber, int invoicecounterkey, int invoiceitemlinenumber,string siirequestid)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICustomContext ctx = CustomContext.GetContext(authToken.Tenant);
                var qs = new SupplierInvoiceItemsReqListQueryService(ctx);
                qs.InitializeSettings();

                SupplierInvoiceItemsReqListPM pm;

                if (!string.IsNullOrWhiteSpace(siirequestid) && siirequestid != "null")
                {
                    pm = qs.GetSingle(declarationid,linenumber,invoicecounterkey,invoiceitemlinenumber, true, false);
                }
                else
                {
                    pm = qs.GetSinglePM(null, declarationid, linenumber, invoicecounterkey, invoiceitemlinenumber, authToken.Tenant);
                }

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, pm);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                       HttpStatusCode.BadRequest,
                       ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetProductFileExists(string modelCode,string importerNumber,string originCountry)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken =
                    AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    ApiExceptionBuilder.BuildException(ex));
            }
        }



        }
}