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
    class GAQFILEDATADataMapping : IMapping<GAQFILEDATAPM, GAQFILEDATA>
    {
        public void PMToPOCO(GAQFILEDATAPM entityPM, GAQFILEDATA entityPOCO)
        {
            entityPOCO.ENTNAME = entityPM.ENTNAME;
            entityPOCO.PRIMARYNUM = entityPM.PRIMARYNUM;
            entityPOCO.APPQID = entityPM.APPQID;
            entityPOCO.PATH = entityPM.PATH;
            entityPOCO.FIELDID = entityPM.FIELDID;
            entityPOCO.FIELDVALUE = entityPM.FIELDVALUE;
        }

        public void POCOToPM(GAQFILEDATAPM entityPM, GAQFILEDATA entityPOCO)
        {
            entityPM.ENTNAME = entityPOCO.ENTNAME;
            entityPM.PRIMARYNUM = entityPOCO.PRIMARYNUM;
            entityPM.APPQID =entityPOCO.APPQID;
            entityPM.PATH = entityPOCO.PATH;
            entityPM.FIELDID = entityPOCO.FIELDID;
            entityPM.FIELDVALUE = entityPOCO.FIELDVALUE;
        }

        public void CustomPMToPOCO(GAQFILEDATAPM entityPM, GAQFILEDATA entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GAQFILEDATAPM entityPM, GAQFILEDATA entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GAQFILEDATAPM entityPM, GAQFILEDATAPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
