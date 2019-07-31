namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTaeiffLinePricesFromIntegerToDecimal_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TariffLines", "MinPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TariffLines", "Step1Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TariffLines", "Step2Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TariffLines", "Step3Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TariffLines", "Step4Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TariffLines", "Step5Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TariffLines", "Step6Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TariffLines", "Step7Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TariffLines", "Step8Price", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TariffLines", "Step8Price", c => c.Int());
            AlterColumn("dbo.TariffLines", "Step7Price", c => c.Int());
            AlterColumn("dbo.TariffLines", "Step6Price", c => c.Int());
            AlterColumn("dbo.TariffLines", "Step5Price", c => c.Int());
            AlterColumn("dbo.TariffLines", "Step4Price", c => c.Int());
            AlterColumn("dbo.TariffLines", "Step3Price", c => c.Int());
            AlterColumn("dbo.TariffLines", "Step2Price", c => c.Int());
            AlterColumn("dbo.TariffLines", "Step1Price", c => c.Int());
            AlterColumn("dbo.TariffLines", "MinPrice", c => c.Int());            
        }
    }
}
