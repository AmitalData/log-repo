using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class GITITEMCRDataMapping : IMapping<GITITEMCRPM, GITITEMCR>
    {

        public enum POCOPropertyNames
        {
            None,

        }
        public enum PMPropertyNames
        {
            None,
        }

        public void PMToPOCO(GITITEMCRPM entityPM, GITITEMCR entityPOCO)
        {
            entityPOCO.COUNTER = entityPM.COUNTER;
            entityPOCO.REQCERT = entityPM.REQCERT;
            entityPOCO.REMARKS = entityPM.REMARKS;
        }

        public void POCOToPM(GITITEMCRPM entityPM, GITITEMCR entityPOCO)
        {
            entityPM.COUNTER = entityPOCO.COUNTER;
            entityPM.REQCERT = entityPOCO.REQCERT;
            entityPM.REMARKS = entityPOCO.REMARKS;
        }

        public void CustomPMToPOCO(GITITEMCRPM entityPM, GITITEMCR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GITITEMCRPM entityPM, GITITEMCR entityPOCO)
        {
            //throw new NotImplementedException();
        }


        public void PMToOldPM(GITITEMCRPM entityPM, GITITEMCRPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
