namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseTenantNotesFieldLength_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TenantManagements", "Notes", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenantManagements", "Notes", c => c.String(maxLength: 250));
        }
    }
}
