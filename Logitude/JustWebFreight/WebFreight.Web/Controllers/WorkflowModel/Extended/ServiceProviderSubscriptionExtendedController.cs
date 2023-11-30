using System;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.BL.EntityQueryServices;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended
{
    public class ServiceProviderSubscriptionExtendedController : ApiController
    {
        public HttpResponseMessage GetSingleByWorkflowNumber(string workflowNumber)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("ServiceProviderSubscription", "READ", authToken.Tenant);

                IWorkflowContext workflowContext = WorkflowContext.GetContext(authToken.Tenant);
                ServiceProviderSubscriptionQueryService serviceProviderSubscriptionQuery = new ServiceProviderSubscriptionQueryService(workflowContext);
                serviceProviderSubscriptionQuery.InitializeSettings();
                ServiceProviderSubscriptionPM serviceProviderSubscriptionPM = serviceProviderSubscriptionQuery.GetSingleByWorkflowNumber(workflowNumber, authToken.Tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, serviceProviderSubscriptionPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}