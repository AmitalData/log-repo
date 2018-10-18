
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs; 
using Logitude.WarehouseLib.Data;

namespace Logitude.WarehouseLib.BL.EntityDataMappings
{
   
   public partial class WarehouseReleaseStatusDataMapping: IMapping<WarehouseReleaseStatusPM, WarehouseReleaseStatus>
   {

        public void CustomPMToPOCO(WarehouseReleaseStatusPM entityPM, WarehouseReleaseStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(WarehouseReleaseStatusPM entityPM, WarehouseReleaseStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   