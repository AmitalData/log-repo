
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
    public class MTBCARRDataMapping : IMapping<MTBCARRPM, MTBCARR>
    {
        public void PMToPOCO(MTBCARRPM entityPM, MTBCARR entityPOCO)
        {
            entityPOCO.AIRLINEID = entityPM.AIRLINEID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(MTBCARRPM entityPM, MTBCARR entityPOCO)
        {
            entityPM.AIRLINEID = entityPOCO.AIRLINEID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(MTBCARRPM entityPM, MTBCARR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(MTBCARRPM entityPM, MTBCARR entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(MTBCARRPM entityPM, MTBCARRPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
