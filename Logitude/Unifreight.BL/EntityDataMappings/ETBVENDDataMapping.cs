
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
    public class ETBVENDDataMapping : IMapping<ETBVENDPM, ETBVEND>
    {
        public void PMToPOCO(ETBVENDPM entityPM, ETBVEND entityPOCO)
        {
            entityPOCO.VENDORID = entityPM.VENDORID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(ETBVENDPM entityPM, ETBVEND entityPOCO)
        {
            entityPM.VENDORID = entityPOCO.VENDORID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(ETBVENDPM entityPM, ETBVEND entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ETBVENDPM entityPM, ETBVEND entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ETBVENDPM entityPM, ETBVENDPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
