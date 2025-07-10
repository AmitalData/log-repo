
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
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

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

    }
}