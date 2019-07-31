namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsExternalEntityFix : DbMigration
    {
        public override void Up()
        {
            Sql("update ARPayments set IsExternalEntity = 0 where IsExternalEntity is null");
            AlterColumn("dbo.ARPayments", "IsExternalEntity", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {

        }
    }
}
