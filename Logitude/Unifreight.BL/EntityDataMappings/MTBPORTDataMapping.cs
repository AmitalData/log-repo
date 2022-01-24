
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
    public class MTBPORTDataMapping : IMapping<MTBPORTPM, MTBPORT>
    {
        public void PMToPOCO(MTBPORTPM entityPM, MTBPORT entityPOCO)
        {
            entityPOCO.PORTID = entityPM.PORTID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(MTBPORTPM entityPM, MTBPORT entityPOCO)
        {
            entityPM.PORTID = entityPOCO.PORTID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(MTBPORTPM entityPM, MTBPORT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(MTBPORTPM entityPM, MTBPORT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(MTBPORTPM entityPM, MTBPORTPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
