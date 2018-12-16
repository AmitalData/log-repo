namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddCardToCustomsShippers : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CustomsShippers", "Id");
            AddForeignKey("dbo.CustomsShippers", "Id", "dbo.Cards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomsShippers", "Id", "dbo.Cards");
            DropIndex("dbo.CustomsShippers", new[] { "Id" });
        }
    }
}
