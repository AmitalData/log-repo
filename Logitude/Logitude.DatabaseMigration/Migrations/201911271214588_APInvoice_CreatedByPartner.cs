namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APInvoice_CreatedByPartner : DbMigration
    {
        public override void Up()
        {
           
            AddColumn("dbo.APInvoices", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
           
        }
        
        public override void Down()
        {
             
            DropColumn("dbo.APInvoices", "CreatedByPartner");
            
        }
    }
}
