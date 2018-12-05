namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GLAccountMoreDataTotalFutureOpenChequeInLocalCurrencyMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GLAccountMoreDatas", "TotFutureOpenChequesInLocalCur", c => c.Decimal(precision: 16, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GLAccountMoreDatas", "TotFutureOpenChequesInLocalCur");
        }
    }
}
