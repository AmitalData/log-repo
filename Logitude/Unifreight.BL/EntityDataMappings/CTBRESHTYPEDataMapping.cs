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
    public class CTBRESHTYPEDataMapping : IMapping<CTBRESHTYPEPM, CTBRESHTYPE>
    {
        public void PMToPOCO(CTBRESHTYPEPM entityPM, CTBRESHTYPE entityPOCO)
        {
            entityPOCO.RESHIMONTYPE = entityPM.RESHIMONTYPE;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBRESHTYPEPM entityPM, CTBRESHTYPE entityPOCO)
        {
            entityPM.RESHIMONTYPE = entityPOCO.RESHIMONTYPE;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBRESHTYPEPM entityPM, CTBRESHTYPE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBRESHTYPEPM entityPM, CTBRESHTYPE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBRESHTYPEPM entityPM, CTBRESHTYPEPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
