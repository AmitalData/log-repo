
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
    public class CTBAPPROVDataMapping : IMapping<CTBAPPROVPM, CTBAPPROV>
    {
        public void PMToPOCO(CTBAPPROVPM entityPM, CTBAPPROV entityPOCO)
        {
            entityPOCO.APPROVCODEID = entityPM.APPROVCODEID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBAPPROVPM entityPM, CTBAPPROV entityPOCO)
        {
            entityPM.APPROVCODEID = entityPOCO.APPROVCODEID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBAPPROVPM entityPM, CTBAPPROV entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBAPPROVPM entityPM, CTBAPPROV entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBAPPROVPM entityPM, CTBAPPROVPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
