
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   
   public partial class WorkFlowInstanceDataMapping: IMapping<WorkFlowInstancePM, WorkFlowInstance>
   {

        public void CustomPMToPOCO(WorkFlowInstancePM entityPM, WorkFlowInstance entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(WorkFlowInstancePM entityPM, WorkFlowInstance entityPOCO)
        {
            //throw new NotImplementedException();
        }
        private void BuildSearchFields(WorkFlowInstancePM entityPM, WorkFlowInstance entityPOCO)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.BusinessKey);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.WorkFlowVersionNumber.ToString());
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }


}
   