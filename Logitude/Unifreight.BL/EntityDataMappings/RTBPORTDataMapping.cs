
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
    public class RTBPORTDataMapping : IMapping<RTBPORTPM, RTBPORT>
    {
        public void PMToPOCO(RTBPORTPM entityPM, RTBPORT entityPOCO)
        {
            entityPOCO.PORTID = entityPM.PORTID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(RTBPORTPM entityPM, RTBPORT entityPOCO)
        {
            entityPM.PORTID = entityPOCO.PORTID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(RTBPORTPM entityPM, RTBPORT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(RTBPORTPM entityPM, RTBPORT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(RTBPORTPM entityPM, RTBPORTPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
