using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalPortalReportController : ApiController
    {
        [HttpPost]
        [Route("DigitalPortalReport/GetDigitalToExcelData")]
        public HttpResponseMessage GetDigitalToExcelData(GeneralFilters filters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, filters.CardId);
                var service = new DigitalPortalQueryToExcelExportService();
                var result = service.ExportQueryDataToExcel(filters);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalPortalReport/GetDigitalExportExecutionLogStatus")]
        public HttpResponseMessage GetDigitalExportExecutionLogStatus(string logId, string cardId)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);
                var queryExecutionLogRepository = new QueryExportExecutionLogRepository(authToken.Tenant);
                QueryExportExecutionLog queryExecutionLog = queryExecutionLogRepository.GetSingle(logId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, queryExecutionLog);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}