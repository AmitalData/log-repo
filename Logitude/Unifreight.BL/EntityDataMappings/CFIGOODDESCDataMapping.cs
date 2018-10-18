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
    public class CFIGOODDESCDataMapping : IMapping<CFIGOODDESCPM, CFIGOODDESC>
    {
        public void PMToPOCO(CFIGOODDESCPM entityPM, CFIGOODDESC entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.GOODDESC = entityPM.GOODDESC;
            entityPOCO.CUSTOMERID = entityPM.CUSTOMERID;
        }

        public void POCOToPM(CFIGOODDESCPM entityPM, CFIGOODDESC entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.GOODDESC = entityPOCO.GOODDESC;
            entityPM.CUSTOMERID = entityPOCO.CUSTOMERID;
        }

        public void CustomPMToPOCO(CFIGOODDESCPM entityPM, CFIGOODDESC entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIGOODDESCPM entityPM, CFIGOODDESC entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIGOODDESCPM entityPM, CFIGOODDESCPM oldEntityPM)
        {
        //    throw new System.NotImplementedException();
        }
    }
}
