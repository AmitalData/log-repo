namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocalAddressToTenantLevel_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "LocalAddressId", c => c.String(maxLength: 15, unicode: false));           
            CreateIndex("dbo.Tenants", "LocalAddressId");
            AddForeignKey("dbo.Tenants", "LocalAddressId", "dbo.Addresses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tenants", "LocalAddressId", "dbo.Addresses");
            DropIndex("dbo.Tenants", new[] { "LocalAddressId" });            
            DropColumn("dbo.Tenants", "LocalAddressId");
        }
    }
}
