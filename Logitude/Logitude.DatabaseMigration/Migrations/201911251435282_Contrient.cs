namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Contrient : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE [dbo].[InterestBasesTypes] ADD  CONSTRAINT [UQ_InterestBasesTypes_Code_Tenant] UNIQUE NONCLUSTERED ( [Tenant] ASC,[Code] ASC)");
        }
        
        public override void Down()
        {

        }
    }
}
