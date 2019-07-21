namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchedulerProcedure_Nawras : DbMigration
    {
        public override void Up()
        {
           //DropIndex("dbo.BankAccounts", new[] { "CurrencyId" });
            CreateTable(
                "dbo.SchedulerProcedure",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 100, unicode: false),
                        Name = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(),
                        Description = c.String(nullable: false, maxLength: 1000, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            //AddColumn("dbo.FullAccountingSettings", "PaymentChequesLogoId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.BankAccounts", "CurrencyId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //CreateIndex("dbo.BankAccounts", "CurrencyId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BankAccounts", new[] { "CurrencyId" });
            AlterColumn("dbo.BankAccounts", "CurrencyId", c => c.String(maxLength: 15, unicode: false));
            DropColumn("dbo.FullAccountingSettings", "PaymentChequesLogoId");
            DropTable("dbo.SchedulerProcedure");
            CreateIndex("dbo.BankAccounts", "CurrencyId");
        }
    }
}
