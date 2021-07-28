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
        }

        public void POCOToPM(GGGQCPM entityPM, GGGQC entityPOCO)
        {
            entityPM.QUEID = entityPOCO.QUEID;
            entityPM.FIELDID = entityPOCO.FIELDID;
            entityPM.FIELDVAL = entityPOCO.FIELDVAL;
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
