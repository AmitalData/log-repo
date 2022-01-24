
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
    public class ETBAIRLINEDataMapping : IMapping<ETBAIRLINEPM, ETBAIRLINE>
    {
        public void PMToPOCO(ETBAIRLINEPM entityPM, ETBAIRLINE entityPOCO)
        {
            entityPOCO.AIRLINEID = entityPM.AIRLINEID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(ETBAIRLINEPM entityPM, ETBAIRLINE entityPOCO)
        {
            entityPM.AIRLINEID = entityPOCO.AIRLINEID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(ETBAIRLINEPM entityPM, ETBAIRLINE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ETBAIRLINEPM entityPM, ETBAIRLINE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ETBAIRLINEPM entityPM, ETBAIRLINEPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
