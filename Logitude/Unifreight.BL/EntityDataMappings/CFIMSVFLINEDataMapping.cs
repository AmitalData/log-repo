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
    public class CFIMSVFLINEDataMapping : IMapping<CFIMSVFLINEPM, CFIMSVFLINE>
    {
        public void PMToPOCO(CFIMSVFLINEPM entityPM, CFIMSVFLINE entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.LINENUM = entityPM.LINENUM;
            entityPOCO.ITEMNAME = entityPM.ITEMNAME;
            entityPOCO.ITEMVALUE = entityPM.ITEMVALUE;
            entityPOCO.ITEMTYPE = entityPM.ITEMTYPE;
        }

        public void POCOToPM(CFIMSVFLINEPM entityPM, CFIMSVFLINE entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.COMID = entityPOCO.COMID;
            entityPM.LINENUM = entityPOCO.LINENUM;
            entityPM.ITEMNAME = entityPOCO.ITEMNAME;
            entityPM.ITEMVALUE = entityPOCO.ITEMVALUE;
            entityPM.ITEMTYPE = entityPOCO.ITEMTYPE;
        }

        public void CustomPMToPOCO(CFIMSVFLINEPM entityPM, CFIMSVFLINE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIMSVFLINEPM entityPM, CFIMSVFLINE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIMSVFLINEPM entityPM, CFIMSVFLINEPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
