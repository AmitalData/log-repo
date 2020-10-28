using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class BIReportFolderExtendedController : ApiController
    {
        public HttpResponseMessage GetPermittedFolders(string loggedUserId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("BIReportFolder", "READ", authToken.Tenant);
                SecurityUtility.CheckContactFeature("BIReportFolder", "UPDATE", authToken.Tenant);
                
                IInfrastructureContext MyContext = InfrastructureContext.GetContext(authToken.Tenant);
                BIReportFolderQueryService bIReportFolderQuery = new BIReportFolderQueryService(MyContext);
                List<BIReportFolderList> result = bIReportFolderQuery.GetPermittedFoldersList(authToken.Tenant, loggedUserId);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}