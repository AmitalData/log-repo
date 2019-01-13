namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationChangeResponseStatus : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.DeclarationCargoSplits", "ResponseStatusCode", "Customs.CargoSplitRequestStatuses");
            DropIndex("Customs.DeclarationCargoSplits", new[] { "ResponseStatusCode" });
            DropPrimaryKey("Customs.CargoSplitRequestStatuses");
            AlterColumn("Customs.CargoSplitRequestStatuses", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("Customs.DeclarationCargoSplits", "ResponseStatusCode", c => c.String(maxLength: 2, unicode: false));
            AddPrimaryKey("Customs.CargoSplitRequestStatuses", "Code");
            CreateIndex("Customs.DeclarationCargoSplits", "ResponseStatusCode");
            AddForeignKey("Customs.DeclarationCargoSplits", "ResponseStatusCode", "Customs.CargoSplitRequestStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeclarationCargoSplits", "ResponseStatusCode", "Customs.CargoSplitRequestStatuses");
            DropIndex("Customs.DeclarationCargoSplits", new[] { "ResponseStatusCode" });
            DropPrimaryKey("Customs.CargoSplitRequestStatuses");
            AlterColumn("Customs.DeclarationCargoSplits", "ResponseStatusCode", c => c.String(maxLength: 1, unicode: false));
            AlterColumn("Customs.CargoSplitRequestStatuses", "Code", c => c.String(nullable: false, maxLength: 1, unicode: false));
            AddPrimaryKey("Customs.CargoSplitRequestStatuses", "Code");
            CreateIndex("Customs.DeclarationCargoSplits", "ResponseStatusCode");
            AddForeignKey("Customs.DeclarationCargoSplits", "ResponseStatusCode", "Customs.CargoSplitRequestStatuses", "Code");
        }
    }
}
