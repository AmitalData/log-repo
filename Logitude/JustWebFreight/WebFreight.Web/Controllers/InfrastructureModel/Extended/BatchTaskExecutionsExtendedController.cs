using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class BatchTaskExecutionsExtendedController : ApiController
    {
        public HttpResponseMessage PostCancel(string batchTaskExecutionId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IInfrastructureContext MyContext = InfrastructureContext.GetContext(authToken.Tenant);
                BatchTaskExecutionQueryService batchTaskExecutionQuery = new BatchTaskExecutionQueryService(MyContext);
                batchTaskExecutionQuery.InitializeSettings();
                BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQuery.GetSingle(batchTaskExecutionId, true, false);
                batchTaskExecutionPM.StatusCode = "F";
                batchTaskExecutionPM.ChangeSetOp = ChangeSetOperation.Update;
                var batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(authToken.Tenant);
                batchTaskExecutionUpdateService.Update(batchTaskExecutionPM, true);

                return Request.CreateResponse(HttpStatusCode.OK, "OK");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}