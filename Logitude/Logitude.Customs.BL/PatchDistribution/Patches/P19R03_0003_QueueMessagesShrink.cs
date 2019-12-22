using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0003_QueueMessagesShrink : PatchDistributionBase
    {
        public P19R03_0003_QueueMessagesShrink()
            :base("מחיקת תור ישן",new DateTime(2019, 12, 2))
        {

        }

        public override List<ScriptDTO> GetDownScripts()
        {
            throw new NotImplementedException();
        }

        public override List<ScriptDTO> GetUpScripts()
        {
            int ScriptCount = 0;
            return new List<ScriptDTO>()
            {
                new ScriptDTO()
                { 
                    ScriptCounter = ScriptCount++,
                    SqlScript = "SELECT COUNT(*) FROM QUEUEMESSAGES  WHERE  STATUS=0 "
                },

                new ScriptDTO() { 
                    ScriptCounter = ScriptCount++, 
                    SqlScript = "create table zzz_QUEUEMESSAGES  as SELECT * FROM QUEUEMESSAGES WHERE  STATUS=0 " 
                },

                //new ScriptDTO() { Count = ScriptCount++, SqlScript = "commit;" },
                new ScriptDTO() { 
                    ScriptCounter = ScriptCount++, 
                    SqlScript = "SELECT COUNT(*) FROM zzz_QUEUEMESSAGES  " 
                },

                new ScriptDTO() {
                    ScriptCounter = ScriptCount++,
                    SqlScript = "truncate table  QUEUEMESSAGEMOREDETAILS "
                },
                new ScriptDTO() {
                    ScriptCounter = ScriptCount++,
                    SqlScript = "truncate table  QUEUEMESSAGES  "
                },

                new ScriptDTO()
                {
                     ScriptCounter = ScriptCount++,
                      SqlScript ="insert into  QUEUEMESSAGES  SELECT * FROM zzz_QUEUEMESSAGES   "
                },
                //new ScriptDTO() { Count = ScriptCount++, SqlScript = "commit;" },
                new ScriptDTO()
                {
                     ScriptCounter = ScriptCount++,
                      SqlScript ="drop TABLE zzz_QUEUEMESSAGES   "
                },
        };
        }
    }
}
