
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
    public class GDMENTITYDataMapping : IMapping<GDMENTITYPM, GDMENTITY>
    {
        public void PMToPOCO(GDMENTITYPM entityPM, GDMENTITY entityPOCO)
        {
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.PRIMARYID = entityPM.PRIMARYID;
            entityPOCO.PRIMARYNUM = entityPM.PRIMARYNUM;
        }

        public void POCOToPM(GDMENTITYPM entityPM, GDMENTITY entityPOCO)
        {
            entityPM.COMID = entityPOCO.COMID;
            entityPM.PRIMARYID = entityPOCO.PRIMARYID;
            entityPM.PRIMARYNUM = entityPOCO.PRIMARYNUM;
        }

        public void CustomPMToPOCO(GDMENTITYPM entityPM, GDMENTITY entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GDMENTITYPM entityPM, GDMENTITY entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GDMENTITYPM entityPM, GDMENTITYPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
