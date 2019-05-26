namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TariffContractNumberConvertedToString_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tariffs", "ContractNumber", c => c.String(maxLength: 25, unicode: false));           
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tariffs", "ContractNumber", c => c.Int());            
        }
    }
}
