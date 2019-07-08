namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultCurrencyInChargesType_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ChargesTypes", "PayablesDefaultCurrencyId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId");
            CreateIndex("dbo.ChargesTypes", "PayablesDefaultCurrencyId");
            AddForeignKey("dbo.ChargesTypes", "PayablesDefaultCurrencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ChargesTypes", "PayablesDefaultCurrencyId", "dbo.Currencies");
            DropIndex("dbo.ChargesTypes", new[] { "PayablesDefaultCurrencyId" });
            DropIndex("dbo.ChargesTypes", new[] { "ReceivablesDefaultCurrencyId" });
            DropColumn("dbo.ChargesTypes", "PayablesDefaultCurrencyId");
            DropColumn("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId");
        }
    }
}
