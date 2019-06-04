namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingcustoms2905_3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.PendingErrorPlaces",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            //AddColumn("dbo.ObjectFields", "EnableFullscreenTextBox", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectFields", "EnableFullscreenTextBox");
            DropTable("Customs.PendingErrorPlaces");
        }
    }
}
