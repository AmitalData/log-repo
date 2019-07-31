namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TariffLine_Index_Add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "Index", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffLines", "Index");
        }
    }
}
