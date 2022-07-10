
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
    public class MSPSPEDDataMapping : IMapping<MSPSPEDPM, MSPSPED>
    {
        public void PMToPOCO(MSPSPEDPM entityPM, MSPSPED entityPOCO)
        {
            entityPOCO.SPDNO = entityPM.SPDNO;
            entityPOCO.MAINAWB = entityPM.MAINAWB;            
        }

        public void POCOToPM(MSPSPEDPM entityPM, MSPSPED entityPOCO)
        {
            entityPM.SPDNO = entityPOCO.SPDNO;
            entityPM.MAINAWB = entityPOCO.MAINAWB;
        }

        public void CustomPMToPOCO(MSPSPEDPM entityPM, MSPSPED entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(MSPSPEDPM entityPM, MSPSPED entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(MSPSPEDPM entityPM, MSPSPEDPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
