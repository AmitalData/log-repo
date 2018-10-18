
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
    public class CTBERRORDataMapping : IMapping<CTBERRORPM, CTBERROR>
    {
        public void PMToPOCO(CTBERRORPM entityPM, CTBERROR entityPOCO)
        {
            entityPOCO.ERRORCODE = entityPM.ERRORCODE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBERRORPM entityPM, CTBERROR entityPOCO)
        {
            entityPM.ERRORCODE = entityPOCO.ERRORCODE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBERRORPM entityPM, CTBERROR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBERRORPM entityPM, CTBERROR entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBERRORPM entityPM, CTBERRORPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
