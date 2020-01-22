namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeDWHSetting : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.DWHSettings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DWHSettings",
                c => new
                    {
                        Tenant = c.Int(nullable: false, identity: true),
                        ParentTenant = c.Int(),
                        Server = c.String(maxLength: 200, unicode: false),
                        UserName = c.String(maxLength: 200, unicode: false),
                        Password_ = c.String(maxLength: 200, unicode: false),
                        Catalog = c.String(maxLength: 200, unicode: false),
                        IsParentTenant = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Tenant);
            
        }
    }
}
