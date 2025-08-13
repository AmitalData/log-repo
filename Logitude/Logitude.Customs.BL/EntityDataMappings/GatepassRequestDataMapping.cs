
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
   
   public partial class GatepassRequestDataMapping: IMapping<GatepassRequestPM, GatepassRequest>
   {

        public void CustomPMToPOCO(GatepassRequestPM entityPM, GatepassRequest entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.MasterCourierId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.MasterCourierId = entityPM.MasterCourierId;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(GatepassRequestPM entityPM, GatepassRequest entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   