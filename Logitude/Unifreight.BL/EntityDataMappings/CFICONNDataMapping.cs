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
    public class CFICONNDataMapping : IMapping<CFICONNPM, CFICONN>
    {
        public void PMToPOCO(CFICONNPM entityPM, CFICONN entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.CUSTOMFILE = entityPM.CUSTOMFILE;
        }

        public void POCOToPM(CFICONNPM entityPM, CFICONN entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.CUSTOMFILE = entityPOCO.CUSTOMFILE;
        }

        public void CustomPMToPOCO(CFICONNPM entityPM, CFICONN entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFICONNPM entityPM, CFICONN entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFICONNPM entityPM, CFICONNPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
