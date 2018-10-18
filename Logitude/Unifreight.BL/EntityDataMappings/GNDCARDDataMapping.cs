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
    public class GNDCARDDataMapping : IMapping<GNDCARDPM, GNDCARD>
    {
        public void PMToPOCO(GNDCARDPM entityPM, GNDCARD entityPOCO)
        {
            entityPOCO.CARDID = entityPM.CARDID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.OLDCARD = entityPM.OLDCARD;
            entityPOCO.COMPANYID = entityPM.COMPANYID;
        }
        public void POCOToPM(GNDCARDPM entityPM, GNDCARD entityPOCO)
        {
            entityPM.CARDID = entityPOCO.CARDID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.OLDCARD = entityPOCO.OLDCARD;
            entityPM.COMPANYID = entityPOCO.COMPANYID;
        }

        public void CustomPMToPOCO(GNDCARDPM entityPM, GNDCARD entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GNDCARDPM entityPM, GNDCARD entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GNDCARDPM entityPM, GNDCARDPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}

