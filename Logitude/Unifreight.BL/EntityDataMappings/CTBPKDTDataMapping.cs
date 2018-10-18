
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
    public class CTBPKDTDataMapping : IMapping<CTBPKDTPM, CTBPKDT>
    {
        public void PMToPOCO(CTBPKDTPM entityPM, CTBPKDT entityPOCO)
        {
            entityPOCO.PACKDETAIL = entityPM.PACKDETAIL;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBPKDTPM entityPM, CTBPKDT entityPOCO)
        {
            entityPM.PACKDETAIL = entityPOCO.PACKDETAIL;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBPKDTPM entityPM, CTBPKDT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBPKDTPM entityPM, CTBPKDT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBPKDTPM entityPM, CTBPKDTPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
