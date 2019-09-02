namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Currency_TariffLine_Khalid : DbMigration
    {
        public override void Up()
        {       
            AddColumn("dbo.TariffLines", "CurrencyId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {          
            DropColumn("dbo.TariffLines", "CurrencyId");
        }
    }
}
