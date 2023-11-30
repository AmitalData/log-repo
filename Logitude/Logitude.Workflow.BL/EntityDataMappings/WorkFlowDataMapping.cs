using Logitude.Server.Tools; 
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class WorkFlowDataMapping: IMapping<WorkFlowPM, WorkFlow>
   {
        public void CustomPMToPOCO(WorkFlowPM entityPM, WorkFlow entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;

            entityPM.SearchFields = entityPM.Name;
            entityPOCO.SearchFields = entityPM.Name;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                entityPOCO.FlowJson = entityPOCO.FlowJson;
                CustomMappedPOCOProperties.Add(POCOPropertyNames.FlowJson);

                entityPOCO.Entity = entityPOCO.Entity;
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Entity);

                entityPOCO.Trigger = entityPOCO.Trigger;
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Trigger);

                entityPOCO.WorkFlowTriggerTypeCode = entityPOCO.WorkFlowTriggerTypeCode;
                CustomMappedPOCOProperties.Add(POCOPropertyNames.WorkFlowTriggerTypeCode);

                entityPOCO.WorkFlowNumber = entityPOCO.WorkFlowNumber;
                CustomMappedPOCOProperties.Add(POCOPropertyNames.WorkFlowNumber);
            }
        }

        public void CustomPOCOToPM(WorkFlowPM entityPM, WorkFlow entityPOCO)
        {
            WorkFlowVersionQueryService workFlowVersionQueryService = new WorkFlowVersionQueryService(entityPOCO.Tenant);
            var WorkflowVersions = workFlowVersionQueryService.GetAllWorkflowVersions(entityPOCO.Id, entityPOCO.Tenant);

            entityPM.WorkFlowVersions = WorkflowVersions;
        }
    }
}