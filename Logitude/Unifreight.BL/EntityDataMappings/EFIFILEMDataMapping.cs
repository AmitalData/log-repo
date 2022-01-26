
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
    public class EFIFILEMDataMapping : IMapping<EFIFILEMPM, EFIFILEM>
    {
        public void PMToPOCO(EFIFILEMPM entityPM, EFIFILEM entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.SMP = entityPM.SMP;
            entityPOCO.FLIGHTDATE = entityPM.FLIGHT_DATE;
            entityPOCO.SPEDNO = entityPOCO.SPEDNO;
        }

        public void POCOToPM(EFIFILEMPM entityPM, EFIFILEM entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.SMP = entityPOCO.SMP;
            entityPM.FLIGHT_DATE = entityPOCO.FLIGHTDATE;
            entityPM.SPEDNO = entityPOCO.SPEDNO;

        }

        public void CustomPMToPOCO(EFIFILEMPM entityPM, EFIFILEM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(EFIFILEMPM entityPM, EFIFILEM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(EFIFILEMPM entityPM, EFIFILEMPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
