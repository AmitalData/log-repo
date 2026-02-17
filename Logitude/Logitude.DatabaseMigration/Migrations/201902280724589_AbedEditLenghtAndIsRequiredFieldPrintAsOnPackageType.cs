namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedEditLenghtAndIsRequiredFieldPrintAsOnPackageType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PackageTypes", "PrintAs", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PackageTypes", "PrintAs", c => c.String(maxLength: 5, unicode: false));
        }
    }
}
