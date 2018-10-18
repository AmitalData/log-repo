
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
    public class CTBCURRENCYDataMapping : IMapping<CTBCURRENCYPM, CTBCURRENCY>
    {
        public void PMToPOCO(CTBCURRENCYPM entityPM, CTBCURRENCY entityPOCO)
        {
            entityPOCO.CURRENCYID = entityPM.CURRENCYID;
            entityPOCO.LOCALCODE = entityPM.LOCALCODE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBCURRENCYPM entityPM, CTBCURRENCY entityPOCO)
        {
            entityPM.CURRENCYID = entityPOCO.CURRENCYID;
            entityPM.LOCALCODE = entityPOCO.LOCALCODE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBCURRENCYPM entityPM, CTBCURRENCY entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBCURRENCYPM entityPM, CTBCURRENCY entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBCURRENCYPM entityPM, CTBCURRENCYPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
