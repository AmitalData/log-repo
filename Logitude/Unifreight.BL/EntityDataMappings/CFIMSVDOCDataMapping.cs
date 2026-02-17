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
    public class CFIMSVDOCDataMapping : IMapping<CFIMSVDOCPM, CFIMSVDOC>
    {
        public void PMToPOCO(CFIMSVDOCPM entityPM, CFIMSVDOC entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.CREATEDATE = entityPM.CREATEDATE;
            entityPOCO.CREATEBY = entityPM.CREATEBY;
            entityPOCO.UPDATEBY = entityPM.UPDATEBY;
            entityPOCO.STATUS = entityPM.STATUS;
            entityPOCO.REMARK = entityPM.REMARK;
            entityPOCO.TOTALPAGES = entityPM.TOTALPAGES;
            entityPOCO.CUSTOMERID = entityPM.CUSTOMERID;
        }

        public void POCOToPM(CFIMSVDOCPM entityPM, CFIMSVDOC entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.COMID = entityPOCO.COMID;
            entityPM.CREATEDATE = entityPOCO.CREATEDATE;
            entityPM.CREATEBY = entityPOCO.CREATEBY;
            entityPM.UPDATEBY = entityPOCO.UPDATEBY;
            entityPM.STATUS = entityPOCO.STATUS;
            entityPM.REMARK = entityPOCO.REMARK;
            entityPM.TOTALPAGES = entityPOCO.TOTALPAGES;
            entityPM.CUSTOMERID = entityPOCO.CUSTOMERID;
        }

        public void CustomPMToPOCO(CFIMSVDOCPM entityPM, CFIMSVDOC entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIMSVDOCPM entityPM, CFIMSVDOC entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIMSVDOCPM entityPM, CFIMSVDOCPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
