
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
    public class GGGHDAYDataMapping : IMapping<GGGHDAYPM, GGGHDAY>
    {
        public void PMToPOCO(GGGHDAYPM entityPM, GGGHDAY entityPOCO)
        {
           // entityPOCO.HOLIDAY = entityPM.HOLIDAY;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
        }

        public void POCOToPM(GGGHDAYPM entityPM, GGGHDAY entityPOCO)
        {
           // entityPM.HOLIDAY = entityPOCO.HOLIDAY;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
        }

        public void CustomPMToPOCO(GGGHDAYPM entityPM, GGGHDAY entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GGGHDAYPM entityPM, GGGHDAY entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GGGHDAYPM entityPM, GGGHDAYPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
