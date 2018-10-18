
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
    public class GTBREQCERTDataMapping : IMapping<GTBREQCERTPM, GTBREQCERT>
    {
        public void PMToPOCO(GTBREQCERTPM entityPM, GTBREQCERT entityPOCO)
        {
            entityPOCO.REQCERT = entityPM.REQCERT;
            entityPOCO.ENTITY = entityPM.ENTITY;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(GTBREQCERTPM entityPM, GTBREQCERT entityPOCO)
        {
            entityPM.REQCERT = entityPOCO.REQCERT;
            entityPM.ENTITY = entityPOCO.ENTITY;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(GTBREQCERTPM entityPM, GTBREQCERT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTBREQCERTPM entityPM, GTBREQCERT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTBREQCERTPM entityPM, GTBREQCERTPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
