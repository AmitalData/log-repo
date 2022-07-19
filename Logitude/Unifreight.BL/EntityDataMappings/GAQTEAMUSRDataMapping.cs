
using System;
using System.Collections.Generic;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class GAQTEAMUSRDataMapping : IMapping<GAQTEAMUSRPM, GAQTEAMUSR>
    {
        public void PMToPOCO(GAQTEAMUSRPM entityPM, GAQTEAMUSR entityPOCO)
        {
            entityPOCO.TEAMID = entityPM.TEAMID;
            entityPOCO.USRCODE = entityPM.USRCODE;


        }

        public void POCOToPM(GAQTEAMUSRPM entityPM, GAQTEAMUSR entityPOCO)
        {
            entityPM.TEAMID = entityPOCO.TEAMID;
            entityPM.USRCODE = entityPOCO.USRCODE;

        }

        public void CustomPMToPOCO(GAQTEAMUSRPM entityPM, GAQTEAMUSR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GAQTEAMUSRPM entityPM, GAQTEAMUSR entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GAQTEAMUSRPM entityPM, GAQTEAMUSRPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
