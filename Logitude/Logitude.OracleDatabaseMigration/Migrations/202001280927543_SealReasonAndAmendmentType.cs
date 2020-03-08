namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SealReasonAndAmendmentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.AmendmentTypes", "EnglishName", c => c.String(maxLength: 100, unicode: false));
            AddColumn("Customs.SealUpdateReasonTypes", "EnglishName", c => c.String(maxLength: 100, unicode: false));

        }

        public override void Down()
        {
            DropColumn("Customs.AmendmentTypes", "EnglishName");
            DropColumn("Customs.SealUpdateReasonTypes", "EnglishName");

        }
    }
}
