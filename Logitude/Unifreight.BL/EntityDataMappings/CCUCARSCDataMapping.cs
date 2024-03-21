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
    public class CCUCARSCDataMapping : IMapping<CCUCARSCPM, CCUCARSC>
    {
        public void PMToPOCO(CCUCARSCPM entityPM, CCUCARSC entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.COUNTER = entityPM.COUNTER;
            entityPOCO.VEHICLEFILE = entityPM.VEHICLEFILE;
            entityPOCO.CARMODEL = entityPM.CARMODEL;
            entityPOCO.CHASSISNO = entityPM.CHASSISNO;
            entityPOCO.ENGINENO = entityPM.ENGINENO;
            entityPOCO.WINDOWNO = entityPM.WINDOWNO;
            entityPOCO.FOB = entityPM.FOB;
            entityPOCO.GENERALTAX = entityPM.GENERALTAX;
            entityPOCO.BUYTAX = entityPM.BUYTAX;
            entityPOCO.VATRESHIMON = entityPM.VATRESHIMON;
            entityPOCO.EXEMPTTYPE = entityPM.EXEMPTTYPE;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCHRONIZED = entityPM.IS_SYNCHRONIZED;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUCARSCPM entityPM, CCUCARSC entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.COUNTER = entityPOCO.COUNTER;
            entityPM.VEHICLEFILE = entityPOCO.VEHICLEFILE;
            entityPM.CARMODEL = entityPOCO.CARMODEL;
            entityPM.CHASSISNO = entityPOCO.CHASSISNO;
            entityPM.ENGINENO = entityPOCO.ENGINENO;
            entityPM.WINDOWNO = entityPOCO.WINDOWNO;
            entityPM.FOB = entityPOCO.FOB;
            entityPM.GENERALTAX = entityPOCO.GENERALTAX;
            entityPM.BUYTAX = entityPOCO.BUYTAX;
            entityPM.VATRESHIMON = entityPOCO.VATRESHIMON;
            entityPM.EXEMPTTYPE = entityPOCO.EXEMPTTYPE;
            entityPM.Tenant = (int)entityPOCO.tenant;
            entityPM.IS_SYNCHRONIZED = (bool)entityPOCO.IS_SYNCHRONIZED;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT;

        }

        public void CustomPMToPOCO(CCUCARSCPM entityPM, CCUCARSC entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUCARSCPM entityPM, CCUCARSC entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUCARSCPM entityPM, CCUCARSCPM oldEntityPM)
        {
          //  throw new NotImplementedException();
        }
    }
}
