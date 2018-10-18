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
    public class CTBCOUNTRYDataMapping : IMapping<CTBCOUNTRYPM, CTBCOUNTRY>
    {
        public void PMToPOCO(CTBCOUNTRYPM entityPM, CTBCOUNTRY entityPOCO)
        {
            entityPOCO.COUNTRYID = entityPM.COUNTRYID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBCOUNTRYPM entityPM, CTBCOUNTRY entityPOCO)
        {
            entityPM.COUNTRYID = entityPOCO.COUNTRYID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBCOUNTRYPM entityPM, CTBCOUNTRY entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBCOUNTRYPM entityPM, CTBCOUNTRY entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBCOUNTRYPM entityPM, CTBCOUNTRYPM oldEntityPM)
        {
          //  throw new System.NotImplementedException();
        }
    }
}
