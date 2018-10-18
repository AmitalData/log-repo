using System;
using System.Collections.Generic;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;

using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.EntityPMs;

namespace Unifreight.BL.EntityDataMappings
{
    public class GTBITMCNDataMapping : IMapping<GTBITMCNPM, GTBITMCN>
    {
        public void PMToPOCO(GTBITMCNPM entityPM, GTBITMCN entityPOCO)
        {
            entityPOCO.PARTNERID = entityPM.PARTNERID;
            entityPOCO.ITEMID = entityPM.ITEMID;
            entityPOCO.PARTNER2ID = entityPM.PARTNER2ID;
            entityPOCO.ITEM2ID = entityPM.ITEM2ID;
        }

        public void POCOToPM(GTBITMCNPM entityPM, GTBITMCN entityPOCO)
        {
            entityPM.PARTNERID = entityPOCO.PARTNERID;
            entityPM.ITEMID = entityPOCO.ITEMID;
            entityPM.PARTNER2ID = entityPOCO.PARTNER2ID;
            entityPM.ITEM2ID = entityPOCO.ITEM2ID;
        }

        public void CustomPMToPOCO(GTBITMCNPM entityPM, GTBITMCN entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTBITMCNPM entityPM, GTBITMCN entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTBITMCNPM entityPM, GTBITMCNPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}

