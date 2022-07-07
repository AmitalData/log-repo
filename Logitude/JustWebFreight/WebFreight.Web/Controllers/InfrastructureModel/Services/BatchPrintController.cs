using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.BatchPrint;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Services
{
    public class BatchPrintController : ApiController
    {
        public HttpResponseMessage Post(BatchPrintManagerArgs batchPrintManagerArgs)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                batchPrintManagerArgs.Tenant = authToken.Tenant;
                batchPrintManagerArgs.Email = HttpContext.Current.User.Identity.Name;
                BatchPrintService batchPrintService = new BatchPrintService();
                var batchTaskId = batchPrintService.Print(batchPrintManagerArgs);

                return Request.CreateResponse(HttpStatusCode.OK, batchTaskId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}
