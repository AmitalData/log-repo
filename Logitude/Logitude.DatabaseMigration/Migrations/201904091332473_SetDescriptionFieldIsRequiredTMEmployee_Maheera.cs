namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetDescriptionFieldIsRequiredTMEmployee_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TMEmployeeTimes", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TMEmployeeTimes", "Description", c => c.String(maxLength: 500));
        }
    }
}
