
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
    public class CTBAPPROVTYPEDataMapping : IMapping<CTBAPPROVTYPEPM, CTBAPPROVTYPE>
    {
        public void PMToPOCO(CTBAPPROVTYPEPM entityPM, CTBAPPROVTYPE entityPOCO)
        {
            entityPOCO.APPROVTYPEID = entityPM.APPROVTYPEID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
        }

        public void POCOToPM(CTBAPPROVTYPEPM entityPM, CTBAPPROVTYPE entityPOCO)
        {
            entityPM.APPROVTYPEID = entityPOCO.APPROVTYPEID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
        }

        public void CustomPMToPOCO(CTBAPPROVTYPEPM entityPM, CTBAPPROVTYPE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CTBAPPROVTYPEPM entityPM, CTBAPPROVTYPE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CTBAPPROVTYPEPM entityPM, CTBAPPROVTYPEPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}

