
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
    public class GDMREFDataMapping : IMapping<GDMREFPM, GDMREF>
    {
        public void PMToPOCO(GDMREFPM entityPM, GDMREF entityPOCO)
        {
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.REFID = entityPM.REFID;
            entityPOCO.REFERENCE = entityPM.REFERENCE;
            entityPOCO.METADATA = entityPM.METADATA;
            entityPOCO.MDNEW = entityPM.MDNEW;
        }

        public void POCOToPM(GDMREFPM entityPM, GDMREF entityPOCO)
        {
            entityPM.COMID = entityPOCO.COMID;
            entityPM.REFID = entityPOCO.REFID;
            entityPM.REFERENCE = entityPOCO.REFERENCE;
            entityPM.METADATA = entityPOCO.METADATA;
            entityPM.MDNEW = entityPOCO.MDNEW;
        }

        public void CustomPMToPOCO(GDMREFPM entityPM, GDMREF entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GDMREFPM entityPM, GDMREF entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GDMREFPM entityPM, GDMREFPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
