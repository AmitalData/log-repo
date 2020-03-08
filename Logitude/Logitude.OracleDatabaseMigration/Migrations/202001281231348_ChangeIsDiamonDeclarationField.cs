namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIsDiamonDeclarationField : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Declarations", "IsDiamondDeclaration", c => c.Boolean(nullable: false));
            DropColumn("Customs.Declarations", "DeclarationType");
        }
        
        public override void Down()
        {
            AddColumn("Customs.Declarations", "DeclarationType", c => c.String(maxLength: 1, unicode: false));
            DropColumn("Customs.Declarations", "IsDiamondDeclaration");
        }
    }
}
