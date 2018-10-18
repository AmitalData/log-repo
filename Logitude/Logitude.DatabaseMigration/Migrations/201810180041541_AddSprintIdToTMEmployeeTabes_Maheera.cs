namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSprintIdToTMEmployeeTabes_Maheera : DbMigration
    {
        public override void Up()
        {        
            AddColumn("dbo.TMEmployeeTimes", "SprintId", c => c.String(maxLength: 15, unicode: false));          
        }
        
        public override void Down()
        {
            DropColumn("dbo.TMEmployeeTimes", "SprintId");
        }
    }
}
