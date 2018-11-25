namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_BTE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchTaskExecutions", "CallStack", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchTaskExecutions", "CallStack");
        }
    }
}
