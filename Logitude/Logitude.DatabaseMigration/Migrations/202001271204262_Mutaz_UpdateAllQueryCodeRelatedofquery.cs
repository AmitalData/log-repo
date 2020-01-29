namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_UpdateAllQueryCodeRelatedofquery : DbMigration
    {
        public override void Up()
        {
            Sql("update Queries set OriginalQueryCode =  Queries.UniqueCode");
            Sql("update AdvancedQueryFilters set QueryCode = (select Queries.UniqueCode from Queries where Id = AdvancedQueryFilters.QueryId)");
            Sql("update QueryColumns set QueryCode = (select Queries.UniqueCode from Queries where Id = QueryColumns.QueryId)");
            Sql("update SharedUserQueries set QueryCode = (select Queries.UniqueCode from Queries where Id = SharedUserQueries.QueryId)");

        }

        public override void Down()
        {
        }
    }
}
