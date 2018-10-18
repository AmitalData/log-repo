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
    public class CTBMISHGURDataMapping : IMapping<CTBMISHGURPM, CTBMISHGUR>
    {
        public void PMToPOCO(CTBMISHGURPM entityPM, CTBMISHGUR entityPOCO)
        {
            entityPOCO.SUGMISHGUR = entityPM.SUGMISHGUR;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBMISHGURPM entityPM, CTBMISHGUR entityPOCO)
        {
            entityPM.SUGMISHGUR = entityPOCO.SUGMISHGUR;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBMISHGURPM entityPM, CTBMISHGUR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBMISHGURPM entityPM, CTBMISHGUR entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBMISHGURPM entityPM, CTBMISHGURPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
