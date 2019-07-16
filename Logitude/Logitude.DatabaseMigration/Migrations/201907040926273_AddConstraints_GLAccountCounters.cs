namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConstraints_GLAccountCounters : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE GLAccountCounters ADD CONSTRAINT[UQ_Tenant_Prefix] UNIQUE NONCLUSTERED ([Tenant] ASC,[Prefix] ASC)");
        }

        public override void Down()
        {
        }
    }
}
