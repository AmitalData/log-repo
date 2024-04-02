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
    public class CCUMESSAGEDataMapping : IMapping<CCUMESSAGEPM, CCUMESSAGE>
    {
        public void PMToPOCO(CCUMESSAGEPM entityPM, CCUMESSAGE entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.GROUPNO = entityPM.GROUPNO;
            entityPOCO.GROUPKEY = entityPM.GROUPKEY;
            entityPOCO.REFERENCE = entityPM.REFERENCE;
            entityPOCO.MESSAGENO = entityPM.MESSAGENO;
            entityPOCO.APPROVCODEID = entityPM.APPROVCODEID;
            entityPOCO.APPROVTYPEID = entityPM.APPROVTYPEID;
            entityPOCO.APPROVNO = entityPM.APPROVNO;
            entityPOCO.ADDITIONID = entityPM.ADDITIONID;
            entityPOCO.GENERAL = entityPM.GENERAL;
            entityPOCO.APPROVELEVEL = entityPM.APPROVELEVEL;
            entityPOCO.MESSAGETXT = entityPM.MESSAGETXT;
            entityPOCO.TENANT = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUMESSAGEPM entityPM, CCUMESSAGE entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.GROUPNO = entityPOCO.GROUPNO;
            entityPM.GROUPKEY = entityPOCO.GROUPKEY;
            entityPM.REFERENCE = entityPOCO.REFERENCE;
            entityPM.MESSAGENO = entityPOCO.MESSAGENO;
            entityPM.APPROVCODEID = entityPOCO.APPROVCODEID;
            entityPM.APPROVTYPEID = entityPOCO.APPROVTYPEID;
            entityPM.APPROVNO = entityPOCO.APPROVNO;
            entityPM.ADDITIONID = entityPOCO.ADDITIONID;
            entityPM.GENERAL = entityPOCO.GENERAL;
            entityPM.APPROVELEVEL = entityPOCO.APPROVELEVEL;
            entityPM.MESSAGETXT = entityPOCO.MESSAGETXT;
            entityPM.Tenant = entityPOCO.TENANT != null ? (int)entityPOCO.TENANT : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;
        }

        public void CustomPMToPOCO(CCUMESSAGEPM entityPM, CCUMESSAGE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUMESSAGEPM entityPM, CCUMESSAGE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUMESSAGEPM entityPM, CCUMESSAGEPM oldEntityPM)
        {
        //    throw new NotImplementedException();
        }
    }
}