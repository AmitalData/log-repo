
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
    public class CTBCARMODDataMapping : IMapping<CTBCARMODPM, CTBCARMOD>
    {
        public void PMToPOCO(CTBCARMODPM entityPM, CTBCARMOD entityPOCO)
        {
            entityPOCO.CUSTOMERID = entityPM.CUSTOMERID;
            entityPOCO.CARMODEL = entityPM.CARMODEL;
            entityPOCO.NAME = entityPM.NAME;
            entityPOCO.PRAT = entityPM.PRAT;
        }

        public void POCOToPM(CTBCARMODPM entityPM, CTBCARMOD entityPOCO)
        {
            entityPM.CUSTOMERID = entityPOCO.CUSTOMERID;
            entityPM.CARMODEL = entityPOCO.CARMODEL;
            entityPM.NAME = entityPOCO.NAME;
            entityPM.PRAT = entityPOCO.PRAT;
        }

        public void CustomPMToPOCO(CTBCARMODPM entityPM, CTBCARMOD entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBCARMODPM entityPM, CTBCARMOD entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBCARMODPM entityPM, CTBCARMODPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
