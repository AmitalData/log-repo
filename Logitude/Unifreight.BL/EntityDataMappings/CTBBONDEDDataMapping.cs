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
    public class CTBBONDEDDataMapping : IMapping<CTBBONDEDPM, CTBBONDED>
    {
        public void PMToPOCO(CTBBONDEDPM entityPM, CTBBONDED entityPOCO)
        {
            entityPOCO.WAREHOUSEID = entityPM.WAREHOUSEID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBBONDEDPM entityPM, CTBBONDED entityPOCO)
        {
            entityPM.WAREHOUSEID = entityPOCO.WAREHOUSEID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBBONDEDPM entityPM, CTBBONDED entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBBONDEDPM entityPM, CTBBONDED entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBBONDEDPM entityPM, CTBBONDEDPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
