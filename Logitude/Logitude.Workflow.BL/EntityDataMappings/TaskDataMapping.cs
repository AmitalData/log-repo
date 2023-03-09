
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   
   public partial class TaskDataMapping: IMapping<TaskPM, Task>
   {

        public void CustomPMToPOCO(TaskPM entityPM, Task entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(TaskPM entityPM, Task entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   