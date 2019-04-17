namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Field_TariffLine_DestinationPortId : DbMigration
    {
        public override void Up()
        {

            CreateIndex("dbo.Tariffs", "SellerId");
            AddForeignKey("dbo.Tariffs", "SellerId", "dbo.Cards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tariffs", "SellerId", "dbo.Cards");
            DropIndex("dbo.Tariffs", new[] { "SellerId" });

        }
    }
}
