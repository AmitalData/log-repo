
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
   
   public partial class WorkFlowVersionDataMapping: IMapping<WorkFlowVersionPM, WorkFlowVersion>
   {

        public void CustomPMToPOCO(WorkFlowVersionPM entityPM, WorkFlowVersion entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(WorkFlowVersionPM entityPM, WorkFlowVersion entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(WorkFlowVersionPM entityPM, WorkFlowVersion entityPOCO)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Description);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.VersionNumber.ToString());
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }


}
   