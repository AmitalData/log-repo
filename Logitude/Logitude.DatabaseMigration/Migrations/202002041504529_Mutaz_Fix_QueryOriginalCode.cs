namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Fix_QueryOriginalCode : DbMigration
    {
        public override void Up()
        {
            Sql("update Queries set OriginalQueryCode = (select q1.UniqueCode from Queries q1 where q1.Id = Queries.OriginalQueryId)");
            Sql("IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Queries]') AND name = N'UQ_ObjectTableId_UserId_UniqueCode') ALTER TABLE[dbo].[Queries] DROP CONSTRAINT[UQ_ObjectTableId_UserId_UniqueCode]");
        }
        
        public override void Down()
        {
        
        }
    }
}
