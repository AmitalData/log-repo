
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
    public class CTBPACKTYPEDataMapping : IMapping<CTBPACKTYPEPM, CTBPACKTYPE>
    {
        public void PMToPOCO(CTBPACKTYPEPM entityPM, CTBPACKTYPE entityPOCO)
        {
            entityPOCO.PACKTYPEID = entityPM.PACKTYPEID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBPACKTYPEPM entityPM, CTBPACKTYPE entityPOCO)
        {
            entityPM.PACKTYPEID = entityPOCO.PACKTYPEID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBPACKTYPEPM entityPM, CTBPACKTYPE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBPACKTYPEPM entityPM, CTBPACKTYPE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBPACKTYPEPM entityPM, CTBPACKTYPEPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
