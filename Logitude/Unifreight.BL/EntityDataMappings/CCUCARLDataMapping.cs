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
    public class CCUCARLDataMapping : IMapping<CCUCARLPM, CCUCARL>
    {
        public void PMToPOCO(CCUCARLPM entityPM, CCUCARL entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.COUNTER = entityPM.COUNTER;
            entityPOCO.RIHBIT = entityPM.RIHBIT;
            entityPOCO.ENGINEVOL = entityPM.ENGINEVOL;
            entityPOCO.SHEILDNO = entityPM.SHEILDNO;
            entityPOCO.MNFDATE = entityPM.MNFDATE;
            entityPOCO.A = entityPM.A;
            entityPOCO.B = entityPM.B;
            entityPOCO.E = entityPM.E;
            entityPOCO.WEIGHT = entityPM.WEIGHT;
            entityPOCO.MEMIRTYPE = entityPM.MEMIRTYPE;
            entityPOCO.MADADRATE = entityPM.MADADRATE;
            entityPOCO.FUELTYPE = entityPM.FUELTYPE;
            entityPOCO.FFU1 = entityPM.FFU1;
            entityPOCO.FFU2 = entityPM.FFU2;
            entityPOCO.ABSDEDUCT = entityPM.ABSDEDUCT;
            entityPOCO.KARITDEDUCT = entityPM.KARITDEDUCT;
            entityPOCO.BAKARADEDUCT = entityPM.BAKARADEDUCT;
            entityPOCO.MEMIRDEDUCT = entityPM.MEMIRDEDUCT;
            entityPOCO.MADADDEDUCT = entityPM.MADADDEDUCT;
            entityPOCO.HYBRID = entityPM.HYBRID;
            entityPOCO.TENANT = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUCARLPM entityPM, CCUCARL entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.COUNTER = entityPOCO.COUNTER;
            entityPM.RIHBIT = entityPOCO.RIHBIT;
            entityPM.ENGINEVOL = entityPOCO.ENGINEVOL;
            entityPM.SHEILDNO = entityPOCO.SHEILDNO;
            entityPM.MNFDATE = entityPOCO.MNFDATE;
            entityPM.A = entityPOCO.A;
            entityPM.B = entityPOCO.B;
            entityPM.E = entityPOCO.E;
            entityPM.WEIGHT = entityPOCO.WEIGHT;
            entityPM.MEMIRTYPE = entityPOCO.MEMIRTYPE;
            entityPM.MADADRATE = entityPOCO.MADADRATE;
            entityPM.FUELTYPE = entityPOCO.FUELTYPE;
            entityPM.FFU1 = entityPOCO.FFU1;
            entityPM.FFU2 = entityPOCO.FFU2;
            entityPM.ABSDEDUCT = entityPOCO.ABSDEDUCT;
            entityPM.KARITDEDUCT = entityPOCO.KARITDEDUCT;
            entityPM.BAKARADEDUCT = entityPOCO.BAKARADEDUCT;
            entityPM.MEMIRDEDUCT = entityPOCO.MEMIRDEDUCT;
            entityPM.MADADDEDUCT = entityPOCO.MADADDEDUCT;
            entityPM.HYBRID = entityPOCO.HYBRID;
            entityPM.Tenant = entityPOCO.TENANT != null ? (int)entityPOCO.TENANT : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;
        }

        public void CustomPMToPOCO(CCUCARLPM entityPM, CCUCARL entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUCARLPM entityPM, CCUCARL entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUCARLPM entityPM, CCUCARLPM oldEntityPM)
        {
          //  throw new NotImplementedException();
        }
    }
}
