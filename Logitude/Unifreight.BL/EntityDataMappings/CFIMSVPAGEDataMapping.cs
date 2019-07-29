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
    public class CFIMSVPAGEDataMapping : IMapping<CFIMSVPAGEPM, CFIMSVPAGE>
    {
        public void PMToPOCO(CFIMSVPAGEPM entityPM, CFIMSVPAGE entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.PAGENUM = entityPM.PAGENUM;
            entityPOCO.WIDTH = entityPM.WIDTH;
            entityPOCO.HEIGHT = entityPM.HEIGHT;
            entityPOCO.LEFTDATA = entityPM.LEFTDATA;
            entityPOCO.TOPDATA = entityPM.TOPDATA;
            entityPOCO.WIDTHDATA = entityPM.WIDTHDATA;
            entityPOCO.HEIGHTDATA = entityPM.HEIGHTDATA;
            entityPOCO.QUETYPE = entityPM.QUETYPE;
        }

        public void POCOToPM(CFIMSVPAGEPM entityPM, CFIMSVPAGE entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.COMID = entityPOCO.COMID;
            entityPM.PAGENUM = entityPOCO.PAGENUM;
            entityPM.WIDTH = entityPOCO.WIDTH;
            entityPM.HEIGHT = entityPOCO.HEIGHT;
            entityPM.LEFTDATA = entityPOCO.LEFTDATA;
            entityPM.TOPDATA = entityPOCO.TOPDATA;
            entityPM.WIDTHDATA = entityPOCO.WIDTHDATA;
            entityPM.HEIGHTDATA = entityPOCO.HEIGHTDATA;
            entityPM.QUETYPE = entityPOCO.QUETYPE;
        }

        public void CustomPMToPOCO(CFIMSVPAGEPM entityPM, CFIMSVPAGE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIMSVPAGEPM entityPM, CFIMSVPAGE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIMSVPAGEPM entityPM, CFIMSVPAGEPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
