namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateQBOOAuth2TkensLengthsV2_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccountingSettings", "QBOAccessToken", c => c.String());
            AlterColumn("dbo.AccountingSettings", "QBOAccessTokenSecret", c => c.String());
        }
    }   

        public override void Down()
        {
            AlterColumn("dbo.AccountingSettings", "QBOAccessTokenSecret", c => c.String(maxLength: 200));
            AlterColumn("dbo.AccountingSettings", "QBOAccessToken", c => c.String(maxLength: 200));
        }
    }
}
