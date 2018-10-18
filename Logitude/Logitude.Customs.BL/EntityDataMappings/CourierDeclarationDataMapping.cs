
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CourierDeclarationDataMapping: IMapping<CourierDeclarationPM, CourierDeclaration>
   {

        public void CustomPMToPOCO(CourierDeclarationPM entityPM, CourierDeclaration entityPOCO)
        {

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CourierMasterId = entityPM.CourierMasterId;
             
            }
        }

        public void CustomPOCOToPM(CourierDeclarationPM entityPM, CourierDeclaration entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.DeclarationId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CourierMasterId);
        }
   }


}
   