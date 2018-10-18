
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
    public class CTBINCOTERMDataMapping : IMapping<CTBINCOTERMPM, CTBINCOTERM>
    {
        public void PMToPOCO(CTBINCOTERMPM entityPM, CTBINCOTERM entityPOCO)
        {
            entityPOCO.PTERMID = entityPM.PTERMID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBINCOTERMPM entityPM, CTBINCOTERM entityPOCO)
        {
            entityPM.PTERMID = entityPOCO.PTERMID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBINCOTERMPM entityPM, CTBINCOTERM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBINCOTERMPM entityPM, CTBINCOTERM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBINCOTERMPM entityPM, CTBINCOTERMPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
