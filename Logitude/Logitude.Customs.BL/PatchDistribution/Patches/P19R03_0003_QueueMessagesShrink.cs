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

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript("SELECT COUNT(*) FROM QUEUEMESSAGES  WHERE  STATUS=0 ");
            this.AddUpSqlScript("create table zzz_QUEUEMESSAGES  as SELECT * FROM QUEUEMESSAGES WHERE  STATUS=0 ");
            this.AddUpSqlScript("SELECT COUNT(*) FROM zzz_QUEUEMESSAGES  ");
            this.AddUpSqlScript("truncate table  QUEUEMESSAGEMOREDETAILS ");
            this.AddUpSqlScript("truncate table  QUEUEMESSAGES  ");
            this.AddUpSqlScript("insert into  QUEUEMESSAGES  SELECT * FROM zzz_QUEUEMESSAGES   ");
            this.AddUpSqlScript("drop TABLE zzz_QUEUEMESSAGES   ");
                
        
        }
    }
}
