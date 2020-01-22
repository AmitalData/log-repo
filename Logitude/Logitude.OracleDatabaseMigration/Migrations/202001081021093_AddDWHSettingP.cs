namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDWHSettingP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DWHSettings",
                c => new
                    {
                        Tenant = c.Int(nullable: false),
                        ParentTenant = c.Int(),
                        Server = c.String(maxLength: 200, unicode: false),
                        UserName = c.String(maxLength: 200, unicode: false),
                        Password_ = c.String(maxLength: 200, unicode: false),
                        Catalog = c.String(maxLength: 200, unicode: false),
                        IsParentTenant = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Tenant);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DWHSettings");
        }
    }
}
