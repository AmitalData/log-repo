using System;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;
using Marvin.JsonPatch;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.Workflow;
using WebFreight.Web.Security;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.EntityQueryServices;
using Logitude.Workflow.BL.EntityUpdateServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Workflow.Data;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended.FlowEntities
{
    public class FlowTaskController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            int tenant = 0;

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                tenant = authToken.Tenant;

                TaskPM taskPM = GetTaskPM(id, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, taskPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
            }
        }

        public HttpResponseMessage Post(TaskPM entityPM)
        {
            int tenant = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("Task", entityPM.Tenant, authToken.Tenant);
                        tenant = authToken.Tenant;

                        CreateTask(entityPM);

                        entityPM = GetTaskPM(entityPM.Id, entityPM.Tenant);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage Put(TaskPM entityPM)
        {
            int tenant = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("Task", entityPM.Tenant, authToken.Tenant);
                        tenant = authToken.Tenant;

                        UpdateTask(entityPM);

                        entityPM = GetTaskPM(entityPM.Id, entityPM.Tenant);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage Patch(string id, JsonPatchDocument<TaskPM> taskJsonPatch)
        {
            int tenant = 0;

            try
            {
                using (TransactionScope transactionScope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    tenant = authToken.Tenant;

                    TaskPM taskPM = GetTaskPM(id, tenant);

                    if (taskPM == null) { throw new Exception("Cannot find the task"); }

                    SecurityUtility.AuthenticationOnEntityTenant("Task", taskPM.Tenant, tenant);

                    taskJsonPatch.ApplyTo(taskPM);

                    UpdateTask(taskPM);

                    TaskPM updatedTaskPM = GetTaskPM(taskPM.Id, taskPM.Tenant);

                    transactionScope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, updatedTaskPM);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(exception, tenant));
            }
        }

        private TaskPM GetTaskPM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                IWorkflowContext workflowContext = WorkflowContext.GetContext(tenant);
                TaskQueryService taskQueryService = new TaskQueryService(workflowContext);
                TaskPM taskPM = taskQueryService.GetSingle(id, true, false);
                return taskPM;
            }
            return null;
        }

        private void CreateTask(TaskPM taskPM)
        {
            if (taskPM != null)
            {
                IWorkflowContext workflowContext = WorkflowContext.GetContext(taskPM.Tenant);
                TaskUpdateService taskUpdateService = new TaskUpdateService(workflowContext, new Dictionary<string, IContext>(), taskPM.Tenant);
                taskPM.ChangeSetOp = ChangeSetOperation.Insert;
                taskUpdateService.Update(taskPM, true);
            }
        }

        private void UpdateTask(TaskPM taskPM)
        {
            if (taskPM != null)
            {
                IWorkflowContext workflowContext = WorkflowContext.GetContext(taskPM.Tenant);
                TaskUpdateService taskUpdateService = new TaskUpdateService(workflowContext, new Dictionary<string, IContext>(), taskPM.Tenant);
                taskPM.ChangeSetOp = ChangeSetOperation.Update;
                taskUpdateService.Update(taskPM, true);
            }
        }
    }
}