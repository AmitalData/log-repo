using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    class GNDADRDataMapping : IMapping<GNDADRPM, GNDADR>
    {
        public void PMToPOCO(GNDADRPM entityPM, GNDADR entityPOCO)
        {
            entityPOCO.CARDID = entityPM.CARDID;
            entityPOCO.LINE = entityPM.LINE;
            entityPOCO.TELEPHONE = entityPM.TELEPHONE;
            entityPOCO.FAX = entityPM.FAX;       
        }

        public void POCOToPM(GNDADRPM entityPM, GNDADR entityPOCO)
        {
            entityPM.CARDID = entityPOCO.CARDID;
            entityPM.LINE = entityPOCO.LINE;
            entityPM.TELEPHONE = entityPOCO.TELEPHONE;
            entityPM.FAX = entityPOCO.FAX;
        }

        public void CustomPMToPOCO(GNDADRPM entityPM, GNDADR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GNDADRPM entityPM, GNDADR entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GNDADRPM entityPM, GNDADRPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
