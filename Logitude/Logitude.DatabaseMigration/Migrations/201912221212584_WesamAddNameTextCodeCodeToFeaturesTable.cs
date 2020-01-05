namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddNameTextCodeCodeToFeaturesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Features", "NameTextCodeCode", c => c.String(nullable: true, maxLength: 100, unicode: false));
            Sql(@"update Features set NameTextCodeCode = (select TextCodes.Code from TextCodes where id = Features.NameTextCodeId)");
            AlterColumn("dbo.Features", "NameTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Features", "NameTextCodeCode");
        }
    }
}
