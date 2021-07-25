using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShardLogistics
{
    public class SharedLogisticReportController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ExportReportToExcelFile(SharedLogisticReportFilters sharedLogisticReportFilters)
        {
            try
            {
                AuthenticationToken authenticationToken = GetAuthenticationToken();
                SecurityUtility.AuthenticationOnTenant(authenticationToken.Tenant);
                SecurityUtility.CheckSharedContactAuthentication(authenticationToken.Tenant, sharedLogisticReportFilters.PartnerId);
                SharedLogisticReportService sharedLogisticReportService = new SharedLogisticReportService(sharedLogisticReportFilters, authenticationToken.Tenant);
                ExportReportResult exportReportResult =  sharedLogisticReportService.ExportReportToExcelFileOnStorage();
                return Request.CreateResponse(HttpStatusCode.OK, exportReportResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage Get()
        {
            try
            {



                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            return AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        }


    }


    public class SharedLogisticReportFilters
    {

        public string ObjectTableName { get; set; }
        public string SortByColumnName { get; set; }
        public string QuerySection { get; set; }
        public string SortDirectin { get; set; }
        public string PartnerId { get; set; }
        public string ContactId { get; set; }
        public int Tenant { get; set; }
        public string QueryCode { get; set; }
        public string ReportName { get; set; }

        
        public List<QueryFilterItem> QueryFilterItems { get; set; }


    }
}