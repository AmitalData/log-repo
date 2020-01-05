namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyLengthOfSupportDomainFieldInTenantManagement_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TenantManagements", "SupportDomain", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenantManagements", "SupportDomain", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
