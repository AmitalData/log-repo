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
    public class CTBTRANSPDataMapping : IMapping<CTBTRANSPPM, CTBTRANSP>
    {
        public void PMToPOCO(CTBTRANSPPM entityPM, CTBTRANSP entityPOCO)
        {
            entityPOCO.TRANSPTYPE = entityPM.TRANSPTYPE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBTRANSPPM entityPM, CTBTRANSP entityPOCO)
        {
            entityPM.TRANSPTYPE = entityPOCO.TRANSPTYPE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBTRANSPPM entityPM, CTBTRANSP entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBTRANSPPM entityPM, CTBTRANSP entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBTRANSPPM entityPM, CTBTRANSPPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
