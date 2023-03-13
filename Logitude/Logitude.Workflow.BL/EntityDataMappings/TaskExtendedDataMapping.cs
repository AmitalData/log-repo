using Logitude.Server.Tools; 
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class TaskExtendedDataMapping: IMapping<TaskExtendedPM, TaskExtended>
   {
        public void CustomPMToPOCO(TaskExtendedPM entityPM, TaskExtended entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(TaskExtendedPM entityPM, TaskExtended entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }
}