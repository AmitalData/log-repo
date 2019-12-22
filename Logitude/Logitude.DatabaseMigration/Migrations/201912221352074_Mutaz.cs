namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz : DbMigration
    {
        public override void Up()
        {
            Sql("IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__GLAccount__Minim__43836F81]') AND type = 'D') BEGIN ALTER TABLE[dbo].[GLAccounts] DROP CONSTRAINT[DF__GLAccount__Minim__43836F81] END");
            AlterColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int());

        }

        public override void Down()
        {
        }
    }
}
