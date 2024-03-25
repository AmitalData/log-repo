
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
    public class CCUSIGNUMDataMapping : IMapping<CCUSIGNUMPM, CCUSIGNUM>
    {
        public void PMToPOCO(CCUSIGNUMPM entityPM, CCUSIGNUM entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENOMSHGR = entityPM.LINENOMSHGR;
            entityPOCO.LINENOSIGN = entityPM.LINENOSIGN;
            entityPOCO.SIGNNUM = entityPM.SIGNNUM;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUSIGNUMPM entityPM, CCUSIGNUM entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENOMSHGR = entityPOCO.LINENOMSHGR;
            entityPM.LINENOSIGN = entityPOCO.LINENOSIGN;
            entityPM.SIGNNUM = entityPOCO.SIGNNUM;
            entityPM.Tenant = entityPOCO.tenant != null ? (int)entityPOCO.tenant : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;

        }

        public void CustomPMToPOCO(CCUSIGNUMPM entityPM, CCUSIGNUM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUSIGNUMPM entityPM, CCUSIGNUM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUSIGNUMPM entityPM, CCUSIGNUMPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
