
using Logitude.Server.Tools; 
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class WorkFlowDataMapping: IMapping<WorkFlowPM, WorkFlow>
   {
        public void CustomPMToPOCO(WorkFlowPM entityPM, WorkFlow entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            entityPM.SearchFields = entityPM.Name;
            entityPOCO.SearchFields = entityPM.Name;
        }

        public void CustomPOCOToPM(WorkFlowPM entityPM, WorkFlow entityPOCO)
        {
            //throw new NotImplementedException();
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