
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ReferantTeamDataMapping: IMapping<ReferantTeamPM, ReferantTeam>
   {

        public void CustomPMToPOCO(ReferantTeamPM entityPM, ReferantTeam entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
                entityPOCO.Code = entityPM.Code;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(ReferantTeamPM entityPM, ReferantTeam entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   