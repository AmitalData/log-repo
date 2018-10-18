
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.TimeManagement.BL.EntityDataMappings
{
   
   public partial class TMEmployeeTimeDataMapping: IMapping<TMEmployeeTimePM, TMEmployeeTime>
   {

        public void CustomPMToPOCO(TMEmployeeTimePM entityPM, TMEmployeeTime entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);

            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(TMEmployeeTimePM entityPM, TMEmployeeTime entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   