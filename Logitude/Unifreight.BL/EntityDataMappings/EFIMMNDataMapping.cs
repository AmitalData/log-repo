
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
    public class EFIMMNDataMapping : IMapping<EFIMMNPM, EFIMMN>
    {
        public void PMToPOCO(EFIMMNPM entityPM, EFIMMN entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.STORGENO = entityPM.STORGENO;
            entityPOCO.WAREHOUSE = entityPM.WAREHOUSE;
        }

        public void POCOToPM(EFIMMNPM entityPM, EFIMMN entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.STORGENO = entityPOCO.STORGENO;
            entityPM.WAREHOUSE = entityPOCO.WAREHOUSE;
        }

        public void CustomPMToPOCO(EFIMMNPM entityPM, EFIMMN entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(EFIMMNPM entityPM, EFIMMN entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(EFIMMNPM entityPM, EFIMMNPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
