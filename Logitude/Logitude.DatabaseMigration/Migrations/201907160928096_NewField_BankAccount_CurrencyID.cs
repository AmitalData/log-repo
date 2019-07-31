namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_BankAccount_CurrencyID : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.INTTRABookingStatus", newName: "INTTRABookingStatuses");
            //RenameTable(name: "dbo.INTTRABookingTransStatus", newName: "INTTRABookingTransStatuses");
            //AddColumn("dbo.Users", "AdditionalPackagesOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.BankAccounts", "CurrencyId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.BankAccounts", "CurrencyId");
            AddForeignKey("dbo.BankAccounts", "CurrencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.BankAccounts", new[] { "CurrencyId" });
            DropColumn("dbo.BankAccounts", "CurrencyId");
            //DropColumn("dbo.Users", "AdditionalPackagesOnly");
            //RenameTable(name: "dbo.INTTRABookingTransStatuses", newName: "INTTRABookingTransStatus");
            //RenameTable(name: "dbo.INTTRABookingStatuses", newName: "INTTRABookingStatus");
        }
    }
}
