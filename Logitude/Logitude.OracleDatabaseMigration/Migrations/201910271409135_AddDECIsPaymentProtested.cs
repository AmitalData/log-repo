namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDECIsPaymentProtested : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Declarations", "IsPaymentProtested", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.Declarations", "IsPaymentProtested");
        }
    }
}
