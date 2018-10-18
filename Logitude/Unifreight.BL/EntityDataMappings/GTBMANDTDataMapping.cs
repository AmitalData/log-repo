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
    public class GTBMANDTDataMapping : IMapping<GTBMANDTPM, GTBMANDT>
    {
        public void PMToPOCO(GTBMANDTPM entityPM, GTBMANDT entityPOCO)
        {
            entityPOCO.CLIENTCODE = entityPM.CLIENTCODE;
            entityPOCO.ENTITY = entityPM.ENTITY;
            entityPOCO.FORMNAME = entityPM.FORMNAME;
            entityPOCO.FIELDNAME = entityPM.FIELDNAME;
            entityPOCO.ATTR = entityPM.ATTR;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(GTBMANDTPM entityPM, GTBMANDT entityPOCO)
        {
            entityPM.CLIENTCODE = entityPOCO.CLIENTCODE;
            entityPM.ENTITY = entityPOCO.ENTITY;
            entityPM.FORMNAME = entityPOCO.FORMNAME;
            entityPM.FIELDNAME = entityPOCO.FIELDNAME;
            entityPM.ATTR = entityPOCO.ATTR;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(GTBMANDTPM entityPM, GTBMANDT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTBMANDTPM entityPM, GTBMANDT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTBMANDTPM entityPM, GTBMANDTPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
