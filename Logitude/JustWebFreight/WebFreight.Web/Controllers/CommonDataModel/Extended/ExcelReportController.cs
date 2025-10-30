using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.ExcelReport;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class ExcelReportController : ApiController
    {
        public HttpResponseMessage GetDataProviderFields(string reportId, string reportsTemplateId, int maxSubLevels = 1)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("ReportsTemplate", "REPORTTEMPLATEEXCEL", authToken.Tenant);

                ExcelReportService reportsTemplateQuery = new ExcelReportService(authToken.Tenant);
                ExcelReportResult myResult = reportsTemplateQuery.GetDataProviderFields(reportId, reportsTemplateId, maxSubLevels);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostDataProviderProperties(ExcelReportArguments excelReportArguments)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("ReportsTemplate", "REPORTTEMPLATEEXCEL", authToken.Tenant);


                ExcelReportUpdateService reportsTemplateQuery = new ExcelReportUpdateService(authToken.Tenant);
                reportsTemplateQuery.UpdateReport(excelReportArguments, authToken.Email);
                return Request.CreateResponse(HttpStatusCode.OK);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}