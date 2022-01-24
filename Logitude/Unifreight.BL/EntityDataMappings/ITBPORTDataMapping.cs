
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
    public class ITBPORTDataMapping : IMapping<ITBPORTPM, ITBPORT>
    {
        public void PMToPOCO(ITBPORTPM entityPM, ITBPORT entityPOCO)
        {
            entityPOCO.PORTID = entityPM.PORTID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(ITBPORTPM entityPM, ITBPORT entityPOCO)
        {
            entityPM.PORTID = entityPOCO.PORTID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(ITBPORTPM entityPM, ITBPORT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ITBPORTPM entityPM, ITBPORT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ITBPORTPM entityPM, ITBPORTPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
