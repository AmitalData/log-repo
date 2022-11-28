
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

namespace Logitude.Workflow.BL.EntityDataMappings
{
   
   public partial class WorkFlowVersionDataMapping: IMapping<WorkFlowVersionPM, WorkFlowVersion>
   {

        public void CustomPMToPOCO(WorkFlowVersionPM entityPM, WorkFlowVersion entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            entityPM.SearchFields = entityPM.Description;
            entityPOCO.SearchFields = entityPM.Description;
        }

        public void CustomPOCOToPM(WorkFlowVersionPM entityPM, WorkFlowVersion entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   