
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
   
   public partial class WorkFlowInstanceActivityStatusDataMapping: IMapping<WorkFlowInstanceActivityStatusPM, WorkFlowInstanceActivityStatus>
   {

        public void CustomPMToPOCO(WorkFlowInstanceActivityStatusPM entityPM, WorkFlowInstanceActivityStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(WorkFlowInstanceActivityStatusPM entityPM, WorkFlowInstanceActivityStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   