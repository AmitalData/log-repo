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
    public class CTBIMPORTDataMapping : IMapping<CTBIMPORTPM, CTBIMPORT>
    {
        public void PMToPOCO(CTBIMPORTPM entityPM, CTBIMPORT entityPOCO)
        {
            entityPOCO.IMPORTERID = entityPM.IMPORTERID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBIMPORTPM entityPM, CTBIMPORT entityPOCO)
        {
            entityPM.IMPORTERID = entityPOCO.IMPORTERID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBIMPORTPM entityPM, CTBIMPORT entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBIMPORTPM entityPM, CTBIMPORT entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBIMPORTPM entityPM, CTBIMPORTPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
