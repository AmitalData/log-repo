using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0031_AddControllerUserId : PatchDistributionBase
    {
        public P19R03_0031_AddControllerUserId()
            : base("add controller user id ", new DateTime(2020, 05, 07))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas ADD ClassifiedUserId VARCHAR2(15 CHAR) NULL");
            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas ADD ControllerUserId VARCHAR2(15 CHAR) NULL");
            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas ADD CollectorUserId VARCHAR2(15 CHAR) NULL");
            this.AddUpSqlScript(@"CREATE INDEX IX_1987390991 ON DeclarationReferantDatas (ClassifiedUserId)");
            this.AddUpSqlScript(@"CREATE INDEX IX_N961569312 ON DeclarationReferantDatas (ControllerUserId)");
            this.AddUpSqlScript(@"CREATE INDEX IX_N52936576 ON DeclarationReferantDatas (CollectorUserId)");
            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas
  ADD CONSTRAINT FK_590030953 FOREIGN KEY (ClassifiedUserId) REFERENCES Users (Id)");
            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas
  ADD CONSTRAINT FK_N274799755 FOREIGN KEY (CollectorUserId) REFERENCES Users (Id)");
            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas
  ADD CONSTRAINT FK_N1551769519 FOREIGN KEY (ControllerUserId) REFERENCES Users (Id)");

        }
    }
}
