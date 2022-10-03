using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using System.Data.Entity;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
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