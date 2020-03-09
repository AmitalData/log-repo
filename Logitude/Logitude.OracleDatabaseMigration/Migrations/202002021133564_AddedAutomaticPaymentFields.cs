namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAutomaticPaymentFields : DbMigration
    {
        public override void Up()
        {
             AddColumn("Customs.Declarations", "AvailabilityDate", c => c.DateTime(nullable: false, precision: 7));
            AddColumn("Customs.DeclarationPayments", "AutomaticPayment", c => c.Int(nullable: false));
         }
        
        public override void Down()
        {
             DropColumn("Customs.DeclarationPayments", "AutomaticPayment");
            DropColumn("Customs.Declarations", "AvailabilityDate");
         }
    }
}
