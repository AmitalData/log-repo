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
    public class YTBTABLEDataMapping : IMapping<YTBTABLEPM, YTBTABLE>
    {
        public void PMToPOCO(YTBTABLEPM entityPM, YTBTABLE entityPOCO)
        {
            entityPOCO.CUSTTB = entityPM.CUSTTB;
            entityPOCO.TBCODE = entityPM.TBCODE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.SEARCHENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.IIGUPDTDATE = entityPM.IIGUPDTDATE;
            entityPOCO.TBCODENUM = entityPM.TBCODENUM;
        }

        public void POCOToPM(YTBTABLEPM entityPM, YTBTABLE entityPOCO)
        {
            entityPM.CUSTTB = entityPOCO.CUSTTB;
            entityPM.TBCODE = entityPOCO.TBCODE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.SEARCHENG = entityPOCO.SEARCHENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.IIGUPDTDATE = entityPOCO.IIGUPDTDATE;
            entityPM.TBCODENUM = entityPOCO.TBCODENUM;
        }

        public void CustomPMToPOCO(YTBTABLEPM entityPM, YTBTABLE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(YTBTABLEPM entityPM, YTBTABLE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(YTBTABLEPM entityPM, YTBTABLEPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
