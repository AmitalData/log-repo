
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
    public class CTBPARTDataMapping : IMapping<CTBPARTPM, CTBPART>
    {
        public void PMToPOCO(CTBPARTPM entityPM, CTBPART entityPOCO)
        {
            entityPOCO.PARTIALITYID = entityPM.PARTIALITYID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBPARTPM entityPM, CTBPART entityPOCO)
        {
            entityPM.PARTIALITYID = entityPOCO.PARTIALITYID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBPARTPM entityPM, CTBPART entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBPARTPM entityPM, CTBPART entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBPARTPM entityPM, CTBPARTPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
