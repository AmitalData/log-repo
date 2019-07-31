namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsVeichleToPackageType_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PackageTypes", "IsVehicle", c => c.Boolean(nullable: false));

            Sql(@"update PackageTypes set IsVehicle = 1, IsContainer = 0, IsRefrigerated = 0
                where Code = 'E5' or Code = 'E6' or Code = 'H6'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.PackageTypes", "IsVehicle");
        }
    }
}
