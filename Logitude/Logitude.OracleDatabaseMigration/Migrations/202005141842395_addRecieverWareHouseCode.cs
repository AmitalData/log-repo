namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRecieverWareHouseCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Consignments", "RecieverWareHouseCode", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("Customs.Consignments", "RecieverWareHouseCode");
            AddForeignKey("Customs.Consignments", "RecieverWareHouseCode", "Customs.DeliverySiteTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Consignments", "RecieverWareHouseCode", "Customs.DeliverySiteTypes");
            DropIndex("Customs.Consignments", new[] { "RecieverWareHouseCode" });
            DropColumn("Customs.Consignments", "RecieverWareHouseCode");
        }
    }
}
