
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
    public class CTBSTORAGEDataMapping : IMapping<CTBSTORAGEPM, CTBSTORAGE>
    {
        public void PMToPOCO(CTBSTORAGEPM entityPM, CTBSTORAGE entityPOCO)
        {
            entityPOCO.STORAGESITE = entityPM.STORAGESITE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBSTORAGEPM entityPM, CTBSTORAGE entityPOCO)
        {
            entityPM.STORAGESITE = entityPOCO.STORAGESITE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBSTORAGEPM entityPM, CTBSTORAGE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBSTORAGEPM entityPM, CTBSTORAGE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBSTORAGEPM entityPM, CTBSTORAGEPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
