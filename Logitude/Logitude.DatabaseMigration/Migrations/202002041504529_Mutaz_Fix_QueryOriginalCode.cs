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
            Sql("ALTER TABLE [dbo].[Queries] ADD  CONSTRAINT [UQ_UniqueCode] UNIQUE NONCLUSTERED ([UniqueCode] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]");
        }
        
        public override void Down()
        {
        
        }
    }
}
