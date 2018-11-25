namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubQueries_Rabaia : DbMigration
    {
        public override void Up()
        {
            Sql("alter table dbo.DWQueries drop constraint [FK_dbo.DWQuerys_dbo.DWObjectTables_DWObjectTableCode]");
            DropForeignKey("dbo.DWQueries", "DWObjectTableCode", "dbo.DWObjectTables");
            DropIndex("dbo.DWQueries", new[] { "DWObjectTableCode" });
            CreateTable(
                "dbo.DWSubQueries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Tenant = c.Int(nullable: false),
                        DWFactTableCode = c.String(maxLength: 50, unicode: false),
                        DWQueryId = c.String(maxLength: 15, unicode: false),
                        SQLString = c.String(),
                        FiltersXML = c.String(),
                        ColumnsXML = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DWObjectTables", t => t.DWFactTableCode)
                .ForeignKey("dbo.DWQueries", t => t.DWQueryId)
                .Index(t => t.DWFactTableCode)
                .Index(t => t.DWQueryId);
            
            DropColumn("dbo.DWQueries", "DWObjectTableCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DWQueries", "DWObjectTableCode", c => c.String(nullable: false, maxLength: 50, unicode: false));
            DropForeignKey("dbo.DWSubQueries", "DWQueryId", "dbo.DWQueries");
            DropForeignKey("dbo.DWSubQueries", "DWFactTableCode", "dbo.DWObjectTables");
            DropIndex("dbo.DWSubQueries", new[] { "DWQueryId" });
            DropIndex("dbo.DWSubQueries", new[] { "DWFactTableCode" });
            DropTable("dbo.DWSubQueries");
            CreateIndex("dbo.DWQueries", "DWObjectTableCode");
            AddForeignKey("dbo.DWQueries", "DWObjectTableCode", "dbo.DWObjectTables", "Code");
        }
    }
}
