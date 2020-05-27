namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddOpenAmountLocalCurrency_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARPayments", "OpenAmountInLocalCurrency", c => c.Double());
        }

        public override void Down()
        {
            DropColumn("dbo.ARPayments", "OpenAmountInLocalCurrency");
        }
    }
}
