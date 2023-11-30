
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class DigitalProfileDataMapping: IMapping<DigitalProfilePM, DigitalProfile>
   {

        public void CustomPMToPOCO(DigitalProfilePM entityPM, DigitalProfile entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(DigitalProfilePM entityPM, DigitalProfile entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   