
using Logitude.Server.Tools; 
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.EntityQueryServices;

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


            entityPOCO.FlowJson = entityPOCO.FlowJson;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.FlowJson);

            entityPOCO.Entity = entityPOCO.Entity;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Entity);

            entityPOCO.Trigger = entityPOCO.Trigger;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Trigger);
        }

        public void CustomPOCOToPM(WorkFlowPM entityPM, WorkFlow entityPOCO)
        {
            WorkFlowVersionQueryService workFlowVersionQueryService = new WorkFlowVersionQueryService(entityPOCO.Tenant);
            var WorkflowVersions = workFlowVersionQueryService.GetAllWorkflowVersions(entityPOCO.Id, entityPOCO.Tenant);

            entityPM.WorkFlowVersions = WorkflowVersions;
        }

        //private void BuildSearchFields(WorkFlowPM entityPM, WorkFlow entityPOCO)
        //{
        //    string searchFields = "";
        //    MethodHelper.AddToSearchFields(ref searchFields, entityPM.x);
        //    MethodHelper.AddToSearchFields(ref searchFields, entityPM.y);
        //    UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
        //    User user = userRepository.GetSingleUser(entityPM.OwnerId, entityPM.Tenant);
        //    if (user != null)
        //    {
        //        MethodHelper.AddToSearchFields(ref searchFields, user.Contact.EnglishName);
        //    }
        //    entityPM.SearchFields = searchFields;
        //    entityPOCO.SearchFields = searchFields;
        //}
    }
}