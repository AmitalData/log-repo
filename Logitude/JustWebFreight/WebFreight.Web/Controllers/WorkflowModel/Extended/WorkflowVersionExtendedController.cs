using Logitude.Server.Tools.Helpers;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.EntityQueryServices;
using Logitude.Workflow.BL.EntityUpdateServices;
using Logitude.Workflow.Data;
using Logitude.Workflow.Data.WorkflowValidation.Exceptions;
using Logitude.Workflow.Data.WorkflowValidation.Exceptions.Builders;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended
{
    public class WorkflowVersionExtendedController : ApiController
    {
        public HttpResponseMessage PutActivate(string WorkFlowVersionId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("WorkFlowVersion", "UPDATE", authToken.Tenant);
                    SecurityUtility.AuthenticationOnEntityTenant("WorkFlowVersion", tenant, authToken.Tenant);

                    IWorkflowContext MyContext = WorkflowContext.GetContext(authToken.Tenant);
                    WorkFlowVersionQueryService workFlowVersionQuery = new WorkFlowVersionQueryService(MyContext);
                    workFlowVersionQuery.InitializeSettings();
                    WorkFlowVersionPM workFlowVersionPM = workFlowVersionQuery.GetSingle(WorkFlowVersionId, true, false);

                    WorkFlowVersionUpdateService service = new WorkFlowVersionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    service.InitializeEntityPM(workFlowVersionPM);
                    workFlowVersionPM.ChangeSetOp = ChangeSetOperation.Update;
                    workFlowVersionPM.StatusCode = "ACVE";

                    service.Update(workFlowVersionPM, true);

                    scope.Complete();
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                    return Request.CreateResponse(HttpStatusCode.OK, workFlowVersionPM);
                }
            }
            catch (WorkflowValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowValidationExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}