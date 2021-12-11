
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
    public class ETBSERLVDataMapping : IMapping<ETBSERLVPM, ETBSERLV>
    {
        public void PMToPOCO(ETBSERLVPM entityPM, ETBSERLV entityPOCO)
        {
            entityPOCO.SERVLEVELID = entityPM.SERVLEVELID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(ETBSERLVPM entityPM, ETBSERLV entityPOCO)
        {
            entityPM.SERVLEVELID = entityPOCO.SERVLEVELID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(ETBSERLVPM entityPM, ETBSERLV entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ETBSERLVPM entityPM, ETBSERLV entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ETBSERLVPM entityPM, ETBSERLVPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
