namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerOpenFileAmountMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerOpenFilesAmounts",
                c => new
                    {
                        CustomerId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TotalOpenFilesAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            //AddColumn("dbo.MenuButtons", "LabelTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            //AddColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            //AddColumn("dbo.Translations", "TextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            //AlterColumn("dbo.ObjectFieldValidations", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerOpenFilesAmounts", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CustomerOpenFilesAmounts", new[] { "CustomerId" });
            AlterColumn("dbo.ObjectFieldValidations", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            DropColumn("dbo.Translations", "TextCodeCode");
            DropColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode");
            DropColumn("dbo.MenuButtons", "LabelTextCodeCode");
            DropTable("dbo.CustomerOpenFilesAmounts");
        }
    }
}
