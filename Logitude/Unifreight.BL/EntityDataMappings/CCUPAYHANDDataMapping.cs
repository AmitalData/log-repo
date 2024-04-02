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
    public class CCUPAYHANDDataMapping : IMapping<CCUPAYHANDPM, CCUPAYHAND>
    {
        public void PMToPOCO(CCUPAYHANDPM entityPM, CCUPAYHAND entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.AGENTEXPLAIN = entityPM.AGENTEXPLAIN;
            entityPOCO.BONDEDNAME = entityPM.BONDEDNAME;
            entityPOCO.DATE7 = entityPM.DATE7;
            entityPOCO.DRAFTSTATUS = entityPM.DRAFTSTATUS;
            entityPOCO.ENTRYID = entityPM.ENTRYID;
            entityPOCO.HANDDATE = entityPM.HANDDATE;
            entityPOCO.HANDTYPE = entityPM.HANDTYPE;
            entityPOCO.IMPORTERNAME = entityPM.IMPORTERNAME;
            entityPOCO.OBJECTIONEXPLAIN = entityPM.OBJECTIONEXPLAIN;
            entityPOCO.PAYTAX = entityPM.PAYTAX;
            entityPOCO.PROCESSWANT = entityPM.PROCESSWANT;
            entityPOCO.REJECTTAX = entityPM.REJECTTAX;
            entityPOCO.REQUESTCODE = entityPM.REQUESTCODE;
            entityPOCO.RESHIMONSIGN = entityPM.RESHIMONSIGN;
            entityPOCO.RESHIMONSIGNTYPE = entityPM.RESHIMONSIGNTYPE;
            entityPOCO.SIGNERID = entityPM.SIGNERID;
            entityPOCO.TIME7 = entityPM.TIME7;
            entityPOCO.TOTALPAYDEPOSIT = entityPM.TOTALPAYDEPOSIT;
            entityPOCO.TOTALPAYTAX = entityPM.TOTALPAYTAX;
            entityPOCO.TRANSIMPORTERNAME = entityPM.TRANSIMPORTERNAME;
            entityPOCO.TENANT = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUPAYHANDPM entityPM, CCUPAYHAND entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.AGENTEXPLAIN = entityPOCO.AGENTEXPLAIN;
            entityPM.BONDEDNAME = entityPOCO.BONDEDNAME;
            entityPM.DATE7 = entityPOCO.DATE7;
            entityPM.DRAFTSTATUS = entityPOCO.DRAFTSTATUS;
            entityPM.ENTRYID = entityPOCO.ENTRYID;
            entityPM.HANDDATE = entityPOCO.HANDDATE;
            entityPM.HANDTYPE = entityPOCO.HANDTYPE;
            entityPM.IMPORTERNAME = entityPOCO.IMPORTERNAME;
            entityPM.OBJECTIONEXPLAIN = entityPOCO.OBJECTIONEXPLAIN;
            entityPM.PAYTAX = entityPOCO.PAYTAX;
            entityPM.PROCESSWANT = entityPOCO.PROCESSWANT;
            entityPM.REJECTTAX = entityPOCO.REJECTTAX;
            entityPM.REQUESTCODE = entityPOCO.REQUESTCODE;
            entityPM.RESHIMONSIGN = entityPOCO.RESHIMONSIGN;
            entityPM.RESHIMONSIGNTYPE = entityPOCO.RESHIMONSIGNTYPE;
            entityPM.SIGNERID = entityPOCO.SIGNERID;
            entityPM.TIME7 = entityPOCO.TIME7;
            entityPM.TOTALPAYDEPOSIT = entityPOCO.TOTALPAYDEPOSIT;
            entityPM.TOTALPAYTAX = entityPOCO.TOTALPAYTAX;
            entityPM.TRANSIMPORTERNAME = entityPOCO.TRANSIMPORTERNAME;
            entityPM.Tenant = entityPOCO.TENANT != null ? (int)entityPOCO.TENANT : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;

        }

        public void CustomPMToPOCO(CCUPAYHANDPM entityPM, CCUPAYHAND entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUPAYHANDPM entityPM, CCUPAYHAND entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUPAYHANDPM entityPM, CCUPAYHANDPM oldEntityPM)
        {
        //    throw new NotImplementedException();
        }
    }
}
