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
    public class GTRTRANDataMapping : IMapping<GTRTRANPM, GTRTRAN>
    {
        public void PMToPOCO(GTRTRANPM entityPM, GTRTRAN entityPOCO)
        {
            entityPOCO.PARTNERID = entityPM.PARTNERID;
            entityPOCO.TABLEID = entityPM.TABLEID;
            entityPOCO.PARTNERCODE = entityPM.PARTNERCODE;
            entityPOCO.LOCALCODE = entityPM.LOCALCODE;
        }

        public void POCOToPM(GTRTRANPM entityPM, GTRTRAN entityPOCO)
        {
            entityPM.PARTNERID = entityPOCO.PARTNERID;
            entityPM.TABLEID = entityPOCO.TABLEID;
            entityPM.PARTNERCODE = entityPOCO.PARTNERCODE;
            entityPM.LOCALCODE = entityPOCO.LOCALCODE;
        }

        public void CustomPMToPOCO(GTRTRANPM entityPM, GTRTRAN entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTRTRANPM entityPM, GTRTRAN entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTRTRANPM entityPM, GTRTRANPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
