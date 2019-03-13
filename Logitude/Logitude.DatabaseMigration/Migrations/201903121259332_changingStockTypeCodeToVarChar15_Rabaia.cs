namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingStockTypeCodeToVarChar15_Rabaia : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tenants", "StockTypeCode", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tenants", "StockTypeCode", c => c.String());
        }
    }
}
