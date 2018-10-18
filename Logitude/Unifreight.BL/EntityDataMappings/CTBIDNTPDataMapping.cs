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
    public class CTBIDNTPDataMapping : IMapping<CTBIDNTPPM, CTBIDNTP>
    {
        public void PMToPOCO(CTBIDNTPPM entityPM, CTBIDNTP entityPOCO)
        {
            entityPOCO.IDENTIFITYPE = entityPM.IDENTIFITYPE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBIDNTPPM entityPM, CTBIDNTP entityPOCO)
        {
            entityPM.IDENTIFITYPE = entityPOCO.IDENTIFITYPE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBIDNTPPM entityPM, CTBIDNTP entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBIDNTPPM entityPM, CTBIDNTP entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBIDNTPPM entityPM, CTBIDNTPPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}

