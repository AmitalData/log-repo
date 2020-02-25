namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTenantInactive : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CurrencyTypeTenants", "TenantInactive", c => c.Boolean(nullable: false));
            Sql("ALTER TABLE CurrencyTypeTenants ADD CONSTRAINT CurrencyTypeTenantUQ UNIQUE(tenant,code) ");
        }
        
        public override void Down()
        {
            DropColumn("Customs.CurrencyTypeTenants", "TenantInactive");
            Sql("ALTER TABLE CurrencyTypeTenants DROP  CONSTRAINT CurrencyTypeTenantUQ  ");
        }
    }
}
