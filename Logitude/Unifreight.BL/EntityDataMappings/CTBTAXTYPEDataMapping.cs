
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
    public class CTBTAXTYPEDataMapping : IMapping<CTBTAXTYPEPM, CTBTAXTYPE>
    {
        public void PMToPOCO(CTBTAXTYPEPM entityPM, CTBTAXTYPE entityPOCO)
        {
            entityPOCO.TAXTYPE = entityPM.TAXTYPE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBTAXTYPEPM entityPM, CTBTAXTYPE entityPOCO)
        {
            entityPM.TAXTYPE = entityPOCO.TAXTYPE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBTAXTYPEPM entityPM, CTBTAXTYPE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBTAXTYPEPM entityPM, CTBTAXTYPE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBTAXTYPEPM entityPM, CTBTAXTYPEPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
