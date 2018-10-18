
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
    public class CTBTARIFFDataMapping : IMapping<CTBTARIFFPM, CTBTARIFF>
    {
        public void PMToPOCO(CTBTARIFFPM entityPM, CTBTARIFF entityPOCO)
        {
            entityPOCO.TARIFFID = entityPM.TARIFFID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBTARIFFPM entityPM, CTBTARIFF entityPOCO)
        {
            entityPM.TARIFFID = entityPOCO.TARIFFID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBTARIFFPM entityPM, CTBTARIFF entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBTARIFFPM entityPM, CTBTARIFF entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBTARIFFPM entityPM, CTBTARIFFPM oldEntityPM)
        {
          //  throw new System.NotImplementedException();
        }
    }
}
