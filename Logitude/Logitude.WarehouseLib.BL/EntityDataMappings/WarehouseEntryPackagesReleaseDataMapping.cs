
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
using Simplog.Server.Infrastructure;

namespace Logitude.WarehouseLib.BL.EntityDataMappings
{
   
   public partial class WarehouseEntryPackagesReleaseDataMapping: IMapping<WarehouseEntryPackagesReleasePM, WarehouseEntryPackagesRelease>
   {

     
        public void CustomPMToPOCO(WarehouseEntryPackagesReleasePM entityPM, WarehouseEntryPackagesRelease entityPOCO)
        {
          
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }


        public void CustomPOCOToPM(WarehouseEntryPackagesReleasePM entityPM, WarehouseEntryPackagesRelease entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   