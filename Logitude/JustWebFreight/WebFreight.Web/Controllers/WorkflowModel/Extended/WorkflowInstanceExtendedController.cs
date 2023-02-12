using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.Workflow.Data.EntityLists;
using Logitude.Workflow.Data.WorkflowStorage;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended
{
    public class WorkflowInstanceExtendedController : ApiController
    {
        public HttpResponseMessage GetActivities(string workflowInstanceId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authenticationToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authenticationToken.Tenant);
                SecurityUtility.CheckContactFeature("WorkFlowInstanceActivity", "READ", authenticationToken.Tenant);

                int tenant = authenticationToken.Tenant;

                WorkflowInstanceStorage workflowInstanceStorage = new WorkflowInstanceStorage();
                List<WorkFlowInstanceActivityList> workflowInstanceActivities = workflowInstanceStorage.GetActivities(workflowInstanceId, tenant)?.OrderBy(a => a.Sequence)?.ToList();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                if (workflowInstanceActivities == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception("Cannot get workflow instance activities")));
                }
                return Request.CreateResponse(HttpStatusCode.OK, workflowInstanceActivities);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetVariables(string workflowInstanceId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authenticationToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authenticationToken.Tenant);
                SecurityUtility.CheckContactFeature("WorkFlowInstanceVariable", "READ", authenticationToken.Tenant);

                int tenant = authenticationToken.Tenant;

                WorkflowInstanceStorage workflowInstanceStorage = new WorkflowInstanceStorage();
                List<WorkFlowInstanceVariableList> workflowInstanceVariables = workflowInstanceStorage.GetVariables(workflowInstanceId, tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                if (workflowInstanceVariables == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception("Cannot get workflow instance variables")));
                }
                return Request.CreateResponse(HttpStatusCode.OK, workflowInstanceVariables);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}