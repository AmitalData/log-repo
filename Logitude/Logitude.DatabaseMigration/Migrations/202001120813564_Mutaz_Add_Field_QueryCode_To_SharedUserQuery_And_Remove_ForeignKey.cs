namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Field_QueryCode_To_SharedUserQuery_And_Remove_ForeignKey : DbMigration
    {
        public override void Up()
        {


            AddColumn("dbo.SharedUserQueries", "QueryCode", c => c.String(nullable: true, maxLength: 30, unicode: false));
            Sql(@"update SharedUserQueries set QueryCode = (select Queries.Code from Queries where Id = SharedUserQueries.QueryId)");
            AlterColumn("dbo.SharedUserQueries", "QueryCode", c => c.String(nullable: false, maxLength: 30, unicode: false));
            Sql("ALTER TABLE SharedUserQueries DROP CONSTRAINT  [FK_dbo.SharedUserQueries_dbo.Queries_QueryId]");
        }
        
        public override void Down()
        {
            DropColumn("dbo.SharedUserQueries", "QueryCode");
        }
    }
}
