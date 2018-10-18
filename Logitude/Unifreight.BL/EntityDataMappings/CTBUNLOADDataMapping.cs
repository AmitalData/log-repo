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
    public class CTBUNLOADDataMapping : IMapping<CTBUNLOADPM, CTBUNLOAD>
    {
        public void PMToPOCO(CTBUNLOADPM entityPM, CTBUNLOAD entityPOCO)
        {
            entityPOCO.ULPORTID = entityPM.ULPORTID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBUNLOADPM entityPM, CTBUNLOAD entityPOCO)
        {
            entityPM.ULPORTID = entityPOCO.ULPORTID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBUNLOADPM entityPM, CTBUNLOAD entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBUNLOADPM entityPM, CTBUNLOAD entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBUNLOADPM entityPM, CTBUNLOADPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
