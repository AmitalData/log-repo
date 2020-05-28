namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddNameTextCodeCodeToQueriesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Queries", "NameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            Sql(@"update Queries set NameTextCodeCode = (select TextCodes.Code from TextCodes where id = Queries.NameTextCodeId)");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Queries", "NameTextCodeCode");
        }
    }
}
