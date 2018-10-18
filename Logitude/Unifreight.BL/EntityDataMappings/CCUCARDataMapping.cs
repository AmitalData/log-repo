
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
    public class CCUCARDataMapping : IMapping<CCUCARPM, CCUCAR>
    {
        public void PMToPOCO(CCUCARPM entityPM, CCUCAR entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            
            entityPOCO.ABSDEDUCT = entityPM.ABSDEDUCT;
            entityPOCO.KARITDEDUCT = entityPM.KARITDEDUCT;
            entityPOCO.BAKARADEDUCT = entityPM.BAKARADEDUCT;
            entityPOCO.MEMIRDEDUCT = entityPM.MEMIRDEDUCT;
            entityPOCO.MADADDEDUCT = entityPM.MADADDEDUCT;
            
        }

        public void POCOToPM(CCUCARPM entityPM, CCUCAR entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            
            entityPM.ABSDEDUCT = entityPOCO.ABSDEDUCT;
            entityPM.KARITDEDUCT = entityPOCO.KARITDEDUCT;
            entityPM.BAKARADEDUCT = entityPOCO.BAKARADEDUCT;
            entityPM.MEMIRDEDUCT = entityPOCO.MEMIRDEDUCT;
            entityPM.MADADDEDUCT = entityPOCO.MADADDEDUCT;
            
        }

        public void CustomPMToPOCO(CCUCARPM entityPM, CCUCAR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUCARPM entityPM, CCUCAR entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUCARPM entityPM, CCUCARPM oldEntityPM)
        {
          //  throw new NotImplementedException();
        }
    }
}
