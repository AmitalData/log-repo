
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Transactions;
using Logitude.Customs.BL.AzureSearch;
using System.Threading.Tasks;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class SIIRequestExtendedController : ApiController
    {
        public HttpResponseMessage GetSingle(string declarationId, string id = null)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICustomContext ctx = CustomContext.GetContext(authToken.Tenant);
                var qs = new SIIRequestQueryService(ctx);
                qs.InitializeSettings();

                SIIRequestPM pm;

                if (!string.IsNullOrWhiteSpace(id) && id != "null")
                {
                    pm = qs.GetSingle(id, true, false);
                }
                else
                {
                    pm = qs.GetSinglePM(null, declarationId, authToken.Tenant);
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

        public HttpResponseMessage GetSupplierInvoiceItemsForSIIRequest(string declarationId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                var auth = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(auth.Tenant);

                var ctx = CustomContext.GetContext(auth.Tenant);
                var svc = new SIIRequestQueryService(ctx);
                svc.InitializeSettings();
                var list = svc.GetSupplierInvoiceItems(declarationId, auth.Tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, list);
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