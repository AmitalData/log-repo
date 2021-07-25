
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
    public class GITITEMDataMapping : IMapping<GITITEMPM, GITITEM>
    {
        public void PMToPOCO(GITITEMPM entityPM, GITITEM entityPOCO)
        {
            entityPOCO.COUNTER = entityPM.COUNTER;
            entityPOCO.PARTNERID = entityPM.PARTNERID;
            entityPOCO.ITEMNO = entityPM.ITEMNO;
            entityPOCO.SAPAKID = entityPM.SAPAKID;
            entityPOCO.OPENDATE = entityPM.OPENDATE;
            entityPOCO.BRANCHID = entityPM.BRANCHID;
            entityPOCO.ACCOUNTINGCLOSE = entityPM.ACCOUNTINGCLOSE;
            entityPOCO.ITEMCLOSE = entityPM.ITEMCLOSE;
            entityPOCO.ITEMCANCELLED = entityPM.ITEMCANCELLED;
            entityPOCO.ITEMOPENUSER = entityPM.ITEMOPENUSER;
            entityPOCO.ITEMUPDATEDATE = entityPM.ITEMUPDATEDATE;
            entityPOCO.PRATID = entityPM.PRATID;
            entityPOCO.ITEMUPDATEUSER = entityPM.ITEMUPDATEUSER;
            entityPOCO.NOSTANDART = entityPM.NOSTANDART;
            entityPOCO.APPROVTYPEID = entityPM.APPROVTYPEID;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.SEARCHENG = entityPM.SEARCHENG;
            entityPOCO.LICENCESIV = entityPM.LICENCESIV;
            entityPOCO.ORIGINCOUNTRY = entityPM.ORIGINCOUNTRY;
            entityPOCO.UNITID = entityPM.UNITID;
            entityPOCO.FACTOR = entityPM.FACTOR;
            entityPOCO.VERIFICATIONNUMBER = entityPM.VERIFICATIONNUMBER;
            entityPOCO.TARIFFID = entityPM.TARIFFID;
            entityPOCO.IMPAPPROVTYPEID = entityPM.IMPAPPROVTYPEID;
            entityPOCO.SIVUGINSTRUCTION = entityPM.SIVUGINSTRUCTION;
            entityPOCO.REMARKSMAKAT = entityPM.REMARKSMAKAT;
            entityPOCO.REMARKSPROTEST = entityPM.REMARKSPROTEST;
        }

        public void POCOToPM(GITITEMPM entityPM, GITITEM entityPOCO)
        {
            entityPM.COUNTER = entityPOCO.COUNTER;
            entityPM.PARTNERID = entityPOCO.PARTNERID;
            entityPM.ITEMNO = entityPOCO.ITEMNO;
            entityPM.SAPAKID = entityPOCO.SAPAKID;
            entityPM.OPENDATE = entityPOCO.OPENDATE;
            entityPM.BRANCHID = entityPOCO.BRANCHID;
            entityPM.ACCOUNTINGCLOSE = entityPOCO.ACCOUNTINGCLOSE;
            entityPM.ITEMCLOSE = entityPOCO.ITEMCLOSE;
            entityPM.ITEMCANCELLED = entityPOCO.ITEMCANCELLED;
            entityPM.ITEMOPENUSER = entityPOCO.ITEMOPENUSER;
            entityPM.ITEMUPDATEDATE = entityPOCO.ITEMUPDATEDATE;
            entityPM.PRATID = entityPOCO.PRATID;
            entityPM.ITEMUPDATEUSER = entityPOCO.ITEMUPDATEUSER;
            entityPM.NOSTANDART = entityPOCO.NOSTANDART;
            entityPM.APPROVTYPEID = entityPOCO.APPROVTYPEID;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.SEARCHENG = entityPOCO.SEARCHENG;
            entityPM.LICENCESIV = entityPOCO.LICENCESIV;
            entityPM.ORIGINCOUNTRY = entityPOCO.ORIGINCOUNTRY;
            entityPM.UNITID = entityPOCO.UNITID;
            entityPM.FACTOR = entityPOCO.FACTOR;
            entityPM.VERIFICATIONNUMBER = entityPOCO.VERIFICATIONNUMBER;
            entityPM.TARIFFID = entityPOCO.TARIFFID;
            entityPM.IMPAPPROVTYPEID = entityPOCO.IMPAPPROVTYPEID;
            entityPM.SIVUGINSTRUCTION = entityPOCO.SIVUGINSTRUCTION;
            entityPM.REMARKSMAKAT = entityPOCO.REMARKSMAKAT;
            entityPM.REMARKSPROTEST = entityPOCO.REMARKSPROTEST;
        }

        public void CustomPMToPOCO(GITITEMPM entityPM, GITITEM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GITITEMPM entityPM, GITITEM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GITITEMPM entityPM, GITITEMPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
