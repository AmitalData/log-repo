namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARPaymentIsExternalEntityMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARPayments", "IsExternalEntity", c => c.Boolean());
        }
        
        public override void Down()
        {
        }
    }
}
