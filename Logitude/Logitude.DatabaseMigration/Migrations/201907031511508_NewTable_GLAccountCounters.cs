namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTable_GLAccountCounters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GLAccountCounters",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Prefix = c.String(nullable: false, maxLength: 15, unicode: false),
                        StartNumber = c.Int(nullable: false),
                        CurrentNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.WithholdingTaxDeductionTypes", "LocalName", c => c.String(nullable: false, maxLength: 120));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WithholdingTaxDeductionTypes", "LocalName", c => c.String(nullable: false, maxLength: 60));
            DropTable("dbo.GLAccountCounters");
        }
    }
}
