namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Field_ScreenCode_To_ScreenFields_And_Remove_ForiegnKey : DbMigration
    {
        public override void Up()
        { 
            AddColumn("dbo.ScreenFields", "ScreenCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update ScreenFields set ScreenCode =(select Screens.Code from Screens where Id=ScreenFields.ScreenId)");
            AlterColumn("dbo.ScreenFields", "ScreenCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            Sql("ALTER TABLE ScreenFields DROP CONSTRAINT FK_ScreenScreenField");

         }
        
        public override void Down()
        {
            
            DropColumn("dbo.ScreenFields", "ScreenCode");
           
        }
    }
}
