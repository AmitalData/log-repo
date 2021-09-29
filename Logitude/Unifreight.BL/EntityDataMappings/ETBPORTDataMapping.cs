
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
    public class ETBPORTDataMapping : IMapping<ETBPORTPM, ETBPORT>
    {
        public void PMToPOCO(ETBPORTPM entityPM, ETBPORT entityPOCO)
        {
            entityPOCO.PORTID = entityPM.PORTID;
            entityPOCO.SEARCHENG = entityPM.SEARCHENG;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.COUNTRYID = entityPM.COUNTRYID;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.COLLECTDEBIT = entityPM.COLLECTDEBIT;
            entityPOCO.CONSALLOWED = entityPM.CONSALLOWED;
            entityPOCO.AGENTID = entityPM.AGENTID;
            entityPOCO.TIMEZONE = entityPM.TIMEZONE;
        }

        public void POCOToPM(ETBPORTPM entityPM, ETBPORT entityPOCO)
        {
            entityPM.PORTID = entityPOCO.PORTID;
            entityPM.PORTID = entityPOCO.PORTID;
            entityPM.SEARCHENG = entityPOCO.SEARCHENG;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.COUNTRYID = entityPOCO.COUNTRYID;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.COLLECTDEBIT = entityPOCO.COLLECTDEBIT;
            entityPM.CONSALLOWED = entityPOCO.CONSALLOWED;
            entityPM.AGENTID = entityPOCO.AGENTID;
            entityPM.TIMEZONE = entityPOCO.TIMEZONE;
        }

        public void CustomPMToPOCO(ETBPORTPM entityPM, ETBPORT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ETBPORTPM entityPM, ETBPORT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ETBPORTPM entityPM, ETBPORTPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
