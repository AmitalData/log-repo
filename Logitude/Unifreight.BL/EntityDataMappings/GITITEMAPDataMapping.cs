
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
    public class GITITEMAPDataMapping : IMapping<GITITEMAPPM, GITITEMAP>
    {
        public void PMToPOCO(GITITEMAPPM entityPM, GITITEMAP entityPOCO)
        {
            entityPOCO.COUNTER = entityPM.COUNTER;
            entityPOCO.APPROVTYPEID = entityPM.APPROVTYPEID;
            entityPOCO.REMARKS = entityPM.REMARKS;
        }

        public void POCOToPM(GITITEMAPPM entityPM, GITITEMAP entityPOCO)
        {
            entityPM.COUNTER = entityPOCO.COUNTER;
            entityPM.APPROVTYPEID = entityPOCO.APPROVTYPEID;
            entityPM.REMARKS = entityPM.REMARKS;
     
        }

        public void CustomPMToPOCO(GITITEMAPPM entityPM, GITITEMAP entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GITITEMAPPM entityPM, GITITEMAP entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GITITEMAPPM entityPM, GITITEMAPPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
