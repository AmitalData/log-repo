namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationWCOXmlToshipmentAdditionalCloudData_Rabaia : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Customers", "ActivationDate", c => c.DateTime());
            //AddColumn("dbo.Customers", "InactiveDate", c => c.DateTime());
            //AddColumn("dbo.Customers", "ActivationRequestDate", c => c.DateTime());
            //AddColumn("dbo.Customers", "ActivatedByUserId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Customers", "SetAsInactiveByUserId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Customers", "ActivationRequestedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "DeclarationWCOXml", c => c.String());
            //CreateIndex("dbo.Customers", "ActivatedByUserId");
            //CreateIndex("dbo.Customers", "SetAsInactiveByUserId");
            //CreateIndex("dbo.Customers", "ActivationRequestedByUserId");
            //AddForeignKey("dbo.Customers", "ActivatedByUserId", "dbo.Users", "Id");
            //AddForeignKey("dbo.Customers", "ActivationRequestedByUserId", "dbo.Users", "Id");
            //AddForeignKey("dbo.Customers", "SetAsInactiveByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Customers", "SetAsInactiveByUserId", "dbo.Users");
            //DropForeignKey("dbo.Customers", "ActivationRequestedByUserId", "dbo.Users");
            //DropForeignKey("dbo.Customers", "ActivatedByUserId", "dbo.Users");
            //DropIndex("dbo.Customers", new[] { "ActivationRequestedByUserId" });
            //DropIndex("dbo.Customers", new[] { "SetAsInactiveByUserId" });
            //DropIndex("dbo.Customers", new[] { "ActivatedByUserId" });
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "DeclarationWCOXml");
            //DropColumn("dbo.Customers", "ActivationRequestedByUserId");
            //DropColumn("dbo.Customers", "SetAsInactiveByUserId");
            //DropColumn("dbo.Customers", "ActivatedByUserId");
            //DropColumn("dbo.Customers", "ActivationRequestDate");
            //DropColumn("dbo.Customers", "InactiveDate");
            //DropColumn("dbo.Customers", "ActivationDate");
        }
    }
}
