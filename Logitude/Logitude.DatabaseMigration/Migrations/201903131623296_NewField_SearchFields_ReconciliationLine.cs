namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_SearchFields_ReconciliationLine : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.NumberFormats",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(),
            //            SearchFields = c.String(),
            //        })
            //    .PrimaryKey(t => t.Code);

            //AddColumn("dbo.Tenants", "NumberFormatCode", c => c.String(maxLength: 128));
            //AddColumn("dbo.ChargesTypes", "IsImport", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ChargesTypes", "IsDomestic", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ChargesTypes", "IsExport", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ChargesTypes", "IsDrop", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ARPayments", "IsFullAccounting", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReconciliationLines", "SearchFields", c => c.String(maxLength: 4000));
            //AlterColumn("dbo.ReconcileExternalPageLines", "Reference", c => c.String(maxLength: 30));
            //CreateIndex("dbo.Tenants", "NumberFormatCode");
            //AddForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats", "Code");
        }

        public override void Down()
        {
            //DropForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats");
            //DropIndex("dbo.Tenants", new[] { "NumberFormatCode" });
            //AlterColumn("dbo.ReconcileExternalPageLines", "Reference", c => c.String(maxLength: 30, unicode: false));
            DropColumn("dbo.ReconciliationLines", "SearchFields");
            //DropColumn("dbo.ARPayments", "IsFullAccounting");
            //DropColumn("dbo.ChargesTypes", "IsDrop");
            //DropColumn("dbo.ChargesTypes", "IsExport");
            //DropColumn("dbo.ChargesTypes", "IsDomestic");
            //DropColumn("dbo.ChargesTypes", "IsImport");
            //DropColumn("dbo.Tenants", "NumberFormatCode");
            //DropTable("dbo.NumberFormats");
        }
    }
}
