
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0026_ExceptionReason : PatchDistributionBase
    {
        public P19R03_0026_ExceptionReason()
         : base(" add ExceptionReason table  ", new DateTime(2020, 05, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@" CREATE TABLE  EXCEPTIONREASONS  
                                   ( CODE  VARCHAR2(4 CHAR) DEFAULT NULL NOT NULL ENABLE,
                                   TENANT  NUMBER(10, 0) NOT NULL ENABLE,
                                   ENGLISHNAME  VARCHAR2(100 CHAR),
                                   LOCALNAME  NVARCHAR2(100),
                                   ISACTIVE  NUMBER(1, 0) NOT NULL ENABLE,
                                   UNIFREIGHTSTATUSCODE  VARCHAR2(3 CHAR),
                                   SEARCHFIELDS  NVARCHAR2(1000),
                                   PRIMARY KEY(CODE))");
 
        }
    }
}
