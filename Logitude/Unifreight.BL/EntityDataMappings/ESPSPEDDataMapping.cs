
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
    public class ESPSPEDDataMapping : IMapping<ESPSPEDPM, ESPSPED>
    {
        public void PMToPOCO(ESPSPEDPM entityPM, ESPSPED entityPOCO)
        {
            entityPOCO.SPDNO = entityPM.SPD_NO;
            entityPOCO.MAINAWB = entityPM.MAIN_AWB;

        }

        public void POCOToPM(ESPSPEDPM entityPM, ESPSPED entityPOCO)
        {
            entityPM.SPD_NO = entityPOCO.SPDNO;
            entityPM.MAIN_AWB = entityPOCO.MAINAWB;

        }

        public void CustomPMToPOCO(ESPSPEDPM entityPM, ESPSPED entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ESPSPEDPM entityPM, ESPSPED entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ESPSPEDPM entityPM, ESPSPEDPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
