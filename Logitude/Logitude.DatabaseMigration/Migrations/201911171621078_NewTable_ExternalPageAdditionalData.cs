namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTable_ExternalPageAdditionalData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExternalPageAdditionalDatas",
                c => new
                    {
                        ObjectTableId = c.String(nullable: false, maxLength: 15, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        LastPageNumber = c.String(maxLength: 15, unicode: false),
                        LastPageEndDate = c.DateTime(),
                        LastPageCloseBalance = c.Decimal(precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.ObjectTableId, t.EntityId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExternalPageAdditionalDatas");
        }
    }
}
