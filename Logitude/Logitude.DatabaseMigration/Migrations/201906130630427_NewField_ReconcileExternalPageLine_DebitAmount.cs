namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_ReconcileExternalPageLine_DebitAmount : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Tenants", "DefaultWarningPercentage", c => c.Double());
            AddColumn("dbo.ReconcileExternalPageLines", "DebitAmount", c => c.Decimal(nullable: false, precision: 15, scale: 2));
            AddColumn("dbo.ReconcileExternalPageLines", "Creditamount", c => c.Decimal(nullable: false, precision: 15, scale: 2));
            DropColumn("dbo.ReconcileExternalPageLines", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReconcileExternalPageLines", "Amount", c => c.Decimal(nullable: false, precision: 15, scale: 2));
            DropColumn("dbo.ReconcileExternalPageLines", "Creditamount");
            DropColumn("dbo.ReconcileExternalPageLines", "DebitAmount");
            //DropColumn("dbo.Tenants", "DefaultWarningPercentage");
        }
    }
}
