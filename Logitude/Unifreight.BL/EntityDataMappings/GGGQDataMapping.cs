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
    public class GGGQDataMapping : IMapping<GGGQPM, GGGQ>
    {

        public enum POCOPropertyNames
        {
            None,

        }
        public enum PMPropertyNames
        {
            None,
        }

        public void PMToPOCO(GGGQPM entityPM, GGGQ entityPOCO)
        {


            entityPOCO.QUEID = entityPM.QUEID;

            entityPOCO.CREATEDATE = entityPM.CREATEDATE;
            entityPOCO.ORIGINQUE = entityPM.ORIGINQUE;
            //entityPOCO.USERID = entityPM.USERID;
            entityPOCO.DONEOPERATION = entityPM.DONEOPERATION;
            entityPOCO.STATUS = entityPM.STATUS;
            entityPOCO.EXPTASKTIME = entityPM.EXPTASKTIME;
            entityPOCO.TRY = entityPM.TRY;
            entityPOCO.PRIORITY = entityPM.PRIORITY;
            entityPOCO.ENTNAME = entityPM.ENTNAME;
            entityPOCO.PRIMARYNUM = entityPM.PRIMARYNUM;
            //entityPOCO.PROCESSID = entityPM.PROCESSID;
            entityPOCO.FORMID = entityPM.FORMID;
            //entityPOCO.COMPUTERID = entityPM.COMPUTERID;
            ///entityPOCO.QUEUEMANAGEMENT = entityPM.QUEUEMANAGEMENT;
            //entityPOCO.OTHERASNFILE = entityPM.OTHERASNFILE;
            //entityPOCO.STOPPEDBYSM = entityPM.STOPPEDBYSM;
            entityPOCO.DEBUG = entityPM.DEBUG;
            //entityPOCO.WEAKREF = entityPM.WEAKREF;
            //entityPOCO.HUGERECORD = entityPM.HUGERECORD;
            entityPOCO.EXECDATE = entityPM.EXECDATE;
            entityPOCO.GSTRING1 = entityPM.GSTRING1;
            entityPOCO.GSTRING2 = entityPM.GSTRING2;
            entityPOCO.GSTRING3 = entityPM.GSTRING3;
            entityPOCO.QUEUEMANAGEMENT = entityPM.QUEUEMANAGEMENT;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(GGGQPM entityPM, GGGQ entityPOCO)
        {
            entityPM.QUEID = entityPOCO.QUEID;

            entityPM.CREATEDATE = entityPOCO.CREATEDATE;
            entityPM.ORIGINQUE = entityPOCO.ORIGINQUE;
            //entityPM.USERID = entityPOCO.USERID;
            entityPM.DONEOPERATION = entityPOCO.DONEOPERATION;
            entityPM.STATUS = entityPOCO.STATUS;
            entityPM.EXPTASKTIME = entityPOCO.EXPTASKTIME;
            entityPM.TRY = entityPOCO.TRY;
            entityPM.PRIORITY = entityPOCO.PRIORITY;
            entityPM.ENTNAME = entityPOCO.ENTNAME;
            entityPM.PRIMARYNUM = entityPOCO.PRIMARYNUM;
            //entityPM.PROCESSID = entityPOCO.PROCESSID;
            entityPM.FORMID = entityPOCO.FORMID;
            //entityPM.COMPUTERID = entityPOCO.COMPUTERID;
            //entityPM.QUEUEMANAGEMENT = entityPOCO.QUEUEMANAGEMENT;
            //entityPM.OTHERASNFILE = entityPOCO.OTHERASNFILE;
            //entityPM.STOPPEDBYSM = entityPOCO.STOPPEDBYSM;
            entityPM.DEBUG = entityPOCO.DEBUG;
            //entityPM.WEAKREF = entityPOCO.WEAKREF;
            //entityPM.HUGERECORD = entityPOCO.HUGERECORD;
            entityPM.EXECDATE = entityPOCO.EXECDATE;
            entityPM.GSTRING1 = entityPOCO.GSTRING1;
            entityPM.GSTRING2 = entityPOCO.GSTRING2;
            entityPM.GSTRING3 = entityPOCO.GSTRING3;
            entityPM.QUEUEMANAGEMENT = entityPOCO.QUEUEMANAGEMENT;
            entityPM.Tenant = (int)entityPOCO.tenant;
            entityPM.IS_SYNCH = (bool)entityPOCO.IS_SYNCH;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT;

        }

        public void CustomPMToPOCO(GGGQPM entityPM, GGGQ entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.WEAKREF = "F";
                entityPOCO.HUGERECORD = "F";
                entityPOCO.STOPPEDBYSM = false;
                entityPOCO.DONEOPERATION = "D";
                entityPOCO.COMPUTERID = Environment.MachineName;
                entityPOCO.USERID = "SYSTEM";
                entityPOCO.QUEUEMANAGEMENT = false;
                entityPOCO.OTHERASNFILE = false;
                entityPOCO.PROCESSID = System.Diagnostics.Process.GetCurrentProcess().Id;


            }
                        
        }

        public void CustomPOCOToPM(GGGQPM entityPM, GGGQ entityPOCO)
        {
            //throw new NotImplementedException();
        }


        public void PMToOldPM(GGGQPM entityPM, GGGQPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
