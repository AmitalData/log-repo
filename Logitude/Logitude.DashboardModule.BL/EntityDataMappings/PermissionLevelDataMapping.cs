
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs; 
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{
   
   public partial class PermissionLevelDataMapping: IMapping<PermissionLevelPM, PermissionLevel>
   {

        public void CustomPMToPOCO(PermissionLevelPM entityPM, PermissionLevel entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(PermissionLevelPM entityPM, PermissionLevel entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   