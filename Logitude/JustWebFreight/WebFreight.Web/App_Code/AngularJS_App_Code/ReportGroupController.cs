using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class ReportGroupController : ApiController
    {
        public HttpResponseMessage GetReportGroupLists(int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                ReportGroupRepository reportGroupRepository = new ReportGroupRepository(tenant);
                ReportGroupQuery reportGroupQuery = new ReportGroupQuery(reportGroupRepository);
                IQueryable<ReportGroup> ReportGroups = reportGroupRepository.GetReportGroups(tenant);
                IQueryable<ReportGroupList> query2 = reportGroupQuery.GetIQueryableEntityList(ReportGroups);
                return Request.CreateResponse(HttpStatusCode.OK, query2);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage getReportGroupListByCode(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ReportGroupRepository reportGroupRepository = new ReportGroupRepository(authToken.Tenant);
                ReportGroupQuery reportGroupQuery = new ReportGroupQuery(reportGroupRepository);
                ReportGroup reportGroup = reportGroupRepository.GetReportGroupByCode(code,0);
                return Request.CreateResponse(HttpStatusCode.OK, reportGroup);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}