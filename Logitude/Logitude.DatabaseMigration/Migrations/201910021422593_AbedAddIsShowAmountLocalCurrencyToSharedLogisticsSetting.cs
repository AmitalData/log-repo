namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddIsShowAmountLocalCurrencyToSharedLogisticsSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SharedLogisticsSettings", "IsShowAmountLocalCurrency", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SharedLogisticsSettings", "IsShowAmountLocalCurrency");
        }
    }
}
