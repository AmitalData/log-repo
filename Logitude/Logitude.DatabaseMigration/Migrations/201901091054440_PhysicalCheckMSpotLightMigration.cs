namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhysicalCheckMSpotLightMigration : DbMigration
    {
        public override void Up()
        {
         
        }
        
        public override void Down()
        {
            AddColumn("Customs.Declarations", "CourierSuspentionCode", c => c.String(maxLength: 2, unicode: false));
            CreateIndex("Customs.Declarations", "CourierSuspentionCode");
            AddForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes", "Code");
        }
    }
}
