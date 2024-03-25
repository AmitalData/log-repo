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
    public class CCUQUELOCKDataMapping : IMapping<CCUQUELOCKPM, CCUQUELOCK>
    {
        public void PMToPOCO(CCUQUELOCKPM entityPM, CCUQUELOCK entityPOCO)
        {
            entityPOCO.ENTNAME = entityPM.ENTNAME;
            entityPOCO.FILE_NO = entityPM.FILENO;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUQUELOCKPM entityPM, CCUQUELOCK entityPOCO)
        {
            entityPM.ENTNAME = entityPOCO.ENTNAME;
            entityPM.FILENO = entityPOCO.FILE_NO;
            entityPM.Tenant = entityPOCO.tenant != null ? (int)entityPOCO.tenant : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;
        }

        public void CustomPMToPOCO(CCUQUELOCKPM entityPM, CCUQUELOCK entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUQUELOCKPM entityPM, CCUQUELOCK entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUQUELOCKPM entityPM, CCUQUELOCKPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
