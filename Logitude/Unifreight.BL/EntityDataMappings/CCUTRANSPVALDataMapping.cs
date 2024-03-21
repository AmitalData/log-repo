
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
    public class CCUTRANSPVALDataMapping : IMapping<CCUTRANSPVALPM, CCUTRANSPVAL>
    {
        public void PMToPOCO(CCUTRANSPVALPM entityPM, CCUTRANSPVAL entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.TRANSPVALFC = entityPM.TRANSPVALFC;
            entityPOCO.CURRID = entityPM.CURRID;
            entityPOCO.TRANSPVAL = entityPM.TRANSPVAL;
            entityPOCO.CURRIDN = entityPM.CURRIDN;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCHRONIZED = entityPM.IS_SYNCHRONIZED;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUTRANSPVALPM entityPM, CCUTRANSPVAL entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.TRANSPVALFC = entityPOCO.TRANSPVALFC;
            entityPM.CURRID = entityPOCO.CURRID;
            entityPM.TRANSPVAL = entityPOCO.TRANSPVAL;
            entityPM.CURRIDN = entityPOCO.CURRIDN;
            entityPM.Tenant = (int)entityPOCO.tenant;
            entityPM.IS_SYNCHRONIZED = (bool)entityPOCO.IS_SYNCHRONIZED;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT;

        }

        public void CustomPMToPOCO(CCUTRANSPVALPM entityPM, CCUTRANSPVAL entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUTRANSPVALPM entityPM, CCUTRANSPVAL entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUTRANSPVALPM entityPM, CCUTRANSPVALPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
