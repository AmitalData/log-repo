namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTariffProductsColumnName : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TariffProducts", "LocalName", c => c.String(maxLength: 100));
            //DropColumn("dbo.TariffProducts", "LacalName");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.TariffProducts", "LacalName", c => c.String(maxLength: 100));
            //DropColumn("dbo.TariffProducts", "LocalName");
        }
    }
}
