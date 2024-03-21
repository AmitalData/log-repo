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
    public class CCUPAYLINEFDataMapping : IMapping<CCUPAYLINEFPM, CCUPAYLINEF>
    {
        public void PMToPOCO(CCUPAYLINEFPM entityPM, CCUPAYLINEF entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.ACCOUNTNAME = entityPM.ACCOUNTNAME;
            entityPOCO.BANKACCOUNT = entityPM.BANKACCOUNT;
            entityPOCO.BANKBRANCH = entityPM.BANKBRANCH;
            entityPOCO.BANKID = entityPM.BANKID;
            entityPOCO.HASHAVUTCODE = entityPM.HASHAVUTCODE;
            entityPOCO.PAYAMOUNT = entityPM.PAYAMOUNT;//.ToNullableDouble("entityPM.PAYAMOUNT");
            entityPOCO.PAYDATE = entityPM.PAYDATE;
            entityPOCO.PAYEETYPE = entityPM.PAYEETYPE;
            entityPOCO.PAYMETHOD = entityPM.PAYMETHOD;
            entityPOCO.PAYORDNO = entityPM.PAYORDNO;
            entityPOCO.PAYREF = entityPM.PAYREF;
            entityPOCO.TREATFILE = entityPM.TREATFILE;
            entityPOCO.TYPE = entityPM.TYPE;
            entityPOCO.VATBANK = entityPM.VATBANK;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCHRONIZED = entityPM.IS_SYNCHRONIZED;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUPAYLINEFPM entityPM, CCUPAYLINEF entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.ACCOUNTNAME = entityPOCO.ACCOUNTNAME;
            entityPM.BANKACCOUNT = entityPOCO.BANKACCOUNT;
            entityPM.BANKBRANCH = entityPOCO.BANKBRANCH;
            entityPM.BANKID = entityPOCO.BANKID;
            entityPM.HASHAVUTCODE = entityPOCO.HASHAVUTCODE;
            entityPM.PAYAMOUNT = entityPOCO.PAYAMOUNT;//.ToNullableDecimal("entityPOCO.PAYAMOUNT"); ;
            entityPM.PAYDATE = entityPOCO.PAYDATE;
            entityPM.PAYEETYPE = entityPOCO.PAYEETYPE;
            entityPM.PAYMETHOD = entityPOCO.PAYMETHOD;
            entityPM.PAYORDNO = entityPOCO.PAYORDNO;
            entityPM.PAYREF = entityPOCO.PAYREF;
            entityPM.TREATFILE = entityPOCO.TREATFILE;
            entityPM.TYPE = entityPOCO.TYPE;
            entityPM.VATBANK = entityPOCO.VATBANK;
            //entityPM.Tenant = (int)entityPOCO.tenant;
            //entityPM.IS_SYNCHRONIZED = (bool)entityPOCO.IS_SYNCHRONIZED;
            //entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT;
            entityPM.Tenant = entityPOCO.tenant != null ? (int)entityPOCO.tenant : 0;
            entityPM.IS_SYNCHRONIZED = entityPOCO.IS_SYNCHRONIZED != null ? (bool)entityPOCO.IS_SYNCHRONIZED : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;

        }

        public void CustomPMToPOCO(CCUPAYLINEFPM entityPM, CCUPAYLINEF entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUPAYLINEFPM entityPM, CCUPAYLINEF entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUPAYLINEFPM entityPM, CCUPAYLINEFPM oldEntityPM)
        {
          //  throw new NotImplementedException();
        }
    }
}
