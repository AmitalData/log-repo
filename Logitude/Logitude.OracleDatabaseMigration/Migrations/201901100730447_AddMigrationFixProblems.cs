namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationFixProblems : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Declarations", "CourierSuspentionCode", c => c.String(maxLength: 2, unicode: false));
            CreateIndex("Customs.Declarations", "CourierSuspentionCode");
            AddForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes");
            DropIndex("Customs.Declarations", new[] { "CourierSuspentionCode" });
            DropColumn("Customs.Declarations", "CourierSuspentionCode");
        }
    }
}
