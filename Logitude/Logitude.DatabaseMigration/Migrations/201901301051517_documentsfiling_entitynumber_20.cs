namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documentsfiling_entitynumber_20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DocumentsFilings", "EntityNumber", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DocumentsFilings", "EntityNumber", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
