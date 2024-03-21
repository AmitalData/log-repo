using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class CCUSUPITEMSIDataMapping : IMapping<CCUSUPITEMSIPM, CCUSUPITEMSI>
    {
        public void PMToPOCO(CCUSUPITEMSIPM entityPM, CCUSUPITEMSI entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.ACCLINENO = entityPM.ACCLINENO;
            entityPOCO.LINEID = entityPM.LINEID;
            entityPOCO.MOREDATA = entityPM.MOREDATA;
            entityPOCO.SICOUNTER = entityPM.SICOUNTER;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCHRONIZED = entityPM.IS_SYNCHRONIZED;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;

        }

        public void POCOToPM(CCUSUPITEMSIPM entityPM, CCUSUPITEMSI entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.ACCLINENO = entityPOCO.ACCLINENO;
            entityPM.LINEID = entityPOCO.LINEID;
            entityPM.MOREDATA = entityPOCO.MOREDATA;
            entityPM.SICOUNTER = entityPOCO.SICOUNTER;
            entityPM.Tenant = (int)entityPOCO.tenant;
            entityPM.IS_SYNCHRONIZED = (bool)entityPOCO.IS_SYNCHRONIZED;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT;

        }

        public void CustomPMToPOCO(CCUSUPITEMSIPM entityPM, CCUSUPITEMSI entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUSUPITEMSIPM entityPM, CCUSUPITEMSI entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void PMToOldPM(CCUSUPITEMSIPM entityPM, CCUSUPITEMSIPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
