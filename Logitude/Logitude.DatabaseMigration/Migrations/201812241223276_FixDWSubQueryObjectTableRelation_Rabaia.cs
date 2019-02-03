namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDWSubQueryObjectTableRelation_Rabaia : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.DWSubQueries", "DWObjectTable_Code", "dbo.DWObjectTables");
            //DropIndex("dbo.DWSubQueries", new[] { "DWObjectTable_Code" });
            //DropColumn("dbo.DWSubQueries", "DWObjectTable_Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DWSubQueries", "DWObjectTable_Code", c => c.String(maxLength: 50, unicode: false));
            CreateIndex("dbo.DWSubQueries", "DWObjectTable_Code");
            AddForeignKey("dbo.DWSubQueries", "DWObjectTable_Code", "dbo.DWObjectTables", "Code");
        }
    }
}
