using Logitude.Workflow.BL.EntityQueryServices;
using Logitude.Workflow.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public partial class WorkFlowVersionViewsController : ApiController
    {
        public HttpResponseMessage GetByWorkflowId(string workflowId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("WorkFlowInstance", "READ", authToken.Tenant);
                IWorkflowContext MyContext = WorkflowContext.GetContext(authToken.Tenant);

                WorkFlowVersionQueryService workFlowVersionQuery = new WorkFlowVersionQueryService(MyContext);
                List<string> workflowversionIds = workFlowVersionQuery.GetVersionIds(workflowId, authToken.Tenant);

                ServiceResponse response = new ServiceResponse();
                response.Result = workflowversionIds;

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}