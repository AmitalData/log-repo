
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
    public class ATBPTILDataMapping : IMapping<ATBPTILPM, ATBPTIL>
    {
        public void PMToPOCO(ATBPTILPM entityPM, ATBPTIL entityPOCO)
        {
            entityPOCO.BRANID = entityPM.BRANID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(ATBPTILPM entityPM, ATBPTIL entityPOCO)
        {
            entityPM.BRANID = entityPOCO.BRANID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(ATBPTILPM entityPM, ATBPTIL entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ATBPTILPM entityPM, ATBPTIL entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ATBPTILPM entityPM, ATBPTILPM oldEntityPM)
        {
        //    throw new System.NotImplementedException();
        }
    }
}
