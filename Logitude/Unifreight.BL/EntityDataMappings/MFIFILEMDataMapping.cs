
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
    public class MFIFILEMDataMapping : IMapping<MFIFILEMPM, MFIFILEM>
    {
        public void PMToPOCO(MFIFILEMPM entityPM, MFIFILEM entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.SPEDNO = entityPM.SPEDNO;
            entityPOCO.SMP = entityPM.SMP;
            entityPOCO.LOADPORT = entityPM.LOADPORT;
            entityPOCO.FLIGHTDATE = entityPM.FLIGHTDATE;
        }

        public void POCOToPM(MFIFILEMPM entityPM, MFIFILEM entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.SPEDNO = entityPOCO.SPEDNO;
            entityPM.SMP = entityPOCO.SMP;
            entityPM.LOADPORT = entityPOCO.LOADPORT;
            entityPM.FLIGHTDATE = entityPOCO.FLIGHTDATE;
        }

        public void CustomPMToPOCO(MFIFILEMPM entityPM, MFIFILEM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(MFIFILEMPM entityPM, MFIFILEM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(MFIFILEMPM entityPM, MFIFILEMPM oldEntityPM)
        {
            // throw new System.NotImplementedException();
        }
    }
}
