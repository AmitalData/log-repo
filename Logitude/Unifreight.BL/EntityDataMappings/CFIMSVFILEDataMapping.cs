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
    public class CFIMSVFILEDataMapping : IMapping<CFIMSVFILEPM, CFIMSVFILE>
    {
        public void PMToPOCO(CFIMSVFILEPM entityPM, CFIMSVFILE entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.REMARK = entityPM.REMARK;
            entityPOCO.AQOPERATION = entityPM.AQOPERATION;
        }

        public void POCOToPM(CFIMSVFILEPM entityPM, CFIMSVFILE entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.REMARK = entityPOCO.REMARK;
            entityPM.AQOPERATION = entityPOCO.AQOPERATION;
        }

        public void CustomPMToPOCO(CFIMSVFILEPM entityPM, CFIMSVFILE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIMSVFILEPM entityPM, CFIMSVFILE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIMSVFILEPM entityPM, CFIMSVFILEPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
