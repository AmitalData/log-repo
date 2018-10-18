
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
    public class CTBLOADDataMapping : IMapping<CTBLOADPM, CTBLOAD>
    {
        public void PMToPOCO(CTBLOADPM entityPM, CTBLOAD entityPOCO)
        {
            entityPOCO.LPORTID = entityPM.LPORTID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBLOADPM entityPM, CTBLOAD entityPOCO)
        {
            entityPM.LPORTID = entityPOCO.LPORTID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBLOADPM entityPM, CTBLOAD entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBLOADPM entityPM, CTBLOAD entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBLOADPM entityPM, CTBLOADPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
