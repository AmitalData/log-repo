
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
    public class CTBTSRUFTYPEDataMapping : IMapping<CTBTSRUFTYPEPM, CTBTSRUFTYPE>
    {
        public void PMToPOCO(CTBTSRUFTYPEPM entityPM, CTBTSRUFTYPE entityPOCO)
        {
            entityPOCO.TSRUFAID = entityPM.TSRUFAID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.CODESCOPE = entityPM.CODESCOPE;
        }

        public void POCOToPM(CTBTSRUFTYPEPM entityPM, CTBTSRUFTYPE entityPOCO)
        {
            entityPM.TSRUFAID = entityPOCO.TSRUFAID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.CODESCOPE = entityPOCO.CODESCOPE;
        }

        public void CustomPMToPOCO(CTBTSRUFTYPEPM entityPM, CTBTSRUFTYPE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBTSRUFTYPEPM entityPM, CTBTSRUFTYPE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBTSRUFTYPEPM entityPM, CTBTSRUFTYPEPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
