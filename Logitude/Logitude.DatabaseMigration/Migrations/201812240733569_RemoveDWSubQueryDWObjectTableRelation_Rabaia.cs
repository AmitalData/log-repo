namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDWSubQueryDWObjectTableRelation_Rabaia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DWSubQueries", "DWFactTableCode", "dbo.DWObjectTables");
            DropIndex("dbo.DWSubQueries", new[] { "DWFactTableCode" });
            //AddColumn("dbo.DWSubQueries", "DWObjectTable_Code", c => c.String(maxLength: 50, unicode: false));
            //AlterColumn("dbo.DWSubQueries", "DWFactTableCode", c => c.String());
            //CreateIndex("dbo.DWSubQueries", "DWObjectTable_Code");
            //AddForeignKey("dbo.DWSubQueries", "DWObjectTable_Code", "dbo.DWObjectTables", "Code");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.DWSubQueries", "DWObjectTable_Code", "dbo.DWObjectTables");
            //DropIndex("dbo.DWSubQueries", new[] { "DWObjectTable_Code" });
            //AlterColumn("dbo.DWSubQueries", "DWFactTableCode", c => c.String(maxLength: 50, unicode: false));
            //DropColumn("dbo.DWSubQueries", "DWObjectTable_Code");
            CreateIndex("dbo.DWSubQueries", "DWFactTableCode");
            AddForeignKey("dbo.DWSubQueries", "DWFactTableCode", "dbo.DWObjectTables", "Code");
        }
    }
}
