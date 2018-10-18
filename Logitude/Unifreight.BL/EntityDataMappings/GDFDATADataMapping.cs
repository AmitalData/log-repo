using System;
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
    public class GDFDATADataMapping : IMapping<GDFDATAPM, GDFDATA>
    {
        public void PMToPOCO(GDFDATAPM entityPM, GDFDATA entityPOCO)
        {
            entityPOCO.DISTRID = entityPM.DISTRID;
            entityPOCO.DEFID = entityPM.DEFID;
            entityPOCO.BRANCHID = entityPM.BRANCHID;
            entityPOCO.CARDID = entityPM.CARDID;
            entityPOCO.SHORTDEFDATA = entityPM.SHORTDEFDATA;
            entityPOCO.DEFDATA = entityPM.DEFDATA;
        }

        public void POCOToPM(GDFDATAPM entityPM, GDFDATA entityPOCO)
        {
            entityPM.DISTRID = entityPOCO.DISTRID;
            entityPM.DEFID = entityPOCO.DEFID;
            entityPM.BRANCHID = entityPOCO.BRANCHID;
            entityPM.CARDID = entityPOCO.CARDID;
            entityPM.SHORTDEFDATA = entityPOCO.SHORTDEFDATA;
            entityPM.DEFDATA = entityPOCO.DEFDATA;
        }

        public void CustomPMToPOCO(GDFDATAPM entityPM, GDFDATA entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GDFDATAPM entityPM, GDFDATA entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GDFDATAPM entityPM, GDFDATAPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
