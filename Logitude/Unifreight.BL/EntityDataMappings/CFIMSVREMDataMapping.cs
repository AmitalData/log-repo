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
    public class CFIMSVREMDataMapping : IMapping<CFIMSVREMPM, CFIMSVREM>
    {
        public void PMToPOCO(CFIMSVREMPM entityPM, CFIMSVREM entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.PAGENUM = entityPM.PAGENUM;
            entityPOCO.TOP = entityPM.TOP;
            entityPOCO.LEFT = entityPM.LEFT;
            entityPOCO.HEIGHT = entityPM.HEIGHT;
            entityPOCO.WIDTH = entityPM.WIDTH;
            entityPOCO.REMARK = entityPM.REMARK;
        }

        public void POCOToPM(CFIMSVREMPM entityPM, CFIMSVREM entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.COMID = entityPOCO.COMID;
            entityPM.PAGENUM = entityPOCO.PAGENUM;
            entityPM.TOP = entityPOCO.TOP;
            entityPM.LEFT = entityPOCO.LEFT;
            entityPM.HEIGHT = entityPOCO.HEIGHT;
            entityPM.WIDTH = entityPOCO.WIDTH;
            entityPM.REMARK = entityPOCO.REMARK;
        }

        public void CustomPMToPOCO(CFIMSVREMPM entityPM, CFIMSVREM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIMSVREMPM entityPM, CFIMSVREM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIMSVREMPM entityPM, CFIMSVREMPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
