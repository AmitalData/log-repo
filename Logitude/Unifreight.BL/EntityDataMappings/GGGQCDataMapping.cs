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
    public class GGGQCDataMapping : IMapping<GGGQCPM, GGGQC>
    {

        public enum POCOPropertyNames
        {
            None,

        }
        public enum PMPropertyNames
        {
            None,
        }

        public void PMToPOCO(GGGQCPM entityPM, GGGQC entityPOCO)
        {
            entityPOCO.QUEID = entityPM.QUEID;
            entityPOCO.FIELDID = entityPM.FIELDID;
            entityPOCO.FIELDVAL = entityPM.FIELDVAL;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(GGGQCPM entityPM, GGGQC entityPOCO)
        {
            entityPM.QUEID = entityPOCO.QUEID;
            entityPM.FIELDID = entityPOCO.FIELDID;
            entityPM.FIELDVAL = entityPOCO.FIELDVAL;
            entityPM.Tenant = entityPOCO.tenant != null ? (int)entityPOCO.tenant : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;
        }

        public void CustomPMToPOCO(GGGQCPM entityPM, GGGQC entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GGGQCPM entityPM, GGGQC entityPOCO)
        {
            //throw new NotImplementedException();
        }


        public void PMToOldPM(GGGQCPM entityPM, GGGQCPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
