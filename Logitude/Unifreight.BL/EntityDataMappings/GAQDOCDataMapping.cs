
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
    public class GAQDOCDataMapping : IMapping<GAQDOCPM, GAQDOC>
    {
        public void PMToPOCO(GAQDOCPM entityPM, GAQDOC entityPOCO)
        {
            entityPOCO.APPQID = entityPM.APPQID;
            entityPOCO.FOLDERCODE = entityPM.FOLDERCODE;
            entityPOCO.DOCID = entityPM.DOCID;
        }

        public void POCOToPM(GAQDOCPM entityPM, GAQDOC entityPOCO)
        {
            entityPM.APPQID = entityPOCO.APPQID;
            entityPM.FOLDERCODE = entityPOCO.FOLDERCODE;
            entityPM.DOCID = entityPOCO.DOCID;
        }

        public void CustomPMToPOCO(GAQDOCPM entityPM, GAQDOC entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GAQDOCPM entityPM, GAQDOC entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GAQDOCPM entityPM, GAQDOCPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
