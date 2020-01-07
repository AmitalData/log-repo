
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0007_AutonomyKeywords : PatchDistributionBase
    {
        public P19R03_0007_AutonomyKeywords()
             : base("הגדרת הצהרה אוטונומיה ", new DateTime(2020, 01, 5))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"CREATE TABLE CustomsAutonomyKeywords ( 
  Id VARCHAR2(15 CHAR) NOT NULL,
  Tenant NUMBER(10) NOT NULL,
  KeywordtypeCode VARCHAR2(1 CHAR) NOT NULL,
  KeywordsList NVARCHAR2(2000) NULL,
  PRIMARY KEY (Id)
)");

            this.AddUpSqlScript(@"ALTER TABLE CustomsAutonomyKeywords ADD CONSTRAINT uq_1AutonomyKeywords UNIQUE (tenant, keywordtypecode)");



        }
    }
}
