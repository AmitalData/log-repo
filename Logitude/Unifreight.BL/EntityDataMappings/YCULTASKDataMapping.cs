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
    class YCULTASKDataMapping : IMapping<YCULTASKPM, YCULTASK>
    {
        public void PMToPOCO(YCULTASKPM entityPM, YCULTASK entityPOCO)
        {
            entityPOCO.TASKID = entityPM.TASKID;
            entityPOCO.ENTNAME = entityPM.ENTNAME;
            entityPOCO.PRIMARYNUM = entityPM.PRIMARYNUM;
            entityPOCO.TYPE = entityPM.TYPE ;
            entityPOCO.LOGTIME = entityPM.LOGTIME;
            entityPOCO.PRIORITY = entityPM.PRIORITY;
            entityPOCO.PROCESSSTARTTIME = entityPM.PROCESSSTARTTIME;
            entityPOCO.PROCESSENDTIME = entityPM.PROCESSENDTIME;
            entityPOCO.ARCHIVE = entityPM.ARCHIVE;
            entityPOCO.STATUS = entityPM.STATUS;
            entityPOCO.REQUESTDATA = entityPM.REQUESTDATA;
            entityPOCO.RESPONSE = entityPM.RESPONSE;
            entityPOCO.USRCODE = entityPM.USRCODE;
            entityPOCO.CLIENTID = entityPM.CLIENTID;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCHRONIZED = entityPM.IS_SYNCHRONIZED;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(YCULTASKPM entityPM, YCULTASK entityPOCO)
        {
            entityPM.TASKID = entityPOCO.TASKID;
            entityPM.ENTNAME = entityPOCO.ENTNAME;
            entityPM.PRIMARYNUM = entityPOCO.PRIMARYNUM;
            entityPM.TYPE = entityPOCO.TYPE;
            entityPM.LOGTIME = entityPOCO.LOGTIME;
            entityPM.PRIORITY = entityPOCO.PRIORITY;
            entityPM.PROCESSSTARTTIME = entityPOCO.PROCESSSTARTTIME;
            entityPM.PROCESSENDTIME = entityPOCO.PROCESSENDTIME;
            entityPM.ARCHIVE = entityPOCO.ARCHIVE;
            entityPM.STATUS = entityPOCO.STATUS;
            entityPM.REQUESTDATA = entityPOCO.REQUESTDATA;
            entityPM.RESPONSE = entityPOCO.RESPONSE;
            entityPM.USRCODE = entityPOCO.USRCODE;
            entityPM.CLIENTID = entityPOCO.CLIENTID;
            entityPM.Tenant = (int)entityPOCO.tenant;
            entityPM.IS_SYNCHRONIZED = (bool)entityPOCO.IS_SYNCHRONIZED;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT;

        }

        public void CustomPMToPOCO(YCULTASKPM entityPM, YCULTASK entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(YCULTASKPM entityPM, YCULTASK entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(YCULTASKPM entityPM, YCULTASKPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
