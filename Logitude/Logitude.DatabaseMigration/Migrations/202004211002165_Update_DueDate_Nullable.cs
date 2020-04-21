namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_DueDate_Nullable : DbMigration
    {
        public override void Up()
        {
 
            AlterColumn("dbo.Journals", "DocumentDate", c => c.DateTime());
            AlterColumn("dbo.Journals", "DueDate", c => c.DateTime());
          
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.Journals", "DueDate");
            DropColumn("dbo.Journals", "DocumentDate");
           
        }
    }
}
