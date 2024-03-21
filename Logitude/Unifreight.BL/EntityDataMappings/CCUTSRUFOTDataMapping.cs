using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class CCUTSRUFOTDataMapping : IMapping<CCUTSRUFOTPM, CCUTSRUFOT>
    {
        public void PMToPOCO(CCUTSRUFOTPM entityPM, CCUTSRUFOT entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.TSRUFAID = entityPM.TSRUFAID;
            entityPOCO.QUANTITY = entityPM.QUANTITY;
            entityPOCO.TSRUFANO = entityPM.TSRUFANO;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCHRONIZED = entityPM.IS_SYNCHRONIZED;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUTSRUFOTPM entityPM, CCUTSRUFOT entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.TSRUFAID = entityPOCO.TSRUFAID;
            entityPM.QUANTITY = entityPOCO.QUANTITY;
            entityPM.TSRUFANO = entityPOCO.TSRUFANO;
            entityPM.Tenant = (int)entityPOCO.tenant;
            entityPM.IS_SYNCHRONIZED = (bool)entityPOCO.IS_SYNCHRONIZED;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT;

        }

        public void CustomPMToPOCO(CCUTSRUFOTPM entityPM, CCUTSRUFOT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUTSRUFOTPM entityPM, CCUTSRUFOT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUTSRUFOTPM entityPM, CCUTSRUFOTPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
