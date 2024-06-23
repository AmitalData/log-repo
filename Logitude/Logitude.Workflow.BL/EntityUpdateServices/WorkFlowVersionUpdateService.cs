using Logitude.Server.Tools;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.ExternalAPI;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.WorkflowValidation.Constants;
using Logitude.Workflow.Data.WorkflowValidation.Exceptions;
using Logitude.Workflow.Data.WorkflowValidation.Models;
using RestSharp;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Logitude.Workflow.BL.EntityUpdateServices
{
    public partial class WorkFlowVersionUpdateService
    {
        protected override void OnCreating(WorkFlowVersionPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                SetWorkFlowVersionDefaultValues(entityPM);
                HandleWorkFlowVersionCreateOrUpdate(entityPM);
            }
        }

        protected override void OnUpdating(WorkFlowVersionPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                HandleWorkFlowVersionCreateOrUpdate(entityPM);
            }
        }

        private static void SetWorkFlowVersionDefaultValues(WorkFlowVersionPM entityPM)
        {
            WorkFlowVersionRepository workFlowVersionRepository = new WorkFlowVersionRepository(entityPM.Tenant);
            var workFlowVersions = workFlowVersionRepository.GetAllByWorkflowId(entityPM.Tenant, entityPM.WorkflowId).ToList();
            var versionNumber = workFlowVersions?.Count() == 0 ? 1 : workFlowVersions.LastOrDefault().VersionNumber + 1;
            entityPM.VersionNumber = versionNumber;
            entityPM.StatusCode = "DRFT";
        }

        private static void HandleWorkFlowVersionCreateOrUpdate(WorkFlowVersionPM entityPM)
        {
            WorkFlowVersionRepository workFlowVersionRepository = new WorkFlowVersionRepository(entityPM.Tenant);
            var workFlowVersions = workFlowVersionRepository.GetAllByWorkflowId(entityPM.Tenant, entityPM.WorkflowId).OrderByDescending(w => w.VersionNumber).ToList();
            var activeWorkFlowVersions = workFlowVersions.Where(w => w.Id != entityPM.Id && w.StatusCode == "ACVE");

            if (entityPM.StatusCode == "ACVE")
            {
                ActivationValidation(entityPM);

                foreach (var activeWorkFlowVersion in activeWorkFlowVersions)
                {
                    activeWorkFlowVersion.StatusCode = "INVE";
                    workFlowVersionRepository.Update(activeWorkFlowVersion);
                }
                DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.ActivatedDate = myDate;
                workFlowVersionRepository.SubmitChanges();

                UpdateWorkflow(entityPM);
            }


            if (entityPM.StatusCode == "DRFT" || entityPM.StatusCode == "INVE")
            {
                if (activeWorkFlowVersions.Any())
                {
                    WorkFlowVersion workflow = activeWorkFlowVersions.FirstOrDefault();
                    UpdateWorkflow(workflow);
                }
                else
                {
                    if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
                    {
                        UpdateWorkflow(entityPM);
                    }
                    else
                    {
                        WorkFlowVersion workflow = workFlowVersions.FirstOrDefault();
                        if(workflow.Id == entityPM.Id)
                        {
                            UpdateWorkflow(entityPM);
                        }
                        else
                        {
                            UpdateWorkflow(workflow);
                        }
                    }
                }
            }
        }

        private static void ActivationValidation(WorkFlowVersionPM entityPM)
        {
            List<string> workfloeVersionAvtivationErrors = new List<string>();
            string workfloeVersionAvtivationUrl = ConfigurationManager.AppSettings["WorkflowEngineURL"] + ExternalApiURLs.GetWorkFlowVersionActivationValidation(entityPM.Id);

            List<WorkflowActivationError> workflowActivationErrors = APICaller.CallApi<List<WorkflowActivationError>>(workfloeVersionAvtivationUrl, null, Method.Get);
            if (workflowActivationErrors.Count > 0)
            {
                foreach (WorkflowActivationError workflowActivationError in workflowActivationErrors)
                {
                    workfloeVersionAvtivationErrors.AddRange(workflowActivationError.ErrorMessages);
                }

                throw new WorkflowValidationException(workfloeVersionAvtivationErrors);
            }
        }

        private static void UpdateWorkflow(WorkFlowVersionPM workFlowVersionPM)
        {
            if (workFlowVersionPM != null)
            {
                WorkFlowRepository workFlowRepository = new WorkFlowRepository(workFlowVersionPM.Tenant);
                var workflow = workFlowRepository.GetSingle(workFlowVersionPM.WorkflowId, workFlowVersionPM.Tenant);

                workflow.StatusCode = workFlowVersionPM.StatusCode;
                workflow.Entity = workFlowVersionPM.Entity;
                workflow.Trigger = workFlowVersionPM.Trigger;
                workflow.FlowJson = workFlowVersionPM.FlowJson;

                UpdateDateWorkflow(workflow);

                workFlowRepository.Update(workflow);
                workFlowRepository.SubmitChanges();
            }
        }

        private static void UpdateWorkflow(WorkFlowVersion workFlowVersion)
        {
            if (workFlowVersion != null)
            {
                WorkFlowRepository workFlowRepository = new WorkFlowRepository(workFlowVersion.Tenant);
                var workflow = workFlowRepository.GetSingle(workFlowVersion.WorkflowId, workFlowVersion.Tenant);

                workflow.StatusCode = workFlowVersion.StatusCode;
                workflow.Entity = workFlowVersion.Entity;
                workflow.Trigger = workFlowVersion.Trigger;
                workflow.FlowJson = workFlowVersion.FlowJson;

                UpdateDateWorkflow(workflow);

                workFlowRepository.Update(workflow);
                workFlowRepository.SubmitChanges();
            }
        }

        private static void UpdateDateWorkflow(WorkFlow workflow)
        {
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(workflow.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, workflow.Tenant);
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(workflow.Tenant);

            workflow.UpdateDate = currentDateTime;

            if (loggedContact != null)
            {
                workflow.UpdatedByUserId = loggedContact.Id;
            }
        }
    }
}