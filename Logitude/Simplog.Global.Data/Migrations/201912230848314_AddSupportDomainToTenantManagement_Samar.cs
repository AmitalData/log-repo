namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSupportDomainToTenantManagement_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantManagements", "SupportDomain", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantManagements", "SupportDomain");
        }
    }
}
