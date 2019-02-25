namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_Tenant_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "NumberFormatCode", c => c.String(maxLength: 4));
            CreateIndex("dbo.Tenants", "NumberFormatCode");
            AddForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats");
            DropIndex("dbo.Tenants", new[] { "NumberFormatCode" });
            DropColumn("dbo.Tenants", "NumberFormatCode");
        }
    }
}
