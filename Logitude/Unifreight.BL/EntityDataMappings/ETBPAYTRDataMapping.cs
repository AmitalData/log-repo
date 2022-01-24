
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
    public class ETBPAYTRDataMapping : IMapping<ETBPAYTRPM, ETBPAYTR>
    {
        public void PMToPOCO(ETBPAYTRPM entityPM, ETBPAYTR entityPOCO)
        {
            entityPOCO.PTERMID = entityPM.PTERMID;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.SEARCHENG = entityPM.SEARCHENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.WTVAL = entityPM.WTVAL;
            entityPOCO.OTHER = entityPM.OTHER;
            entityPOCO.SERTYPEDEF = entityPM.SERTYPEDEF;
        }

        public void POCOToPM(ETBPAYTRPM entityPM, ETBPAYTR entityPOCO)
        {
            entityPM.PTERMID = entityPOCO.PTERMID;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.SEARCHENG = entityPOCO.SEARCHENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.WTVAL = entityPOCO.WTVAL;
            entityPM.OTHER = entityPOCO.OTHER;
            entityPM.SERTYPEDEF = entityPOCO.SERTYPEDEF;

        }

        public void CustomPMToPOCO(ETBPAYTRPM entityPM, ETBPAYTR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ETBPAYTRPM entityPM, ETBPAYTR entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ETBPAYTRPM entityPM, ETBPAYTRPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
