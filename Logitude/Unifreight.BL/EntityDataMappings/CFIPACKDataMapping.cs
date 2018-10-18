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
    public class CFIPACKDataMapping : IMapping<CFIPACKPM, CFIPACK>
    {
        public void PMToPOCO(CFIPACKPM entityPM, CFIPACK entityPOCO)
        {
            entityPOCO.CONTNO = entityPM.CONTNO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.CONTTYPEID = entityPM.CONTTYPEID;
            entityPOCO.SEAL = entityPM.SEAL;
            entityPOCO.WEIGHT = entityPM.WEIGHT;
            entityPOCO.QTYADD = entityPM.QTYADD;
            entityPOCO.AVAILABILITY = entityPM.AVAILABILITY;
            entityPOCO.EXITFROMPORT = entityPM.EXITFROMPORT;
            entityPOCO.DAMAGE = entityPM.DAMAGE;
            entityPOCO.LACK = entityPM.LACK;
        }

        public void POCOToPM(CFIPACKPM entityPM, CFIPACK entityPOCO)
        {
            entityPM.CONTNO = entityPOCO.CONTNO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.CONTTYPEID = entityPOCO.CONTTYPEID;
            entityPM.SEAL = entityPOCO.SEAL;
            entityPM.WEIGHT = entityPOCO.WEIGHT;
            entityPM.QTYADD = entityPOCO.QTYADD;
            entityPM.AVAILABILITY = entityPOCO.AVAILABILITY;
            entityPM.EXITFROMPORT = entityPOCO.EXITFROMPORT;
            entityPM.DAMAGE = entityPOCO.DAMAGE;
            entityPM.LACK = entityPOCO.LACK;
        }

        public void CustomPMToPOCO(CFIPACKPM entityPM, CFIPACK entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIPACKPM entityPM, CFIPACK entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIPACKPM entityPM, CFIPACKPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
