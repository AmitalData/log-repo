
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
    public class GTBSERLVDataMapping : IMapping<GTBSERLVPM, GTBSERLV>
    {
        public void PMToPOCO(GTBSERLVPM entityPM, GTBSERLV entityPOCO)
        {
            entityPOCO.SERVLEVELID = entityPM.SERVLEVELID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(GTBSERLVPM entityPM, GTBSERLV entityPOCO)
        {
            entityPM.SERVLEVELID = entityPOCO.SERVLEVELID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(GTBSERLVPM entityPM, GTBSERLV entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTBSERLVPM entityPM, GTBSERLV entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTBSERLVPM entityPM, GTBSERLVPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
