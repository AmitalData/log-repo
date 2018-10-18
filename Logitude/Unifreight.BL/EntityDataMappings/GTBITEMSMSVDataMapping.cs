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
    public class GTBITEMSMSVDataMapping : IMapping<GTBITEMSMSVPM, GTBITEMSMSV>
    {
        public void PMToPOCO(GTBITEMSMSVPM entityPM, GTBITEMSMSV entityPOCO)
        {
            entityPOCO.PRATID = entityPM.PRATID;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
        }

        public void POCOToPM(GTBITEMSMSVPM entityPM, GTBITEMSMSV entityPOCO)
        {
            entityPM.PRATID = entityPOCO.PRATID;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
        }

        public void CustomPMToPOCO(GTBITEMSMSVPM entityPM, GTBITEMSMSV entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTBITEMSMSVPM entityPM, GTBITEMSMSV entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTBITEMSMSVPM entityPM, GTBITEMSMSVPM oldEntityPM)
        {
        //    throw new System.NotImplementedException();
        }
    }
}
