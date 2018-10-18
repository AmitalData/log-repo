
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
   
   public partial class WarehouseReleasePackageDataMapping: IMapping<WarehouseReleasePackagePM, WarehouseReleasePackage>
   {

        public void CustomPMToPOCO(WarehouseReleasePackagePM entityPM, WarehouseReleasePackage entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);

            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(WarehouseReleasePackagePM entityPM, WarehouseReleasePackage entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   