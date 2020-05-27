namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeToPaymentTerm_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentTerms", "Code", c => c.String(maxLength: 4, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentTerms", "Code");
        }
    }
}
