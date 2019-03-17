namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReconciliationLines", "SearchFields", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReconciliationLines", "SearchFields", c => c.String(maxLength: 4000));
        }
    }
}
