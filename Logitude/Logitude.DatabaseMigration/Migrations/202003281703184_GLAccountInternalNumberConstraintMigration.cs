namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GLAccountInternalNumberConstraintMigration : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE GLACCOUNTS  ADD  CONSTRAINT [UQ_InternalNumber_Tenant] UNIQUE NONCLUSTERED ([Tenant] ASC,[InternalNumber] ASC)");

        }

        public override void Down()
        {
            DropForeignKey("dbo.WarehouseReleases", "TruckerId", "dbo.Cards");
            DropIndex("dbo.WarehouseReleases", new[] { "TruckerId" });
            DropColumn("dbo.WarehouseReleases", "TruckerReference");
            DropColumn("dbo.WarehouseReleases", "TruckerId");
        }
    }
}
