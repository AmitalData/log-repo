namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_CustomerFiled_To_InterestReport : DbMigration
    {
        public override void Up() 
        {
            AddColumn("dbo.InterestReports", "CustomerId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.InterestReports", "CustomerId");
            AddForeignKey("dbo.InterestReports", "CustomerId", "dbo.Cards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestReports", "CustomerId", "dbo.Cards");
            DropIndex("dbo.InterestReports", new[] { "CustomerId" });
            DropColumn("dbo.InterestReports", "CustomerId");
        }
    }
}
