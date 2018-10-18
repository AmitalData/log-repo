using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class GDMFILEVERDataMapping : IMapping<GDMFILEVERPM, GDMFILEVER>
    {

        public void PMToPOCO(GDMFILEVERPM entityPM, GDMFILEVER entityPOCO)
        {

            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.VERSION = entityPM.VERSION;

            entityPOCO.EXTENSION = entityPM.EXTENSION;
            entityPOCO.MD5HASH = entityPM.MD5HASH;
        }

        public void POCOToPM(GDMFILEVERPM entityPM, GDMFILEVER entityPOCO)
        {
            entityPM.COMID = entityPOCO.COMID;
            entityPM.VERSION = entityPOCO.VERSION;

            entityPM.EXTENSION = entityPOCO.EXTENSION;
            entityPM.MD5HASH = entityPOCO.MD5HASH;
        }

        public void CustomPMToPOCO(GDMFILEVERPM entityPM, GDMFILEVER entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GDMFILEVERPM entityPM, GDMFILEVER entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void PMToOldPM(GDMFILEVERPM entityPM, GDMFILEVERPM oldEntityPM)
        {
         ///   throw new NotImplementedException();
        }
    }
}
