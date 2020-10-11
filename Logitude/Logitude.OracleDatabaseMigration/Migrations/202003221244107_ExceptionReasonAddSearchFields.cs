namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExceptionReasonAddSearchFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.ExceptionReasons", "SearchFields", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("Customs.ExceptionReasons", "SearchFields");
        }
    }
}
