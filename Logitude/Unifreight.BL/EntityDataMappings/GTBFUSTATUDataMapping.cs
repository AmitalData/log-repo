
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
    public class GTBFUSTATUDataMapping : IMapping<GTBFUSTATUPM, GTBFUSTATU>
    {
        public void PMToPOCO(GTBFUSTATUPM entityPM, GTBFUSTATU entityPOCO)
        {
            entityPOCO.ENTNAME = entityPM.ENTNAME;
            entityPOCO.STATUSCODE = entityPM.STATUSCODE;
        }

        public void POCOToPM(GTBFUSTATUPM entityPM, GTBFUSTATU entityPOCO)
        {
            entityPM.ENTNAME = entityPOCO.ENTNAME;
            entityPM.STATUSCODE = entityPOCO.STATUSCODE;
        }

        public void CustomPMToPOCO(GTBFUSTATUPM entityPM, GTBFUSTATU entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTBFUSTATUPM entityPM, GTBFUSTATU entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTBFUSTATUPM entityPM, GTBFUSTATUPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
