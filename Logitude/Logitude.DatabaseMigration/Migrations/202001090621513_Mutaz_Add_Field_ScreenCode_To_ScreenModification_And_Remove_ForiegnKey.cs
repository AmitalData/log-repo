namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Field_ScreenCode_To_ScreenModification_And_Remove_ForiegnKey : DbMigration
    {
        public override void Up()
        {


            AddColumn("dbo.ScreenModifications", "ScreenCode", c => c.String(nullable: true, maxLength: 100, unicode: false));
            Sql(@"update ScreenModifications set ScreenCode =(select Screens.Code from Screens where Id=ScreenModifications.ScreenId)");
            AlterColumn("dbo.ScreenModifications", "ScreenCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            Sql("ALTER TABLE ScreenModifications DROP CONSTRAINT FK_ScreenModificationScreen");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScreenModifications", "ScreenCode");
        }
    }
}
