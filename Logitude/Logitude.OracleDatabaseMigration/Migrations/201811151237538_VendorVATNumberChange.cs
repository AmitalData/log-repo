namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorVATNumberChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.CustomsVendors", "VATNumber", c => c.String(maxLength: 25, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.CustomsVendors", "VATNumber", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
