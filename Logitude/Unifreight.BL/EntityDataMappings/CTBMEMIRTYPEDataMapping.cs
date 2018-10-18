
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
    public class CTBMEMIRTYPEDataMapping : IMapping<CTBMEMIRTYPEPM, CTBMEMIRTYPE>
    {
        public void PMToPOCO(CTBMEMIRTYPEPM entityPM, CTBMEMIRTYPE entityPOCO)
        {
            entityPOCO.MEMIRTYPE = entityPM.MEMIRTYPE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBMEMIRTYPEPM entityPM, CTBMEMIRTYPE entityPOCO)
        {
            entityPM.MEMIRTYPE = entityPOCO.MEMIRTYPE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBMEMIRTYPEPM entityPM, CTBMEMIRTYPE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBMEMIRTYPEPM entityPM, CTBMEMIRTYPE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBMEMIRTYPEPM entityPM, CTBMEMIRTYPEPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
